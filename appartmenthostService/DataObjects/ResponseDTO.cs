using System.Collections.Generic;

namespace apartmenthostService.DataObjects
{
    /************************************************************************
     *          Стандартный ответ сервиса                                   *
     *  Отправляется вместе со стандартным HttpStatusCod'ом.                *
     *                                                                      *
     *  HttpStatus 200 "OK" Операция выполнена успешно                      *
     *  HttpStatus 400 "BadRequest" Операция не выполнена, не верный запрос *
     ************************************************************************/
    public class ResponseDTO 
    {
        /* Тип ответа
         * Принимает одно из значений класса ResponseTypes */
        public string Code { get; set; } 

        /* Список значений */
        public List<string> Data { get; set; }

    }

    public class ResponseBoolDataDTO
    {
        /* Тип ответа
         * Принимает одно из значений класса ResponseTypes */
        public string Code { get; set; }

        /* Список значений */
        public List<bool> Data { get; set; }

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
        public const string SRV_USER_INVALID_DICITEM = "SRV_USER_INVALID_DICITEM"; // Не верное значение словаря
        public const string SRV_USER_PROP_NOTFOUND = "SRV_USER_PROP_NOTFOUND"; // Объект не найден
        public const string SRV_USER_REQUIRED = "SRV_USER_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_USER_WRONG_USER = "SRV_USER_WRONG_USER"; // Объект не может быть изменен/удален другим пользователем
        public const string SRV_USER_EXISTS = "SRV_USER_EXISTS"; // Объект уже существует
        public const string SRV_USER_DEPENDENCY = "SRV_USER_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  

        //Registration
        public const string SRV_REG_INVALID_EMAIL = "SRV_REG_INVALID_EMAIL"; // Не верный email
        public const string SRV_REG_INVALID_PASSWORD = "SRV_REG_INVALID_PASSWORD"; // Не верный пароль
        public const string SRV_REG_EXISTS_EMAIL = "SRV_REG_EXISTS_EMAIL"; // Пользователь с таким email уже существует

        //Login
        public const string SRV_LOGIN_INVALID_EMAIL = "SRV_LOGIN_INVALID_EMAIL"; // Не верный логин 
        public const string SRV_LOGIN_INVALID_PASS = "SRV_LOGIN_INVALID_PASS"; // Не верный пароль

        //Apartment
        public const string SRV_APARTMENT_NULL = "SRV_APARTMENT_NULL"; // Пустой объект запроса
        public const string SRV_APARTMENT_NOTFOUND = "SRV_APARTMENT_NOTFOUND"; // Объект не найден
        public const string SRV_APARTMENT_PROP_NOTFOUND = "SRV_APARTMENT_PROP_NOTFOUND"; // Свойство объекта не найденл
        public const string SRV_APARTMENT_PROPVAL_NOTFOUND = "SRV_APARTMENT_PROPVAL_NOTFOUND"; // Значение свойства объекта не найдено
        public const string SRV_APARTMENT_REQUIRED = "SRV_APARTMENT_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_APARTMENT_INVALID_DICITEM = "SRV_APARTMENT_INVALID_DICITEM"; // Не верное значение словаря
        public const string SRV_APARTMENT_EXISTS = "SRV_APARTMENT_EXISTS"; // Объект уже существует
        public const string SRV_APARTMENT_DEPENDENCY = "SRV_APARTMENT_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  
        public const string SRV_APARTMENT_WRONG_USER = "SRV_APARTMENT_WRONG_USER"; // Объект не может быть изменен/удален другим пользователем

        //Card
        public const string SRV_CARD_NULL = "SRV_CARD_NULL"; // Пустой объект запроса
        public const string SRV_CARD_NOTFOUND = "SRV_CARD_NOTFOUND"; // Объект не найден
        public const string SRV_CARD_PROP_NOTFOUND = "SRV_CARD_PROP_NOTFOUND"; // Объект не найден
        public const string SRV_CARD_PROPVAL_NOTFOUND = "SRV_CARD_PROPVAL_NOTFOUND"; // Значение свойства объекта не найдено
        public const string SRV_CARD_REQUIRED = "SRV_CARD_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_CARD_INVALID_DICITEM = "SRV_CARD_INVALID_DICITEM"; // Не верное значение словаря
        public const string SRV_CARD_EXISTS = "SRV_CARD_EXISTS"; // Объект уже существует
        public const string SRV_CARD_DEPENDENCY = "SRV_CARD_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  
        public const string SRV_CARD_WRONG_USER = "SRV_CARD_WRONG_USER"; // Объект не может быть изменен/удален другим пользователем
        public const string SRV_CARD_WRONG_DATE = "SRV_CARD_WRONG_DATE"; // Дата С должна быть меньше Даты ПО

        //Reservation
        public const string SRV_RESERVATION_NULL = "SRV_RESERVATION_NULL"; //Пустой объект запроса
        public const string SRV_RESERVATION_STATUS_NULL = "SRV_RESERVATION_STATUS_NULL"; //Пустой объект запроса
        public const string SRV_RESERVATION_NOTFOUND = "SRV_RESERVATION_NOTFOUND"; // Объект не найден
        public const string SRV_RESERVATION_WRONG_DATE = "SRV_RESERVATION_WRONG_DATE"; // Дата С должна быть меньше Даты ПО
        public const string SRV_RESERVATION_WRONG_STATUS = "SRV_RESERVATION_WRONG_STATUS"; // Неверный статус
        public const string SRV_RESERVATION_UNAVAILABLE_DATE = "SRV_RESERVATION_UNAVAILABLE_DATE"; // Даты недоступны

        //Review
        public const string SRV_REVIEW_NULL = "SRV_REVIEW_NULL"; // Пустой объект запроса
        public const string SRV_REVIEW_NOTFOUND = "SRV_REVIEW_NOTFOUND"; // Объект не найден
        public const string SRV_REVIEW_REQUIRED = "SRV_REVIEW_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_REVIEW_EXISTS = "SRV_REVIEW_EXISTS"; // Объект уже существует
        public const string SRV_REVIEW_WRONG_RESERV_STATUS = "SRV_REVIEW_WRONG_RESERV_STATUS"; // Не верный статус бронирования
        public const string SRV_REVIEW_WRONG_USER = "SRV_REVIEW_WRONG_USER"; // Объект не может быть изменен/удален другим пользователем
        public const string SRV_REVIEW_WRONG_DATE = "SRV_REVIEW_WRONG_DATE"; // Не верная дата
        

        //Favorite
        public const string SRV_FAVORITE_CARDID_NULL = "SRV_FAVORITE_CARDID_NULL"; //Пустой объект запроса
        public const string SRV_FAVORITE_WRONG_USER = "SRV_FAVORITE_WRONG_USER"; // Объект не может быть изменен/удален другим пользователем


        //Notification
        public const string SRV_NOTIFICATION_NOTFOUND = "SRV_NOTIFICATION_NOTFOUND"; // Объект не найден
        public const string SRV_NOTIFICATION_WRONG_USER = "SRV_NOTIFICATION_WRONG_USER"; // Объект не может быть изменен/удален другим пользователем

        public const string SRV_NOTIF_RESERV_PENDING = "SRV_NOTIF_RESERV_PENDING"; // Новое бронирование
        public const string SRV_NOTIF_RESERV_ACCEPTED = "SRV_NOTIF_RESERV_ACCEPTED"; // Бронирование одобрено
        public const string SRV_NOTIF_RESERV_DECLINED = "SRV_NOTIF_RESERV_DECLINED"; // Бронирование отклонено
        public const string SRV_NOTIF_REVIEW_ADDED = "SRV_NOTIF_REVIEW_ADDED"; // Оставили отзыв (без рейтинга)
        public const string SRV_NOTIF_REVIEW_RATING_ADDED = "SRV_NOTIF_REVIEW_RATING_ADDED"; // Оставили отзыв (с рейтингом)
        public const string SRV_NOTIF_REVIEW_AVAILABLE = "SRV_NOTIF_REVIEW_AVAILABLE"; // Оставьте отзыв
        public const string SRV_NOTIF_CARD_FAVORITED = "SRV_NOTIF_CARD_FAVORITED"; // Объявление добавили в избранное
        //Dictionary
        public const string SRV_DICTIONARY_NULL = "SRV_DICTIONARY_NULL"; // Пустой объект запроса
        public const string SRV_DICTIONARY_NOTFOUND = "SRV_DICTIONARY_NOTFOUND"; // Объект не найден
        public const string SRV_DICTIONARY_REQUIRED = "SRV_DICTIONARY_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_DICTIONARY_EXISTS = "SRV_DICTIONARY_EXISTS"; // Объект уже существует
        public const string SRV_DICTIONARY_DEPENDENCY = "SRV_DICTIONARY_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  

        //Dictionary Item
        public const string SRV_DICTIONARYITEM_NULL = "SRV_DICTIONARYITEM_NULL"; // Пустой объект запроса
        public const string SRV_DICTIONARYITEM_NOTFOUND = "SRV_DICTIONARYITEM_NOTFOUND"; // Объект не найден
        public const string SRV_DICTIONARYITEM_REQUIRED = "SRV_DICTIONARYITEM_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_DICTIONARYITEM_EXISTS = "SRV_DICTIONARYITEM_EXISTS"; // Объект уже существует
        public const string SRV_DICTIONARYITEM_DEPENDENCY = "SRV_DICTIONARYITEM_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  

        public static ResponseDTO Create(string code, List<string> data = null)
        {
            
            return new ResponseDTO()
            {
                Code = code,
                Data = data
            };
        }

        public static ResponseBoolDataDTO CreateBool(string code, List<bool> data = null)
        {

            return new ResponseBoolDataDTO()
            {
                Code = code,
                Data = data
            };
        }
    }


}