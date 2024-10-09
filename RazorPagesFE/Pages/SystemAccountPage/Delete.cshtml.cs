using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.SystemAccountPage
{
    public class DeleteModel : PageModel
    {
        public SystemAccount SystemAccount { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/NotAuthorized");
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync($"http://localhost:5178/odata/SystemAccounts/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    SystemAccount = JsonConvert.DeserializeObject<SystemAccount>(jsonString);
                }
                else
                {
                    Message = "Failed to load account details.";
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(short? id)
        {

            using (var httpClient = new HttpClient())
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToPage("/NotAuthorized");
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.DeleteAsync($"http://localhost:5178/odata/SystemAccounts/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Account deleted successfully!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                    TempData["SuccessMessage"] = $"Failed to delete account: {errorResponse.Error.Message}";
                }
            }

            return RedirectToPage("./Index");


        }
    }
    public class ErrorResponse
    {
        public ErrorDetail Error { get; set; }
    }

    public class ErrorDetail
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
