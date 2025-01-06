using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PremFEPost.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Azure;
using OfficeOpenXml;

namespace PremFEPost.Pages
{
    [Authorize]
    public class BatchItemsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public int BatchId { get; set; }
        public IList<TranDetails> BatchItems { get; set; }
        
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10; // Or your desired page size
        public int TotalPages { get; set; }
        public int Pending { get; set; }
        public int Success { get; set; }
        public int Failed { get; set; }
        public int Allrecords { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } // Search term for filtering results

        public BatchItemsModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetDownloadExcel(int id, string searchTerm = null)
        {
            var query = _dbContext.TranDetails.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.Transactionreference.Contains(searchTerm)
                                    || t.Narration.Contains(searchTerm));
            }

            var batchItems = await query.Where(t => t.BatchID == id.ToString()).ToListAsync();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("BatchItems");

                // Write data to the worksheet (adjust column headers and data mapping)
                int row = 1;
                worksheet.Cells[row, 1].Value = "Tran Reference";
                worksheet.Cells[row, 2].Value = "Currency";
                worksheet.Cells[row, 3].Value = "Amount";
                worksheet.Cells[row, 4].Value = "DR Account";
                worksheet.Cells[row, 5].Value = "CR Account";
                worksheet.Cells[row, 6].Value = "Narration";
                worksheet.Cells[row, 7].Value = "Status";
                worksheet.Cells[row, 8].Value = "Message";
                worksheet.Cells[row, 9].Value = "Date Time";
                worksheet.Cells[row, 10].Value = "Reference";

                foreach (var item in batchItems)
                {
                    row++;
                    worksheet.Cells[row, 1].Value = item.Transactionreference;
                    worksheet.Cells[row, 2].Value = item.Currency;
                    worksheet.Cells[row, 3].Value = item.Amount;
                    worksheet.Cells[row, 4].Value = item.AccountNo;
                    worksheet.Cells[row, 5].Value = item.DestinationAccount;
                    worksheet.Cells[row, 6].Value = item.Narration;
                    worksheet.Cells[row, 7].Value = item.Status;
                    worksheet.Cells[row, 8].Value = item.ResponseMessage;
                    worksheet.Cells[row, 9].Value = item.TranDate;
                    worksheet.Cells[row, 10].Value = item.Reference;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Batch_{id}_Data.xlsx");
            }
        }

        public async Task<IActionResult> OnGetDownloadAllExcel(int id)
        {
            var batchItems = await _dbContext.TranDetails.Where(t => t.BatchID == id.ToString()).ToListAsync();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("BatchItems");

                // Write data to the worksheet (adjust column headers and data mapping)
                int row = 1;
                worksheet.Cells[row, 1].Value = "Tran Reference";
                worksheet.Cells[row, 2].Value = "Currency";
                worksheet.Cells[row, 3].Value = "Amount";
                worksheet.Cells[row, 4].Value = "DR Account";
                worksheet.Cells[row, 5].Value = "CR Account";
                worksheet.Cells[row, 6].Value = "Narration";
                worksheet.Cells[row, 7].Value = "Status";
                worksheet.Cells[row, 8].Value = "Message";
                worksheet.Cells[row, 9].Value = "Date Time";
                worksheet.Cells[row, 10].Value = "Reference";

                foreach (var item in batchItems)
                {
                    row++;
                    worksheet.Cells[row, 1].Value = item.Transactionreference;
                    worksheet.Cells[row, 2].Value = item.Currency;
                    worksheet.Cells[row, 3].Value = item.Amount;
                    worksheet.Cells[row, 4].Value = item.AccountNo;
                    worksheet.Cells[row, 5].Value = item.DestinationAccount;
                    worksheet.Cells[row, 6].Value = item.Narration;
                    worksheet.Cells[row, 7].Value = item.Status;
                    worksheet.Cells[row, 8].Value = item.ResponseMessage;
                    worksheet.Cells[row, 9].Value = item.TranDate;
                    worksheet.Cells[row, 10].Value = item.Reference;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Batch_{id}_All_Data.xlsx");
            }
        }

        public async Task OnGetAsync(int id, int pageIndex = 0)
        {
            BatchId = id;
            PageIndex = pageIndex;

            // Fetch the total count of items in the batch
            var totalItems = await _dbContext.TranDetails
                .Where(i => i.BatchID == BatchId.ToString())
                .CountAsync();

            var queryy =  _dbContext.TranDetails
                .Where(i => i.BatchID == BatchId.ToString())
                .AsQueryable();

            Pending = queryy.Where(b=> b.Status == "Pending").Count();
            Failed = queryy.Where(b => b.Status == "Failed").Count();
            Success = queryy.Where(b => b.Status == "Success").Count();
            Allrecords = totalItems;
            TotalPages = (int)Math.Ceiling(decimal.Divide(totalItems, PageSize));

            // Fetch the paginated list of items
            //BatchItems = await _dbContext.TranDetails
            //    .Where(i => i.BatchID == BatchId.ToString())
            //    .Skip(PageIndex * PageSize)
            //    .Take(PageSize)
            //    .ToListAsync();
     
            BatchItems = await _dbContext.TranDetails
    .Where(i => i.BatchID == BatchId.ToString()
            && (string.IsNullOrEmpty(SearchTerm)
                || i.Transactionreference.Contains(SearchTerm)|| i.Narration.Contains(SearchTerm))) // Replace YourProperty with the property to search
    .Skip(PageIndex * PageSize)
    .Take(PageSize)
    .ToListAsync();

        }
    }
}