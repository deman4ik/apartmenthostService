using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apartmenthostService.DataObjects
{
    public class BaseUserDTO
    {
        // Уникальный идентификатор(User)
        public string Id { get; set; }

        // Email (Логин)
        public string Email { get; set; }

        // Имя
        public string FirstName { get; set; }

        // Фамилия
        public string LastName { get; set; }

        // Рейтинг
        public decimal Rating { get; set; }

        // Пол
        public string Gender { get; set; }
    }

}
