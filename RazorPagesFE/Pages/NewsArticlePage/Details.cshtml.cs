using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.NewsArticlePage
{
    public class DetailsModel : PageModel
    {
        public NewsArticle NewsArticle { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToPage("/NotAuthorized");
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await httpClient.GetAsync($"http://localhost:5178/odata/NewsArticles({id})?$expand=Category,CreatedBy");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        NewsArticle = JsonConvert.DeserializeObject<NewsArticle>(jsonString);
                    }
                    else
                    {
                        Message = "Failed to load the news article.";
                        return Page();
                    }
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return Page();
        }
    }
}
