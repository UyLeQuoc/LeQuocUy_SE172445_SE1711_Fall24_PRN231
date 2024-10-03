using BusinessObjects;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RazorPagesFE.Pages.SystemAccountPage
{
    public class IndexModel : PageModel
    {
        private readonly FunewsManagementFall2024Context _context;

        public IndexModel(FunewsManagementFall2024Context context)
        {
            _context = context;
        }

        public IList<SystemAccount> SystemAccount { get; set; } = default!;

        public async Task OnGetAsync()
        {
            SystemAccount = await _context.SystemAccounts.ToListAsync();
        }
    }
}
