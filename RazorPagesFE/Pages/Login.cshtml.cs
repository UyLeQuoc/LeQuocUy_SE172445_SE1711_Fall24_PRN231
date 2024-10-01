using BusinessObjects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace RazorPagesFE.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync("http://localhost:5178/odata/SystemAccounts/Login", new { AccountEmail = Email, AccountPassword = Password });

            if (response.IsSuccessStatusCode)
            {
                var account = await response.Content.ReadFromJsonAsync<SystemAccount>();

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.AccountName),
                new Claim(ClaimTypes.Email, account.AccountEmail),
                new Claim(ClaimTypes.Role, account.AccountRole == 0 ? "Admin" : "Staff")
            };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
