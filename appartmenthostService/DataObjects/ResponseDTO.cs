using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

    public static class ResponseTypes
    {
        public const string NULL = "NULL"; // Пустой объект запроса
        public const string REQUIRED = "REQUIRED"; // Не заполнено обязательно поле
        public const string EXISTS = "EXISTS"; // Объект уже существует
        public const string UNAUTH = "UNAUTH"; // Пользователь не авторизован
        public const string NOTFOUND = "NOTFOUND"; // Объект не найден
        public const string FORBIDDEN = "FORBIDDEN"; // Пользователю запрещено выполнение этого действия
        public const string DEPENDENCY = "DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.
    }
}