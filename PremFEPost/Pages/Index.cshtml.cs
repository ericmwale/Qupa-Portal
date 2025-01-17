using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using PremFEPost.Data;
using Microsoft.AspNetCore.Authorization;
using PremFEPost.Areas.Identity.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;


namespace PremFEPost.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public DateTime? SelectedDate { get; set; } = DateTime.Today; // Default to today
        public List<TransactionStat> TransactionStats { get; set; } // Change this to TransactionStat

        public IActionResult OnGet()
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to the login page
                return RedirectToPage("/Identity/Account/Login", new { ReturnUrl = Url.Content("/Identity/Account/Login") });
            }

            // Load default stats
           // LoadTransactionStats(DateTime.Today);
            return Page(); // Return the page after loading stats
        }

        public void OnPost()
        {
            // Load stats based on the selected date
            LoadTransactionStats(SelectedDate ?? DateTime.Today);
        }

        private void LoadTransactionStats(DateTime dateToUse)
        {
           
            TransactionStats = _context.LoanDetails
            .AsEnumerable() // Load data into memory
            .Where(t => decimal.TryParse(t.Amount, NumberStyles.Any, CultureInfo.InvariantCulture, out _)) // Ensure Amount is numeric
            .Where(t => DateTime.ParseExact(t.TranDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).Date == dateToUse.Date)
            .GroupBy(t => new
            {
                t.Currency,
                t.Status,
                TransactionDate = DateTime.ParseExact(t.TranDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).Date,
                t.BatchID
            })
            .Select(g => new TransactionStat
            {
                Currency = g.Key.Currency,
                Status = g.Key.Status,
                TransactionDate = g.Key.TransactionDate,
                BatchID = g.Key.BatchID,
                TotalRecords = g.Count(),
                TotalAmount = g.Sum(t => decimal.Parse(t.Amount, CultureInfo.InvariantCulture))
            })
            .OrderBy(r => r.TransactionDate)
            .ThenBy(r => r.Currency)
            .ThenBy(r => r.Status)
            .ToList();
            }
    }
}