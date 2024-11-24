using Microsoft.AspNetCore.Identity;
using Research_Software_Dev.Models.Researchers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Pages
{
    public class MyStudiesModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<Researcher> _userManager;

        public MyStudiesModel(HttpClient httpClient, UserManager<Researcher> userManager)
        {
            _httpClient = httpClient;
            _userManager = userManager;
        }

        public List<Study> Studies { get; set; } = new List<Study>();

        public async Task OnGetAsync()
        {
            var researcherId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(researcherId))
            {
                return;
            }

            var response = await _httpClient.GetFromJsonAsync<List<Study>>("https://localhost:7018/api/researcher/studies");

            if (response != null)
            {
                Studies = response;
            }
        }
    }
}
