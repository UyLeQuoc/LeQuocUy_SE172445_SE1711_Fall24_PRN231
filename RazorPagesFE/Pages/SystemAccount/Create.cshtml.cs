using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesFE.Pages.SystemAccount
{
    public class CreateModel : PageModel
    {
        private readonly FunewsManagementFall2024Context _context;

        public CreateModel(FunewsManagementFall2024Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SystemAccounts.Add(SystemAccount);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
