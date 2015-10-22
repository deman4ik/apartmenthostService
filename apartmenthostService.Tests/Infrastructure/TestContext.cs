using System.Data.Entity;
using apartmenthostService.Models;

namespace apartmenthostService.Tests.Infrastructure
{
    public class TestContext : IApartmenthostContext
    {

        public TestContext()
        {
            this.Apartments = new TestDbSet<Apartment>();
            this.Cards = new TestDbSet<Card>();
            this.Dates = new TestDbSet<CardDates>();
            this.Genders = new TestDbSet<CardGenders>();
            this.Reservations = new TestDbSet<Reservation>();
            this.Reviews = new TestDbSet<Review>();
            this.Users = new TestDbSet<User>();
            this.Accounts = new TestDbSet<Account>();
            this.Profile = new TestDbSet<Profile>();
            this.Notifications = new TestDbSet<Notification>();
            this.Pictures = new TestDbSet<Picture>();
            this.Favorites = new TestDbSet<Favorite>();
            this.Article = new TestDbSet<Article>();
        }

        public DbSet<Apartment> Apartments { get; }
        public DbSet<Card> Cards { get; }
        public DbSet<CardDates> Dates { get; }
        public DbSet<CardGenders> Genders { get; }
        public DbSet<Reservation> Reservations { get; }
        public DbSet<Review> Reviews { get; }
        public DbSet<User> Users { get; }
        public DbSet<Account> Accounts { get; }
        public DbSet<Profile> Profile { get; }
        public DbSet<Notification> Notifications { get; }
        public DbSet<Picture> Pictures { get; }
        public DbSet<Favorite> Favorites { get; }
        public DbSet<Article> Article { get; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(object item)
        {

        }

        public void Dispose()
        {

        }
    }
}
