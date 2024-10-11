using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages
{
    public class IndexModel : PageModel
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string AccountId { get; set; }
        public string Message { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Login");
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync("http://localhost:5178/CurrentUser");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var currentUser = JsonConvert.DeserializeObject<CurrentUserDTO>(result);

                    // Lưu thông tin người dùng để hiển thị trên trang
                    Email = currentUser.Email;
                    Role = currentUser.Role switch
                    {
                        "0" => "Admin",
                        "1" => "Staff",
                        "2" => "Lecture",
                        _ => "Unknown"
                    };
                    AccountId = currentUser.AccountId;
                }

                return Page();
            }
        }

        public class CurrentUserDTO
        {
            public string Email { get; set; }
            public string Role { get; set; }
            public string AccountId { get; set; }
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Remove("JWTToken");
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("AccountId");

            return RedirectToPage("/Login");
        }

    }
}