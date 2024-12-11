using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Research_Software_Dev.Data;
using Research_Software_Dev.Models.Participants;
using Research_Software_Dev.Models.Researchers;
using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Services
{
    public class Helper
    {
        //check permissions
        public static bool IsAuthorized(ClaimsPrincipal user, string[] requiredRoles)
        {
            var roles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return roles.Any(role => requiredRoles.Contains(role));
        }

        //return related studies for dropdown
        public static async Task<List<Study>> GetStudiesForResearcher(ApplicationDbContext context, string researcherId)
        {
            return await context.ResearcherStudies
                .Where(rs => rs.ResearcherId == researcherId)
                .Include(rs => rs.Study)
                .Select(rs => rs.Study)
                .ToListAsync();
        }




        //check if logged in
        public static bool IsLoggedIn(UserManager<Researcher> userManager, ClaimsPrincipal user)
        {
            var researcherId = userManager.GetUserId(user);
            return !string.IsNullOrEmpty(researcherId);
        }

        //check researcher is in study
        public static async Task<bool> IsResearcherInStudy(ApplicationDbContext context, string researcherId, string studyId)
        {
            return await context.ResearcherStudies
                .AnyAsync(rs => rs.ResearcherId == researcherId && rs.StudyId == studyId);
        }

        //check particpant is in study
        public static async Task<bool> IsParticipantInStudy(ApplicationDbContext context, string firstName, string lastName, string studyId)
        {
            return await context.Participants
                .Where(p => p.ParticipantFirstName == firstName && p.ParticipantLastName == lastName)
                .Join(context.ParticipantStudies, p => p.ParticipantId, ps => ps.ParticipantId, (p, ps) => new { p, ps })
                .AnyAsync(joined => joined.ps.StudyId == studyId);
        }

        //get a GUID
        public static string GetGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
