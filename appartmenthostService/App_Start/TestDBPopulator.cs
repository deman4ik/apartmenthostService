using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using appartmenthostService.Authentication;
using appartmenthostService.Helpers;
using appartmenthostService.Models;

namespace appartmenthostService.App_Start
{
    public class TestDBPopulator
    {
        public static void Populate(appartmenthostContext context)
        {
            PopulateUsers(context);
            context.SaveChanges();
        }

        public static void PopulateUsers(appartmenthostContext context)
        {
            byte[] salt = StandartLoginProviderUtils.generateSalt();
            string id1 = Guid.NewGuid().ToString();
            string id2 = Guid.NewGuid().ToString();
            List<User> users = new List<User>
            {

                new User { Id = id1,
                           Email = "vasek@example.com", 
                           Salt = salt, 
                           SaltedAndHashedPassword = StandartLoginProviderUtils.hash("parusina", salt),
                           Profile = new Profile { Id = id1, 
                                                   FirstName = "Василий", 
                                                   LastName = "Пупович", 
                                                   Birthday = new DateTime(1976,3,23),
                                                   ContactEmail = "vasek@example.com",
                                                   ContactKind = "Email",
                                                   Description = "Пуповичи 100 лет на рынке недвижимости!",
                                                   Gender = "Male",
                                                   Phone = "+79998887766"               
                                                } 
                        },

                new User { Id = id2, 
                           Email = "parus@parus.ru", 
                           Salt = salt, 
                           SaltedAndHashedPassword = StandartLoginProviderUtils.hash("parusina", salt),
                           Profile = new Profile { Id = id2,
                                                   FirstName = "ЦИТК",
                                                   LastName = "Парус",
                                                   Birthday = new DateTime(1989,1,1),
                                                   ContactEmail = "parus@parus.ru",
                                                   ContactKind = "Phone",
                                                   Description = "Информационные Системы Управления",
                                                   Gender = "Male",
                                                   Phone = "+74957777777"               
                                                 }  
                        },
          };

           
            foreach (User user in users)
            {
                context.Set<User>().Add(user);
            }

            

           
        }
    }
}
