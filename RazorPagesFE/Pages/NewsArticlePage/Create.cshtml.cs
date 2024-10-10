using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace RazorPagesFE.Pages.NewsArticlePage
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = new NewsArticle();

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
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

                    // Fetch Categories
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
                return await OnGetAsync();
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

                    var response = await httpClient.PostAsync("http://localhost:5178/odata/NewsArticles", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "News Article created successfully!";
                        return RedirectToPage("./Index");
                    }
                    else
                    {
                        Message = "Failed to create news article.";
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
