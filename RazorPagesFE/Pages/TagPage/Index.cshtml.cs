using BusinessObjects;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.TagPage
{
    public class IndexModel : PageModel
    {
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public string Message { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (TempData["SuccessMessage"] != null)
            {
                Message = TempData["SuccessMessage"].ToString();
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync("http://localhost:5178/odata/Tags");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Tags = JsonConvert.DeserializeObject<ODataResponse<Tag>>(jsonString).Value;
                }
            }
        }
    }
}
