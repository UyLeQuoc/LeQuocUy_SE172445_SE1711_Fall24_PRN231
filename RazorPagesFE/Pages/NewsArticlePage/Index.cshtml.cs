using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.NewsArticlePage
{
    public class IndexModel : PageModel
    {
        public List<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public string Message { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 3;
        public int CurrentPage { get; set; } = 1;

        // Search & Filter parameters
        [BindProperty(SupportsGet = true)]
        public string SearchTitle { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedTagId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "NewsTitle";

        [BindProperty(SupportsGet = true)]
        public bool Ascending { get; set; } = true;

        public async Task<IActionResult> OnGetAsync(int currentPage = 1)
        {
            try
            {
                CurrentPage = currentPage;

                var token = HttpContext.Session.GetString("JWTToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToPage("/NotAuthorized");
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Construct OData query with filters, sorting, and paging
                    var query = new List<string>();

                    // Search by News Title
                    if (!string.IsNullOrEmpty(SearchTitle))
                    {
                        query.Add($"$filter=contains(NewsTitle, '{SearchTitle}')");
                    }

                    // Filter by Tag
                    if (SelectedTagId != null)
                    {
                        query.Add($"$filter=Tags/any(t: t/TagId eq {SelectedTagId})");
                    }

                    // Sorting
                    var order = Ascending ? "asc" : "desc";
                    query.Add($"$orderby={SortBy} {order}");

                    // Paging
                    var skip = (CurrentPage - 1) * PageSize;
                    query.Add($"$top={PageSize}");
                    query.Add($"$skip={skip}");

                    // Count total records
                    query.Add("$count=true");

                    // Build full OData query string
                    var queryString = string.Join("&", query);
                    var response = await httpClient.GetAsync($"http://localhost:5178/odata/NewsArticles?{queryString}&$expand=Category,CreatedBy,Tags");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var odataResponse = JsonConvert.DeserializeObject<ODataResponse<NewsArticle>>(jsonString);

                        NewsArticles = odataResponse.Value;
                        TotalCount = odataResponse.Count;
                    }
                    else
                    {
                        NewsArticles = new List<NewsArticle>();
                    }

                    var tagResponse = await httpClient.GetAsync($"http://localhost:5178/odata/Tags");
                    if (tagResponse.IsSuccessStatusCode)
                    {
                        var tagJsonString = await tagResponse.Content.ReadAsStringAsync();
                        Tags = JsonConvert.DeserializeObject<ODataResponse<Tag>>(tagJsonString).Value;
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
