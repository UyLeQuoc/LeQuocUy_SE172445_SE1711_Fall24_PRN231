using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.NewsArticlePage
{
    public class DeleteModel : PageModel
    {
        public NewsArticleResponse NewsArticle { get; set; }

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

                    var response = await httpClient.GetAsync($"http://localhost:5178/odata/NewsArticles/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        NewsArticle = JsonConvert.DeserializeObject<NewsArticleResponse>(jsonString);
                    }
                    else
                    {
                        Message = "Failed to retrieve news article.";
                        return NotFound();
                    }
                }

                return Page();
            }
            catch (Exception e)
            {
                Message = $"Error: {e.Message}";
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

                    var response = await httpClient.DeleteAsync($"http://localhost:5178/odata/NewsArticles/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "News Article deleted successfully!";
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
                Message = $"Error: {e.Message}";
                TempData["SuccessMessage"] = Message;
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
