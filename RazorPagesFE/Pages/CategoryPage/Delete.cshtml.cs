using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RazorPagesFE.Pages.CategoryPage
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Category Category { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
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

                    var response = await httpClient.GetAsync($"http://localhost:5178/odata/Categories({id})");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        Category = JsonConvert.DeserializeObject<Category>(jsonString);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                        throw new Exception(errorResponse.Error.Message);
                    }
                }

                return Page();
            }
            catch (Exception e)
            {
                TempData["SuccessMessage"] = e.Message;
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(short? id)
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

                    var response = await httpClient.DeleteAsync($"http://localhost:5178/odata/Categories/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Category deleted successfully!";
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
                TempData["SuccessMessage"] = e.Message;
                return RedirectToPage("./Index");
            }
        }
    }
}
