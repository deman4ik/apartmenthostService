using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Tables;

namespace apartmenthostService.Models
{
    public class apartmenthostContext : DbContext
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

        public apartmenthostContext()
            : base(connectionStringName)
        {
            Database.SetInitializer<apartmenthostContext>(null);
        }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardDates> Dates { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Prop> Props { get; set; }
        public DbSet<PropVal> PropVals { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<DictionaryItem> DictionaryItems { get; set; }
        public DbSet<Article> Article { get; set; }

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
                .HasMany<Account>(s => s.Accounts)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // User + Apartment
            modelBuilder.Entity<User>()
                .HasMany<Apartment>(s => s.Apartments)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserId)
                .WillCascadeOnDelete(false);

            // User + Card
            modelBuilder.Entity<User>()
                .HasMany<Card>(s => s.Cards)
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
                .HasMany<Review>(s => s.OutReviews)
                .WithRequired(s => s.FromUser)
                .HasForeignKey(s => s.FromUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany<Review>(s => s.InReviews)
                .WithRequired(s => s.ToUser)
                .HasForeignKey(s => s.ToUserId)
                .WillCascadeOnDelete(false);


            //Apartment
            modelBuilder.Entity<Apartment>()
                .HasMany<Card>(s => s.Cards)
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

            modelBuilder.Entity<Apartment>()
                .HasMany<PropVal>(s => s.PropVals)
                .WithOptional(s => s.Apartment)
                .HasForeignKey(s => s.ApartmentItemId)
                .WillCascadeOnDelete(false);

            // Card

            modelBuilder.Entity<Card>()
                .HasMany<CardDates>(s => s.Dates)
                .WithRequired(s => s.Card)
                .HasForeignKey(s => s.CardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany<Reservation>(s => s.Reservations)
                .WithRequired(s => s.Card)
                .HasForeignKey(s => s.CardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany<Favorite>(s => s.Favorites)
                .WithRequired(s => s.Card)
                .HasForeignKey(s => s.CardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany<Notification>(s => s.Notifications)
                .WithOptional(s => s.Card)
                .HasForeignKey(s => s.CardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany<Picture>(s => s.Pictures)
                .WithMany(c => c.Cards)
                .Map(cs =>
                {
                    cs.MapLeftKey("CardRefId");
                    cs.MapRightKey("PictureRefId");
                    cs.ToTable("CardPicture");
                });

            modelBuilder.Entity<Card>()
                .HasMany<PropVal>(s => s.PropVals)
                .WithOptional(s => s.Card)
                .HasForeignKey(s => s.AdvertItemId)
                .WillCascadeOnDelete(false);

            // Reservation
            modelBuilder.Entity<Reservation>()
                .HasMany<Notification>(s => s.Notifications)
                .WithOptional(s => s.Reservation)
                .HasForeignKey(s => s.ReservationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reservation>()
                .HasMany<PropVal>(s => s.PropVals)
                .WithOptional(s => s.Reservation)
                .HasForeignKey(s => s.ReservationItemId)
                .WillCascadeOnDelete(false);

            // Review
            modelBuilder.Entity<Review>()
                .HasMany<Notification>(s => s.Notifications)
                .WithOptional(s => s.Review)
                .HasForeignKey(s => s.ReviewId)
                .WillCascadeOnDelete(false);

            //Favorite
            modelBuilder.Entity<Favorite>()
                .HasMany<Notification>(s => s.Notifications)
                .WithOptional(s => s.Favorite)
                .HasForeignKey(s => s.FavoriteId)
                .WillCascadeOnDelete(true);

            // Picture + Article
            modelBuilder.Entity<Picture>()
                .HasMany<Article>(s => s.Articles)
                .WithOptional(s => s.Picture)
                .HasForeignKey(s => s.PictureId);

            // Picture + Profile
            modelBuilder.Entity<Picture>()
                .HasMany<Profile>(s => s.Profiles)
                .WithOptional(s => s.Picture)
                .HasForeignKey(s => s.PictureId);

            // Table + Prop
            modelBuilder.Entity<Table>()
                .HasMany<Prop>(s => s.Props)
                .WithMany(c => c.Tables)
                .Map(cs =>
                {
                    cs.MapLeftKey("TableRefId");
                    cs.MapRightKey("PropRefId");
                    cs.ToTable("TableProp");
                });

            // Prop + PropVal
            modelBuilder.Entity<Prop>()
                .HasMany<PropVal>(s => s.PropVals)
                .WithRequired(s => s.Prop)
                .HasForeignKey(s => s.PropId);


            // Dictionary + Dictionary Items
            modelBuilder.Entity<Dictionary>()
                .HasMany<DictionaryItem>(s => s.DictionaryItems)
                .WithRequired(s => s.Dictionary)
                .HasForeignKey(s => s.DictionaryId);

            // Dictionary + Prop
            modelBuilder.Entity<Dictionary>()
                .HasMany<Prop>(s => s.Props)
                .WithOptional(s => s.Dictionary)
                .HasForeignKey(s => s.DictionaryId);

            // DictionaryItem + PropVal
            modelBuilder.Entity<DictionaryItem>()
                .HasMany<PropVal>(s => s.PropVals)
                .WithOptional(s => s.DictionaryItem)
                .HasForeignKey(s => s.DictionaryItemId);
        }
    }
}