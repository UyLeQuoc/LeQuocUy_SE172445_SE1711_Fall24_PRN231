using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.CategoryPage
{
    public class IndexModel : PageModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public string Message { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 3;
        public int CurrentPage { get; set; } = 1;

        // Search & Filter parameters
        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "CategoryName";

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

                if (TempData["SuccessMessage"] != null)
                {
                    Message = TempData["SuccessMessage"].ToString();
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Construct OData query with filters, sorting, and paging
                    var query = new List<string>();

                    // Search by Category Name
                    if (!string.IsNullOrEmpty(SearchName))
                    {
                        query.Add($"$filter=contains(CategoryName, '{SearchName}')");
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
                    var response = await httpClient.GetAsync($"http://localhost:5178/odata/Categories?{queryString}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var odataResponse = JsonConvert.DeserializeObject<ODataResponse<Category>>(jsonString);

                        Categories = odataResponse.Value;
                        TotalCount = odataResponse.Count;
                    }
                    else
                    {
                        Categories = new List<Category>();
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

        public class ODataResponse<T>
        {
            [JsonProperty("value")]
            public List<T> Value { get; set; }

            [JsonProperty("@odata.count")]
            public int Count { get; set; }
        }
    }
}
