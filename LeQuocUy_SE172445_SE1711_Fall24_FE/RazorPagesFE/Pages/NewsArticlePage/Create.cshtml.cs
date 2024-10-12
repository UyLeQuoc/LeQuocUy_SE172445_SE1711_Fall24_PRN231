using DTO;
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
        public NewsArticleResponse NewsArticle { get; set; } = new NewsArticleResponse();

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
        public List<TagDTO> Tags { get; set; } = new List<TagDTO>();

        public string Message { get; set; } = string.Empty;

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
                        var odataResponse = JsonConvert.DeserializeObject<ODataResponse<CategoryDTO>>(jsonString);
                        Categories = odataResponse.Value;
                    }

                    // Fetch Tags
                    response = await httpClient.GetAsync("http://localhost:5178/odata/Tags");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        Tags = JsonConvert.DeserializeObject<ODataResponse<TagDTO>>(jsonString).Value;
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

                    NewsArticle.Tags = SelectedTagIds.Select(tagId => new TagDTO { TagId = tagId }).ToList();

                    var newsArticleDTO = new NewsArticleDTO
                    {
                        NewsArticleId = NewsArticle.NewsArticleId,
                        NewsTitle = NewsArticle.NewsTitle,
                        Headline = NewsArticle.Headline,
                        NewsContent = NewsArticle.NewsContent,
                        CreatedDate = NewsArticle.CreatedDate,
                        NewsSource = NewsArticle.NewsSource,
                        CategoryId = NewsArticle.CategoryId,
                        CreatedById = NewsArticle.CreatedById,
                        NewsStatus = NewsArticle.NewsStatus,
                        ModifiedDate = NewsArticle.ModifiedDate,
                        TagIds = SelectedTagIds
                    };

                    var jsonContent = JsonConvert.SerializeObject(newsArticleDTO);

                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("http://localhost:5178/odata/NewsArticles", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "News Article created successfully!";
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
                Message = e.Message;
            }

            return Page();
        }
    }
}
