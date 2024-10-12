using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace RazorPagesFE.Pages.Profile
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public SystemAccountDTO SystemAccount { get; set; }

        public string Message { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var accountId = HttpContext.Session.GetString("AccountId");
                if (string.IsNullOrEmpty(accountId))
                {
                    return RedirectToPage("/NotAuthorized");
                }

                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToPage("/NotAuthorized");
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await httpClient.GetAsync($"http://localhost:5178/odata/SystemAccounts/{accountId}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        SystemAccount = JsonConvert.DeserializeObject<SystemAccountDTO>(jsonString);
                    }
                    else
                    {
                        Message = "Failed to load profile.";
                    }
                }

                return Page();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToPage("/NotAuthorized");
                }

                var accountId = HttpContext.Session.GetString("AccountId");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var jsonContent = JsonConvert.SerializeObject(SystemAccount);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync($"http://localhost:5178/odata/SystemAccounts/{accountId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Profile updated successfully!";
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                        throw new Exception(errorResponse.Error.Message);
                    }
                }

                return Page();
            }
            catch (Exception e)
            {
                Message = e.Message;
                return Page();
            }
        }
    }
}
