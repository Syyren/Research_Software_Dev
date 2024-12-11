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

        //check particpant is in study by name
        public static async Task<bool> IsParticipantInStudyByName(ApplicationDbContext context, string firstName, string lastName, string studyId)
        {
            return await context.Participants
                .Where(p => p.ParticipantFirstName == firstName && p.ParticipantLastName == lastName)
                .Join(context.ParticipantStudies, p => p.ParticipantId, ps => ps.ParticipantId, (p, ps) => new { p, ps })
                .AnyAsync(joined => joined.ps.StudyId == studyId);
        }

        //check particpant is in study by Id
        public static async Task<bool> IsParticipantInStudyById(ApplicationDbContext context, string participantId, string studyId)
        {
            return await context.ParticipantStudies
                .AnyAsync(ps => ps.ParticipantId == participantId && ps.StudyId == studyId);
        }

        //get participant by Id
        public static async Task<Participant> GetParticipantById(ApplicationDbContext context, string participantId)
        {
            return await context.Participants
                .FirstOrDefaultAsync(p => p.ParticipantId == participantId);
        }

        //get study by id
        public static async Task<Study> GetStudyById(ApplicationDbContext context, string studyId)
        {
            return await context.Studies
                .FirstOrDefaultAsync(s => s.StudyId == studyId);
        }
        //get ParticipantStudy from participant studies table
        public static async Task<ParticipantStudy> GetParticipantStudy(ApplicationDbContext context, string participantId, string studyId)
        {
            var participantStudy = await context.ParticipantStudies
                .FirstOrDefaultAsync(ps => ps.ParticipantId == participantId && ps.StudyId == studyId);
            return participantStudy;
        }

        //get a GUID
        public static string GetGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
