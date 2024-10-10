﻿using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.NewsArticlePage
{
    public class IndexModel : PageModel
    {
        public List<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public string Message { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 3;
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string SearchTitle { get; set; }

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

                if (TempData["SuccessMessage"] != null)
                {
                    Message = TempData["SuccessMessage"].ToString();
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
                    var response = await httpClient.GetAsync($"http://localhost:5178/odata/NewsArticles?{queryString}&$expand=Category,CreatedBy");

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
