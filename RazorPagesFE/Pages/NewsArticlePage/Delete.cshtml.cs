using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.NewsArticlePage
{
    public class DeleteModel : PageModel
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

                    var response = await httpClient.GetAsync($"http://localhost:5178/odata/NewsArticles({id})");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        NewsArticle = JsonConvert.DeserializeObject<NewsArticle>(jsonString);
                    }
                    else
                    {
                        return NotFound();
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
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

                    var response = await httpClient.DeleteAsync($"http://localhost:5178/odata/NewsArticles({id})");

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "News Article deleted successfully!";
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        Message = "Failed to delete news article.";
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
