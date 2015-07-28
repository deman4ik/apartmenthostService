using Microsoft.WindowsAzure.Mobile.Service;

namespace apartmenthostService.Models
{
    /* 
     * Аккаунты пользователя 
     * (может быть несколько связанных непосредственно
     * с одним пользователем, например для различных соц. сетей)
     */

    public class Account : EntityData
    {
        // Уникальный идентификатор аккаунта 
        public string AccountId { get; set; }
        // Уникальный идентификатор пользователя
        public string UserId { get; set; }
        // Наименование провайдера для входа
        public string Provider { get; set; }
        // Уникальный идентификатор провайдера
        public string ProviderId { get; set; }
        // Ссылка на пользователя
        public User User { get; set; }
    }
}