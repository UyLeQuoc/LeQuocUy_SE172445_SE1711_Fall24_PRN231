using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.TagPage
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public TagDTO Tag { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync($"http://localhost:5178/odata/Tags/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Tag = JsonConvert.DeserializeObject<TagDTO>(jsonString);
                }
                else
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await httpClient.DeleteAsync($"http://localhost:5178/odata/Tags/{Tag.TagId}");

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                        throw new Exception(errorResponse.Error.Message);
                    }
                }
            }
            catch (Exception e)
            {
                TempData["SuccessMessage"] = e.Message;
                return RedirectToPage("./Index");
            }
        }
    }
}
