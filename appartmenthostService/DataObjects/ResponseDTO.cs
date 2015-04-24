using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace appartmenthostService.DataObjects
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

    public static class RespH
    {
        //Success
        public const string DONE = "DONE";
        public const string CREATED = "CREATED";
        public const string UPDATED = "UPDATED";
        public const string DELETED = "DELETED";

        //Error
        public const string EXCEPTION = "EXCEPTION";

        //User
        public const string UNAUTH = "UNAUTH"; // Пользователь не авторизован
        public const string FORBIDDEN = "FORBIDDEN"; // Пользователю запрещено выполнение этого действия

        public const string USER_NULL = "USER_NULL"; // Пустой объект запроса
        public const string USER_NOTFOUND = "USER_NOTFOUND"; // Объект не найден
        public const string USER_PROP_NOTFOUND = "USER_PROP_NOTFOUND"; // Объект не найден
        public const string USER_REQUIRED = "USER_REQUIRED"; // Не заполнено обязательно поле
        public const string USER_EXISTS = "USER_EXISTS"; // Объект уже существует
        public const string USER_DEPENDENCY = "USER_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  

        //Apartment
        public const string APARTMENT_NULL = "APARTMENT_NULL"; // Пустой объект запроса
        public const string APARTMENT_NOTFOUND = "APARTMENT_NOTFOUND"; // Объект не найден
        public const string APARTMENT_PROP_NOTFOUND = "APARTMENT_PROP_NOTFOUND"; // Объект не найден
        public const string APARTMENT_REQUIRED = "APARTMENT_REQUIRED"; // Не заполнено обязательно поле
        public const string APARTMENT_EXISTS = "APARTMENT_EXISTS"; // Объект уже существует
        public const string APARTMENT_DEPENDENCY = "APARTMENT_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  


        //Advert
        public const string ADVERT_NULL = "ADVERT_NULL"; // Пустой объект запроса
        public const string ADVERT_NOTFOUND = "ADVERT_NOTFOUND"; // Объект не найден
        public const string ADVERT_PROP_NOTFOUND = "ADVERT_PROP_NOTFOUND"; // Объект не найден
        public const string ADVERT_REQUIRED = "ADVERT_REQUIRED"; // Не заполнено обязательно поле
        public const string ADVERT_EXISTS = "ADVERT_EXISTS"; // Объект уже существует
        public const string ADVERT_DEPENDENCY = "ADVERT_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  

        //Dictionary
        public const string DICTIONARY_NULL = "DICTIONARY_NULL"; // Пустой объект запроса
        public const string DICTIONARY_NOTFOUND = "DICTIONARY_NOTFOUND"; // Объект не найден
        public const string DICTIONARY_REQUIRED = "DICTIONARY_REQUIRED"; // Не заполнено обязательно поле
        public const string DICTIONARY_EXISTS = "DICTIONARY_EXISTS"; // Объект уже существует
        public const string DICTIONARY_DEPENDENCY = "DICTIONARY_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  
        //Dictionary Item
        public const string DICTIONARYITEM_NULL = "DICTIONARYITEM_NULL"; // Пустой объект запроса
        public const string DICTIONARYITEM_NOTFOUND = "DICTIONARYITEM_NOTFOUND"; // Объект не найден
        public const string DICTIONARYITEM_REQUIRED = "DICTIONARYITEM_REQUIRED"; // Не заполнено обязательно поле
        public const string DICTIONARYITEM_EXISTS = "DICTIONARYITEM_EXISTS"; // Объект уже существует
        public const string DICTIONARYITEM_DEPENDENCY = "DICTIONARYITEM_DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.  

        public static string Create(string code, List<string> data = null)
        {
            return JsonConvert.SerializeObject(new ResponseDTO()
            {
                Code = code,
                Data = data
            });
        }
    }


}