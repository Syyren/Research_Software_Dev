using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;

namespace Research_Software_Dev.Models.Researchers.Endpoints
{
    public static class GetListOfStudies
    {
        public static void MapGetListOfStudies(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/researcher/studies", async (
                UserManager<Researcher> userManager,
                ApplicationDbContext dbContext,
                HttpContext httpContext) =>
            {

                var researcherId = userManager.GetUserId(httpContext.User);

                if (string.IsNullOrEmpty(researcherId))
                {
                    return Results.Unauthorized();
                }


                //get studies where ResearcherStudies.researcherid matches identity id
                var studies = await dbContext.ResearcherStudies
                    .Where(rs => rs.ResearcherId == researcherId)
                    .Include(rs => rs.Study)
                    .Select(rs => rs.Study)
                    .ToListAsync();

                return Results.Ok(studies);
            });
        }
    }
}