using System.Collections.Generic;
using apartmenthostService.DataObjects;

namespace apartmenthostService.Helpers
{
    public static class ConstLang
    {
        public const string RU = "RU";
        public const string EN = "EN";
    }

    public static class ConstVals
    {
        public const string Any = "DVAL_ANY"; //Любой, не важно
        public const string Pending = "DVAL_PENDING"; //Ожидает подтверждения
        public const string Accepted = "DVAL_ACCEPTED"; //Одобрено
        public const string Declined = "DVAL_DECLINED"; //Отклонено
        public const string PMale = "PG_MALE"; //Мужской
        public const string PFemale = "PG_FEMALE"; //Женский
        public const string Male = "PC_MALE"; //Мужской
        public const string Female = "PC_FEMALE"; //Женский
        public const string Thing = "PC_THING";
        public const string Alien = "PC_ALIEN";
        public const string House = "POT_HOUSE"; //Дом
        public const string Apartment = "POT_APARTMENT"; //Квартира
        public const string Room = "POT_ROOM"; //Комната
        public const string HotelRoom = "POT_HOTEL_ROOM"; //Гостинечный номер
        public const string SeperateResidence = "DVAL_SEPARATE_RESIDENCE"; //Раздельное проживание
        public const string Cohabitation = "DVAL_COHABITATION"; //Совместное проживание
        public const string Parking = "Паркова"; //Паркова
        public const string Concierge = "Консьерж"; //Консьерж
        public const string Refrigerator = "Холодильник"; //Холодильник
        public const string WashingMachine = "Стиральная машина"; //Стиральная машина
        public const string AirConditioning = "Кондиционер"; //Кондиционер
        public const string Owner = "DVAL_OWNER"; // Арендодатель
        public const string Renter = "DVAL_RENTER"; // Съемщик
        public const string General = "DVAL_GENERAL"; // Станадартные оповещения
        public const string Global = "DVAL_GLOBAL"; // Глобальные оповещения
        public const string EmailTemp = "EMAILTEMPLATE";
        public const string Article = "ARTICLE";
        public const string Greet = "GREET";
        public const string Reg = "REG";
        public const string Restore = "RESTORE";
        public const string EmailTemplate = "EMAIL_TEMPLATE";
        public const string Email = "EMAIL";
        public const string Sms = "SMS";
        public const string Feedback = "FEEDBACK";
        public const string Abuse = "ABUSE";
        public const string PUnconf = "UNCONF";
        public const string PPending = "PENDING";
        public const string PConf = "CONF";
    }

    public static class RespH
    {
        //Success
        public const string SRV_DONE = "SRV_DONE";
        public const string SRV_CREATED = "SRV_CREATED";
        public const string SRV_UPDATED = "SRV_UPDATED";
        public const string SRV_DELETED = "SRV_DELETED";
        //Error
        public const string SRV_EXCEPTION = "SRV_EXCEPTION";
        //User
        public const string SRV_UNAUTH = "SRV_UNAUTH"; // Пользователь не авторизован
        public const string SRV_FORBIDDEN = "SRV_FORBIDDEN"; // Пользователю запрещено выполнение этого действия
        public const string SRV_USER_NULL = "SRV_USER_NULL"; // Пустой объект запроса
        public const string SRV_USER_NOTFOUND = "SRV_USER_NOTFOUND"; // Объект не найден
        public const string SRV_USER_REQUIRED = "SRV_USER_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_USER_BLOCKED = "SRV_USER_BLOCKED"; // Пользователь заблокирован
        public const string SRV_USER_NO_NAME = "SRV_USER_NO_NAME"; // Не указано Имя или Фамилия

        public const string SRV_USER_WRONG_USER = "SRV_USER_WRONG_USER";
        // Объект не может быть изменен/удален другим пользователем

        public const string SRV_USER_EXISTS = "SRV_USER_EXISTS"; // Объект уже существует

        public const string SRV_USER_DEPENDENCY = "SRV_USER_DEPENDENCY";
        // Объект не может быть изменен/удален т.к. зависит от другого объекта.  

        public const string SRV_USER_WRONG_CODE = "SRV_USER_WRONG_CODE"; // Не верный код
        public const string SRV_USER_CONFIRMED = "SRV_USER_CONFIRMED"; // Email уже подтвержден
        public const string SRV_USER_NOT_CONFIRMED = "SRV_USER_NOT_CONFIRMED"; // Email не подтвержден
        public const string SRV_USER_RESET_REQUESTED = "SRV_USER_RESET_REQUESTED"; // Сброс пароля запрошен
        public const string SRV_USER_RESET_NOT_REQUESTED = "SRV_USER_RESET_NOT_REQUESTED"; // Сброс пароля не запрошен
        public const string SRV_USER_RESETED = "SRV_USER_RESETED"; // Пароль изменен

        public const string SRV_USER_PHONE_CONFIRM_REQUESTED = "SRV_USER_PHONE_CONFIRM_REQUESTED";
        // Подтверждение телефона запрошено

        public const string SRV_USER_PHONE_CONFIRMED = "SRV_USER_PHONE_CONFIRMED"; // Телефон подтвержден
        public const string SRV_USER_PHONE_NOT_CONFIRMED = "SRV_USER_PHONE_NOT_CONFIRMED"; // Телефон не подтвержден

        public const string SRV_PROFILE_ERR_UPDATE_PHONE = "SRV_PROFILE_ERR_UPDATE_PHONE";
        public const string SRV_PROFILE_WRONG_PHONE = "SRV_PROFILE_WRONG_PHONE";
        // У пользователя есть карточка 

        //Registration
        public const string SRV_REG_INVALID_EMAIL = "SRV_REG_INVALID_EMAIL"; // Не верный email
        public const string SRV_REG_INVALID_PASSWORD = "SRV_REG_INVALID_PASSWORD"; // Не верный пароль
        public const string SRV_REG_EXISTS_EMAIL = "SRV_REG_EXISTS_EMAIL"; // Пользователь с таким email уже существует
        //Login
        public const string SRV_LOGIN_INVALID_EMAIL = "SRV_LOGIN_INVALID_EMAIL"; // Не верный логин 
        public const string SRV_LOGIN_INVALID_PASS = "SRV_LOGIN_INVALID_PASS"; // Не верный пароль
        public const string SRV_LOGIN_NO_PASS = "SRV_LOGIN_NO_PASS"; // Не установлен пароль
        //Apartment
        public const string SRV_APARTMENT_NULL = "SRV_APARTMENT_NULL"; // Пустой объект запроса
        public const string SRV_APARTMENT_NOTFOUND = "SRV_APARTMENT_NOTFOUND"; // Объект не найден
        public const string SRV_APARTMENT_REQUIRED = "SRV_APARTMENT_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_APARTMENT_EXISTS = "SRV_APARTMENT_EXISTS"; // Объект уже существует
        public const string SRV_APARTMENT_WRONG_GEO = "SRV_APARTMENT_WRONG_GEO"; //Не заданы координаты
        public const string SRV_APARTMENT_DEPENDENCY = "SRV_APARTMENT_DEPENDENCY";
        // Объект не может быть изменен/удален т.к. зависит от другого объекта.  

        public const string SRV_APARTMENT_WRONG_USER = "SRV_APARTMENT_WRONG_USER";
        // Объект не может быть изменен/удален другим пользователем

        //Card
        public const string SRV_CARD_NULL = "SRV_CARD_NULL"; // Пустой объект запроса
        public const string SRV_CARD_INVALID_FILTER = "SRV_CARD_INVALID_FILTER"; // Не верный объект запроса
        public const string SRV_CARD_NOTFOUND = "SRV_CARD_NOTFOUND"; // Объект не найден
        public const string SRV_CARD_REQUIRED = "SRV_CARD_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_CARD_EXISTS = "SRV_CARD_EXISTS"; // Объект уже существует
        public const string SRV_CARD_PHONE_UNCONF = "SRV_CARD_PHONE_UNCONF"; //Телефон не подтвержден
        public const string SRV_CARD_DEPENDENCY = "SRV_CARD_DEPENDENCY";
        // Объект не может быть изменен/удален т.к. зависит от другого объекта.  

        public const string SRV_CARD_WRONG_USER = "SRV_CARD_WRONG_USER";
        // Объект не может быть изменен/удален другим пользователем

        public const string SRV_CARD_WRONG_DATE = "SRV_CARD_WRONG_DATE"; // Дата С должна быть меньше Даты ПО
        //Reservation
        public const string SRV_RESERVATION_NULL = "SRV_RESERVATION_NULL"; //Пустой объект запроса
        public const string SRV_RESERVATION_STATUS_NULL = "SRV_RESERVATION_STATUS_NULL"; //Пустой объект запроса
        public const string SRV_RESERVATION_NOTFOUND = "SRV_RESERVATION_NOTFOUND"; // Объект не найден
        public const string SRV_RESERVATION_EXISTS = "SRV_RESERVATION_EXISTS"; // Объект уже существует
        public const string SRV_RESERVATION_SELF = "SRV_RESERVATION_SELF"; // Нельзя бронировать свое же жилье
        public const string SRV_RESERVATION_WRONG_DATE = "SRV_RESERVATION_WRONG_DATE";
        // Дата С должна быть меньше Даты ПО

        public const string SRV_RESERVATION_WRONG_STATUS = "SRV_RESERVATION_WRONG_STATUS"; // Неверный статус
        public const string SRV_RESERVATION_UNAVAILABLE_DATE = "SRV_RESERVATION_UNAVAILABLE_DATE"; // Даты недоступны

        public const string SRV_RESERVATION_GET_TYPE_NULL = "SRV_RESERVATION_GET_TYPE_NULL";
        // Не задан параметр отбора брони

        //Review
        public const string SRV_REVIEW_NULL = "SRV_REVIEW_NULL"; // Пустой объект запроса
        public const string SRV_REVIEW_NOTFOUND = "SRV_REVIEW_NOTFOUND"; // Объект не найден
        public const string SRV_REVIEW_REQUIRED = "SRV_REVIEW_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_REVIEW_EXISTS = "SRV_REVIEW_EXISTS"; // Объект уже существует

        public const string SRV_REVIEW_WRONG_RESERV_STATUS = "SRV_REVIEW_WRONG_RESERV_STATUS";
        // Не верный статус бронирования

        public const string SRV_REVIEW_WRONG_USER = "SRV_REVIEW_WRONG_USER";
        // Объект не может быть изменен/удален другим пользователем

        public const string SRV_REVIEW_WRONG_DATE = "SRV_REVIEW_WRONG_DATE"; // Не верная дата
        //Favorite
        public const string SRV_FAVORITE_CARDID_NULL = "SRV_FAVORITE_CARDID_NULL"; //Пустой объект запроса

        public const string SRV_FAVORITE_WRONG_USER = "SRV_FAVORITE_WRONG_USER";
        // Объект не может быть изменен/удален другим пользователем

        //Notification
        public const string SRV_NOTIFICATION_NOTFOUND = "SRV_NOTIFICATION_NOTFOUND"; // Объект не найден

        public const string SRV_NOTIFICATION_WRONG_USER = "SRV_NOTIFICATION_WRONG_USER";
        // Объект не может быть изменен/удален другим пользователем

        public const string SRV_NOTIF_RESERV_NEW = "SRV_NOTIF_RESERV_NEW"; // Новое бронирование
        public const string SRV_NOTIF_RESERV_PENDING = "SRV_NOTIF_RESERV_PENDING"; // Новое бронирование
        public const string SRV_NOTIF_RESERV_ACCEPTED = "SRV_NOTIF_RESERV_ACCEPTED"; // Бронирование одобрено
        public const string SRV_NOTIF_RESERV_DECLINED = "SRV_NOTIF_RESERV_DECLINED"; // Бронирование отклонено
        public const string SRV_NOTIF_REVIEW_ADDED = "SRV_NOTIF_REVIEW_ADDED"; // Оставили отзыв (без рейтинга)

        public const string SRV_NOTIF_REVIEW_RATING_ADDED = "SRV_NOTIF_REVIEW_RATING_ADDED";
        // Оставили отзыв (с рейтингом)

        public const string SRV_NOTIF_REVIEW_AVAILABLE = "SRV_NOTIF_REVIEW_AVAILABLE"; // Оставьте отзыв
        public const string SRV_NOTIF_CARD_FAVORITED = "SRV_NOTIF_CARD_FAVORITED"; // Объявление добавили в избранное
        //Picture
        public const string SRV_PICTURE_NOTFOUND = "SRV_PICTURE_NOTFOUND"; // Объект не найден
        public const string SRV_PICTURE_NULL = "SRV_PICTURE_NULL"; // Пустой объект запроса
        public const string SRV_PICTURE_REQUIRED = "SRV_PICTURE_REQUIRED"; // Не заполнено обязательно поле

        public const string SRV_PICTURE_BAD_CLOUDINARY_CRED = "SRV_PICTURE_BAD_CLOUDINARY_CRED";
        // Не правильно заданы настройки сервиса CLOUDINARY

        //Article
        public const string SRV_ARTICLE_NULL = "SRV_ARTICLE_NULL"; // Пустой объект запроса
        public const string SRV_ARTICLE_NOTFOUND = "SRV_ARTICLE_NOTFOUND"; // Объект не найден
        public const string SRV_ARTICLE_INVALID_FILTER = "SRV_ARTICLE_INVALID_FILTER"; // Не верный объект запроса
        public const string SRV_ARTICLE_REQUIRED = "SRV_ARTICLE_REQUIRED"; // Не заполнено обязательно поле

        //Feedback
        public const string SRV_FEEDBACK_NULL = "SRV_FEEDBACK_NULL"; // Пустой объект запроса
        public const string SRV_FEEDBACK_NOTFOUND = "SRV_FEEDBACK_NOTFOUND"; // Объект не найден
        public const string SRV_FEEDBACK_INVALID_FILTER = "SRV_FEEDBACK_INVALID_FILTER"; // Не верный объект запроса
        public const string SRV_FEEDBACK_REQUIRED = "SRV_FEEDBACK_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_FEEDBACK_ABUSER_NOTFOUND = "SRV_FEEDBACK_ABUSER_NOTFOUND";

        public static ResponseDTO Create(string code, List<string> data = null)
        {
            return new ResponseDTO
            {
                Code = code,
                Data = data
            };
        }

        public static ResponseBoolDataDTO CreateBool(string code, List<bool> data = null)
        {
            return new ResponseBoolDataDTO
            {
                Code = code,
                Data = data
            };
        }
    }
}