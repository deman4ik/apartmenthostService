using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Tables;

namespace apartmenthostService.Models
{
    public class ApartmenthostContext : DbContext, IApartmenthostContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //
        // To enable Entity Framework migrations in the cloud, please ensure that the 
        // service name, set by the 'MS_MobileServiceName' AppSettings in the local 
        // Web.config, is the same as the service name when hosted in Azure.
        private const string connectionStringName = "Name=MS_TableConnectionString";

        public ApartmenthostContext()
            : base(connectionStringName)
        {
            Database.SetInitializer<ApartmenthostContext>(null);
        }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardDates> Dates { get; set; }
        public DbSet<CardGenders> Genders { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public void MarkAsModified(object item)
        {
            Entry(item).State = EntityState.Modified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var schema = ServiceSettingsDictionary.GetSchemaName();
            if (!string.IsNullOrEmpty(schema))
            {
                modelBuilder.HasDefaultSchema(schema);
            }
            //modelBuilder.Entity<Apartment>().Property(x => x.Latitude).HasPrecision(20, 20);
            //modelBuilder.Entity<Apartment>().Property(x => x.Longitude).HasPrecision(20, 20);
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

            /***********************
             *       User          *
             ***********************/
            // User + Profile

            modelBuilder.Entity<User>()
                .HasOptional(s => s.Profile)
                .WithRequired(ad => ad.User)
                .WillCascadeOnDelete(false);

            // User + Account
            modelBuilder.Entity<User>()
                .HasMany(s => s.Accounts)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // User + Apartment
            modelBuilder.Entity<User>()
                .HasMany(s => s.Apartments)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // User + Card
            modelBuilder.Entity<User>()
                .HasMany(s => s.Cards)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // User + Favorite
            modelBuilder.Entity<User>()
                .HasMany(s => s.Favorites)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // User + Notification
            modelBuilder.Entity<User>()
                .HasMany(s => s.Notifications)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // User + Reservation
            modelBuilder.Entity<User>()
                .HasMany(s => s.Reservations)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // User + Review
            modelBuilder.Entity<User>()
                .HasMany(s => s.OutReviews)
                .WithRequired(s => s.FromUser)
                .HasForeignKey(s => s.FromUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(s => s.InReviews)
                .WithRequired(s => s.ToUser)
                .HasForeignKey(s => s.ToUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(s => s.Feedbacks)
                .WithOptional(s => s.User)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            //Apartment
            modelBuilder.Entity<Apartment>()
                .HasMany(s => s.Cards)
                .WithOptional(s => s.Apartment)
                .HasForeignKey(s => s.ApartmentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Apartment>()
                .HasMany(s => s.Pictures)
                .WithMany(c => c.Apartments)
                .Map(cs =>
                {
                    cs.MapLeftKey("ApartmentRefId");
                    cs.MapRightKey("PictureRefId");
                    cs.ToTable("ApartmentPicture");
                });


            // Card

            modelBuilder.Entity<Card>()
                .HasMany(s => s.Dates)
                .WithRequired(s => s.Card)
                .HasForeignKey(s => s.CardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany(s => s.Genders)
                .WithRequired(s => s.Card)
                .HasForeignKey(s => s.CardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany(s => s.Reservations)
                .WithRequired(s => s.Card)
                .HasForeignKey(s => s.CardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany(s => s.Favorites)
                .WithRequired(s => s.Card)
                .HasForeignKey(s => s.CardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany(s => s.Notifications)
                .WithOptional(s => s.Card)
                .HasForeignKey(s => s.CardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany(s => s.Pictures)
                .WithMany(c => c.Cards)
                .Map(cs =>
                {
                    cs.MapLeftKey("CardRefId");
                    cs.MapRightKey("PictureRefId");
                    cs.ToTable("CardPicture");
                });


            // Reservation
            modelBuilder.Entity<Reservation>()
                .HasMany(s => s.Notifications)
                .WithOptional(s => s.Reservation)
                .HasForeignKey(s => s.ReservationId)
                .WillCascadeOnDelete(false);


            // Review
            modelBuilder.Entity<Review>()
                .HasMany(s => s.Notifications)
                .WithOptional(s => s.Review)
                .HasForeignKey(s => s.ReviewId)
                .WillCascadeOnDelete(false);

            //Favorite
            modelBuilder.Entity<Favorite>()
                .HasMany(s => s.Notifications)
                .WithOptional(s => s.Favorite)
                .HasForeignKey(s => s.FavoriteId)
                .WillCascadeOnDelete(true);

            // Picture + Profile
            modelBuilder.Entity<Picture>()
                .HasMany(s => s.Profiles)
                .WithOptional(s => s.Picture)
                .HasForeignKey(s => s.PictureId);
        }
    }
}