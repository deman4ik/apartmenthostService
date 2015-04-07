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


            // User + Profile
            // Configure StudentId as PK for StudentAddress
            modelBuilder.Entity<Profile>()
                .HasKey(e => e.UserId);

            // Configure StudentId as FK for StudentAddress
            modelBuilder.Entity<User>()
                        .HasOptional(s => s.Profile) // Mark StudentAddress is optional for Student
                        .WithRequired(ad => ad.User); // Create inverse relationship
        }
    }

}
