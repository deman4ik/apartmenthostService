using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Tables;

namespace appartmenthostService.Models
{
    public class appartmenthostContext : DbContext
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

        public appartmenthostContext() : base(connectionStringName)
        {
        }
        
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewComment> ReviewComments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SocialAccount> SocialAccounts { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string schema = ServiceSettingsDictionary.GetSchemaName();
            if (!string.IsNullOrEmpty(schema))
            {
                modelBuilder.HasDefaultSchema(schema);
            }

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

            // User + Social Account
            modelBuilder.Entity<User>()
                       .HasMany<SocialAccount>(s => s.SocialAccounts)
                       .WithRequired(s => s.User)
                       .HasForeignKey(s => s.UserId)
                       .WillCascadeOnDelete(false);

            // User + Apartment
            modelBuilder.Entity<User>()
                       .HasMany<Apartment>(s => s.Apartments)
                       .WithRequired(s => s.User)
                       .HasForeignKey(s => s.UserId)
                       .WillCascadeOnDelete(false);

            // User + Advert
            modelBuilder.Entity<User>()
                       .HasMany<Advert>(s => s.Adverts)
                       .WithRequired(s => s.User)
                       .HasForeignKey(s => s.UserId)
                       .WillCascadeOnDelete(false);

            // User + Favorite
            modelBuilder.Entity<User>()
                      .HasMany<Favorite>(s => s.Favorites)
                      .WithRequired(s => s.User)
                      .HasForeignKey(s => s.UserId)
                      .WillCascadeOnDelete(false);

            // User + Notification
            modelBuilder.Entity<User>()
                      .HasMany<Notification>(s => s.Notifications)
                      .WithRequired(s => s.User)
                      .HasForeignKey(s => s.UserId)
                      .WillCascadeOnDelete(false);

            // User + Reservation
            modelBuilder.Entity<User>()
                .HasMany<Reservation>(s => s.Reservations)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // User + Review
            modelBuilder.Entity<User>()
               .HasMany<Review>(s => s.Reviews)
               .WithRequired(s => s.User)
               .HasForeignKey(s => s.UserId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
              .HasMany<Review>(s => s.AdvertiserReviews)
              .WithRequired(s => s.Advertiser)
              .HasForeignKey(s => s.AdvertiserId)
                .WillCascadeOnDelete(false);

            // User + ReviewComment
            modelBuilder.Entity<User>()
               .HasMany<ReviewComment>(s => s.ReviewComments)
               .WithRequired(s => s.User)
               .HasForeignKey(s => s.UserId)
               .WillCascadeOnDelete(false);
            
            
            //Apartment
            modelBuilder.Entity<Apartment>()
              .HasMany<Advert>(s => s.Adverts)
              .WithOptional(s => s.Apartment)
              .HasForeignKey(s => s.ApartmentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Apartment>()
                  .HasMany<Picture>(s => s.Pictures)
                  .WithMany(c => c.Apartments)
                  .Map(cs =>
                  {
                      cs.MapLeftKey("ApartmentRefId");
                      cs.MapRightKey("PictureRefId");
                      cs.ToTable("ApartmentPicture");
                  });

            // Advert

            modelBuilder.Entity<Advert>()
 .HasMany<Reservation>(s => s.Reservations)
 .WithRequired(s => s.Advert)
 .HasForeignKey(s => s.AdvertId)
   .WillCascadeOnDelete(false);

            modelBuilder.Entity<Advert>()
              .HasMany<Favorite>(s => s.Favorites)
              .WithRequired(s => s.Advert)
              .HasForeignKey(s => s.AdvertId)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<Advert>()
                  .HasMany<Picture>(s => s.Pictures)
                  .WithMany(c => c.Adverts)
                  .Map(cs =>
                  {
                      cs.MapLeftKey("AdvertRefId");
                      cs.MapRightKey("PictureRefId");
                      cs.ToTable("AdvertPicture");
                  });

            // Review

            modelBuilder.Entity<Review>()
             .HasMany<ReviewComment>(s => s.ReviewComments)
             .WithRequired(s => s.Review)
             .HasForeignKey(s => s.ReviewId)
               .WillCascadeOnDelete(false);

            // Picture + Profile
            modelBuilder.Entity<Picture>()
               .HasMany<Profile>(s => s.Profiles)
               .WithOptional(s => s.Picture)
               .HasForeignKey(s => s.PictureId);

            // Picture + Notification
            modelBuilder.Entity<Picture>()
               .HasMany<Notification>(s => s.Notifications)
               .WithOptional(s => s.Picture)
               .HasForeignKey(s => s.PictureId);
        }
    }

}
