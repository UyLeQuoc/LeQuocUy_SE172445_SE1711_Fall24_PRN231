using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace RazorPagesFE.Pages.TagPage
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public TagDTO Tag { get; set; } = new TagDTO();

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var token = HttpContext.Session.GetString("JWTToken");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var jsonContent = JsonConvert.SerializeObject(Tag);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://localhost:5178/odata/Tags", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Create Successful";
                    return RedirectToPage("./Index");
                }
                else
                {
                    Message = "Failed to create tag.";
                    return Page();
                }
            }
        }
    }
}
