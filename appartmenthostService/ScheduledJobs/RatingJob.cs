using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService
{
    // A simple scheduled job which can be invoked manually by submitting an HTTP
    // POST request to the path "/jobs/sample".

    public class RatingJob : ScheduledJob
    {
        readonly apartmenthostContext _context = new apartmenthostContext();
        public override Task ExecuteAsync()
        {
            var profiles = _context.Profile;
            foreach (var profile in profiles)
            {
                var reviews = _context.Reviews.Where(rev => rev.ToUserId == profile.Id && rev.Rating > 0);
                var count = reviews.Count();
                if (count > 0)
                { 
                profile.RatingCount = count;
                profile.Rating = reviews.Average(x => (Decimal) x.Rating);
                profile.Score = reviews.Sum(x => x.Rating);
                }

            }

            _context.SaveChanges();
            return Task.FromResult(true);
        }
    }
}