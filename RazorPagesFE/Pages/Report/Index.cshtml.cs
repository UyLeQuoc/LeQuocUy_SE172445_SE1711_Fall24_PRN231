using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.Report
{
    public class IndexModel : PageModel
    {
        public List<NewsArticleResponse> NewsArticles { get; set; } = new List<NewsArticleResponse>();
        public bool ReportGenerated { get; set; } = false;

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!StartDate.HasValue || !EndDate.HasValue)
            {
                return Page();
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

                    string filter = $"$filter=CreatedDate ge {StartDate:yyyy-MM-dd} and CreatedDate le {EndDate:yyyy-MM-dd}&$expand=Category";
                    string orderBy = "$orderby=CreatedDate desc";
                    string queryString = $"{filter}&{orderBy}";

                    var response = await httpClient.GetAsync($"http://localhost:5178/odata/NewsArticles?{queryString}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var odataResponse = JsonConvert.DeserializeObject<ODataResponse<NewsArticleResponse>>(jsonString);

                        NewsArticles = odataResponse.Value;
                    }
                    else
                    {
                        Message = "Failed to fetch report data.";
                    }
                }

                ReportGenerated = true;
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
