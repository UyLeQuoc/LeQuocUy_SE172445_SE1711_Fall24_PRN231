using DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var loginRequest = new
            {
                Email = Email,
                Password = Password
            };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync("http://localhost:5178/odata/SystemAccounts/Login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

                    // Lưu JWT token vào localStorage (hoặc sessionStorage)
                    HttpContext.Session.SetString("JWTToken", result.Token);
                    HttpContext.Session.SetString("Role", result.Role.ToString());
                    HttpContext.Session.SetString("AccountId", result.AccountId.ToString());

                    // Tạo ClaimsPrincipal để lưu trữ thông tin đăng nhập
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginRequest.Email),
                        new Claim("Role", result.Role.ToString()),
                        new Claim("AccountId", result.AccountId.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Lưu thông tin đăng nhập vào cookie
                    await HttpContext.SignInAsync("CookieAuth", principal);

                    return RedirectToPage("/Index");
                }

                Message = "Invalid email or password";
            }

            return Page();
        }
    }
}
