using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Research_Software_Dev.Data;
using System.Threading.Tasks;

[Authorize(Roles = "Study Admin")]
public class SeedDataModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public SeedDataModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [TempData]
    public string Message { get; set; }

    public void OnGet()
    {
        // Initial page load
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var seeder = new DataSeeder(_context);
        try
        {
            await seeder.SeedDataAsync();
            Message = "Seeding completed successfully.";
        }
        catch (Exception ex)
        {
            Message = $"Error during seeding: {ex.Message}";
        }

        return RedirectToPage();
    }
}
