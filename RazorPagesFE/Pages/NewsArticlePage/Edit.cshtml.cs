using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace RazorPagesFE.Pages.NewsArticlePage
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public string Message { get; set; } = string.Empty;

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

                    // Fetch categories
                    var response = await httpClient.GetAsync("http://localhost:5178/odata/Categories?$filter=IsActive eq true");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var odataResponse = JsonConvert.DeserializeObject<ODataResponse<Category>>(jsonString);
                        Categories = odataResponse.Value;
                    }

                    // Fetch Tags
                    response = await httpClient.GetAsync("http://localhost:5178/odata/Tags");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        Tags = JsonConvert.DeserializeObject<ODataResponse<Tag>>(jsonString).Value;
                    }

                    // Fetch news article details
                    response = await httpClient.GetAsync($"http://localhost:5178/odata/NewsArticles({id})?$expand=Tags");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        NewsArticle = JsonConvert.DeserializeObject<NewsArticle>(jsonString);

                        SelectedTagIds = NewsArticle.Tags.Select(t => t.TagId).ToList();
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(NewsArticle.NewsArticleId);
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

                    NewsArticle.Tags = SelectedTagIds.Select(tagId => new Tag { TagId = tagId }).ToList();

                    var jsonContent = JsonConvert.SerializeObject(NewsArticle);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync($"http://localhost:5178/odata/NewsArticles/{NewsArticle.NewsArticleId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "News Article updated successfully!";
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        Message = "Failed to update news article.";
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
