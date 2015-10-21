using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace apartmenthostService.Models
{
    public interface IApartmenthostContext : IDisposable
    {
         DbSet<Apartment> Apartments { get; }
         DbSet<Card> Cards { get;  }
         DbSet<CardDates> Dates { get;  }
         DbSet<CardGenders> Genders { get;  }
         DbSet<Reservation> Reservations { get;  }
         DbSet<Review> Reviews { get;  }
         DbSet<User> Users { get;  }
         DbSet<Account> Accounts { get;  }
         DbSet<Profile> Profile { get;  }
         DbSet<Notification> Notifications { get; }
         DbSet<Picture> Pictures { get;  }
         DbSet<Favorite> Favorites { get;  }
         DbSet<Article> Article { get;  }

        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        void MarkAsModified(object item);
    }
}