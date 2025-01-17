using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Microsoft.JSInterop;
using PremFEPost.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace PremFEPost.Pages
{
    [Authorize]
    public class LoadExcelModel : PageModel
    {

        [BindProperty]
        public bool UploadSuccess { get; set; }
        ApplicationDbContext DbContext;
        const long maxAllowedSize = 1024 * 1024 * 200; // 200 MB
        IJSRuntime JS;
       
       // private readonly UserManager<ApplicationUser> _userManager;

        public LoadExcelModel(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            DbContext = applicationDbContext;
           // _userManager = userManager;
        }

        public int BatchId { get; set; }
        public IList<BatchDetails> BatchDetailsList { get; set; }

        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10; // Or your desired page size
        public int TotalPages { get; set; }
        public int Pending { get; set; }
        public int Success { get; set; }
        public int Failed { get; set; }
        public int Allrecords { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } // Search term for filtering results



        public async Task OnGetAsync(int id, int pageIndex = 0)
        {
            BatchId = id;
            PageIndex = pageIndex;

            // Fetch the total count of items in the batch
            var totalItems = await DbContext.BatchDetails
                .OrderByDescending(i => i.Date)
                .CountAsync();

            var queryy = DbContext.BatchDetails .OrderByDescending(i => i.Date)
                .AsQueryable();

            Pending = queryy.Where(b => b.Status == "Pending").Count();
            Failed = queryy.Where(b => b.Status == "Success").Count();
            Success = queryy.Where(b => b.Status == "Failed").Count();

            Allrecords = totalItems;
            TotalPages = (int)Math.Ceiling(decimal.Divide(totalItems, PageSize));

            // Fetch the paginated list of items
            //BatchItems = await _dbContext.TranDetails
            //    .Where(i => i.BatchID == BatchId.ToString())
            //    .Skip(PageIndex * PageSize)
            //    .Take(PageSize)
            //    .ToListAsync();

            BatchDetailsList = await DbContext.BatchDetails.OrderByDescending(i => i.Date)
    .Where(i => (string.IsNullOrEmpty(SearchTerm)
                || i.AuthorisedBy.Contains(SearchTerm) || i.UploadedBy.Contains(SearchTerm))) // Replace YourProperty with the property to search
    .Skip(PageIndex * PageSize)
    .Take(PageSize)
    .ToListAsync();

        }





        public static string GenerateUniqueReference()
        {
            // Prefix
            string prefix = "ZB";

            // Get current date and time
            DateTime now = DateTime.Now;
            string datePart = now.ToString("yyyyMMddHHmm"); // Format: YYYYMMDDHHMM

            // Generate the random part
            string randomPart = GenerateRandomString(20 - (prefix.Length + datePart.Length));

            // Combine all parts
            return $"{prefix}{datePart}{randomPart}";
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);
                char[] result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    result[i] = chars[randomBytes[i] % chars.Length];
                }

                return new string(result);
            }
        }
        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var batch = await DbContext.BatchDetails.FindAsync(id);
            if (batch != null)
            {
                var userName = User.Identity.Name;
                batch.AuthorisedBy = userName;
                batch.Status = "Approved"; // Change status to Approved
                await DbContext.SaveChangesAsync(); // Save changes to the database
            }
            BatchDetailsList = DbContext.BatchDetails.OrderByDescending(b => b.Date).ToList();
            return RedirectToPage(); // Redirect back to the page
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var batch = await DbContext.BatchDetails.FindAsync(id);
            if (batch != null)
            {
                var userName = User.Identity.Name;
                batch.AuthorisedBy = userName;
                batch.Status = "Rejected"; // Change status to Rejected
                await DbContext.SaveChangesAsync(); // Save changes to the database
            }
            BatchDetailsList = DbContext.BatchDetails.OrderByDescending(b => b.Date).ToList();
            return RedirectToPage(); // Redirect back to the page
        }
        public async Task<IActionResult> OnPostAsync(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                // Handle the error (file not selected)
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user ID
            //var user = await _userManager.FindByIdAsync(userId);
            //var roles = await _userManager.GetRolesAsync(user); // Get roles for the user
           // var UserRole = roles.FirstOrDefault();


            try
            {


                using (var stream = new MemoryStream())
                {
                    await excelFile.OpenReadStream().CopyToAsync(stream);
                    //await excelFile.OpenReadStream(maxAllowedSize).CopyToAsync(stream);
                    stream.Position = 0; // Reset the stream position
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        var userName = User.Identity.Name;
                        var bdetails = new BatchDetails
                        {
                            FileName = excelFile.FileName,
                            Date = DateTime.Now.ToString("yyyyMMdd"),
                           
                            UploadedBy = userName,
                            AuthorisedBy = "",
                            Status = "Pending",
                            Records = rowCount - 1
                        };
                        DbContext.BatchDetails.Add(bdetails);
                        await DbContext.SaveChangesAsync();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var clientDetails = new ClientDetails();
                            var loandetails = new LoanDetails();
                            var nid = worksheet.Cells[row, 10].Text;
                            if(nid != null || nid != "")
                            {
                                clientDetails = new ClientDetails();

                                clientDetails.FileName = bdetails.FileName;
                                clientDetails.BatchId = bdetails.Id.ToString();
                                clientDetails.FirstName = worksheet.Cells[row, 1].Text;
                                clientDetails.MiddleName = worksheet.Cells[row, 2].Text;
                                clientDetails.LastName = worksheet.Cells[row, 3].Text;
                                clientDetails.Gender = worksheet.Cells[row, 4].Text.ToUpper();
                                clientDetails.EmailAddress = worksheet.Cells[row, 5].Text;
                                clientDetails.Address = worksheet.Cells[row, 6].Text;
                                clientDetails.BankName = worksheet.Cells[row, 7].Text;
                                clientDetails.BankAccount = worksheet.Cells[row, 8].Text;
                                clientDetails.MobileNumber = worksheet.Cells[row, 9].Text;
                                clientDetails.NationalID = worksheet.Cells[row, 10].Text;
                                
                                clientDetails.DateOfBirth = DateTime.Parse(worksheet.Cells[row, 11].Text).ToString("yyyyMMdd");
                                clientDetails.DateCreated = DateTime.Now.ToString("yyyyMMdd");
                                DbContext.ClientDetails.Add(clientDetails);
                                await DbContext.SaveChangesAsync();
                                loandetails = new LoanDetails
                                {
                                    FileName = bdetails.FileName,
                                    BatchID = bdetails.Id.ToString(),
                                    ClientID = clientDetails.Id,
                                    LoanType = worksheet.Cells[row, 12].Text,
                                    ProductName = worksheet.Cells[row, 13].Text,
                                    PrincipalAmount = worksheet.Cells[row, 14].Text,
                                    LoanTenure = worksheet.Cells[row, 15].Text,
                                    NumberOfRepayments = worksheet.Cells[row, 16].Text,
                                    InterestRatePerPeriod = worksheet.Cells[row, 17].Text,
                                    expectedDisbursementDate = worksheet.Cells[row, 18].Text,
                                    Status = "Pending",
                                   DateCreated = DateTime.Now.ToString("yyyyMMdd")
                               };
                                DbContext.LoanDetails.Add(loandetails);
                            }
                            else
                            {
                                loandetails = new LoanDetails();
                            }
                            
                        }

                        await DbContext.SaveChangesAsync();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            BatchDetailsList = DbContext.BatchDetails.OrderByDescending(b => b.Date).ToList();
            return Page(); // Return the same page to show the result
        }
    }
}
