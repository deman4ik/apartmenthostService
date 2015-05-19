using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using apartmenthostService.Helpers;
using Newtonsoft.Json;

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

        //Advert
        public const string SRV_ADVERT_NULL = "SRV_ADVERT_NULL"; // Пустой объект запроса
        public const string SRV_ADVERT_NOTFOUND = "SRV_ADVERT_NOTFOUND"; // Объект не найден
        public const string SRV_ADVERT_PROP_NOTFOUND = "SRV_ADVERT_PROP_NOTFOUND"; // Объект не найден
        public const string SRV_ADVERT_PROPVAL_NOTFOUND = "SRV_ADVERT_PROPVAL_NOTFOUND"; // Значение свойства объекта не найдено
        public const string SRV_ADVERT_REQUIRED = "SRV_ADVERT_REQUIRED"; // Не заполнено обязательно поле
        public const string SRV_ADVERT_INVALID_DICITEM = "SRV_ADVERT_INVALID_DICITEM"; // Не верное значение словаря
        public const string SRV_ADVERT_EXISTS = "SRV_ADVERT_EXISTS"; // Объект уже существует
        public const string SRV_ADVERT_DEPENDENCY = "SRV_ADVERT_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  
        public const string SRV_ADVERT_WRONG_USER = "SRV_ADVERT_WRONG_USER"; // Объект не может быть изменен/удален другим пользователем
        public const string SRV_ADVERT_WRONG_DATE = "SRV_ADVERT_WRONG_DATE"; // Дата С должна быть меньше Даты ПО

        //Reservation
        public const string SRV_RESERVATION_NULL = "SRV_RESERVATION_NULL"; //Пустой объект запроса
        public const string SRV_RESERVATION_STATUS_NULL = "SRV_RESERVATION_STATUS_NULL"; //Пустой объект запроса
        public const string SRV_RESERVATION_NOTFOUND = "SRV_RESERVATION_NOTFOUND"; // Объект не найден
        public const string SRV_RESERVATION_WRONG_DATE = "SRV_RESERVATION_WRONG_DATE"; // Дата С должна быть меньше Даты ПО
        public const string SRV_RESERVATION_WRONG_STATUS = "SRV_RESERVATION_WRONG_STATUS"; // Неверный статус
        public const string SRV_RESERVATION_UNAVAILABLE_DATE = "SRV_RESERVATION_UNAVAILABLE_DATE"; // Даты недоступны

        //Favorite
        public const string SRV_FAVORITE_ADVERTID_NULL = "SRV_FAVORITE_ADVERTID_NULL"; //Пустой объект запроса
        public const string SRV_FAVORITE_WRONG_USER = "SRV_FAVORITE_WRONG_USER"; // Объект не может быть изменен/удален другим пользователем
        
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