using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesFE.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var loginRequest = new
            {
                Email,
                Password
            };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync("http://localhost:5178/odata/SystemAccounts/Login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

                    HttpContext.Session.SetString("JWTToken", result.Token);
                    HttpContext.Session.SetString("Role", result.Role.ToString());
                    HttpContext.Session.SetString("AccountId", result.AccountId.ToString());

                    return RedirectToPage("/Index");
                }

                Message = "Invalid email or password";
            }

            return Page();
        }
    }
}
