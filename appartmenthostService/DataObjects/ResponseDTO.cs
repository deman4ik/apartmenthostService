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
        public string Type { get; set; } 

        /* Тип объекта вызвашего ошибку */
        public string Entity { get; set; }

        /* Имя поля вызвавшего ошибку */
        public string Prop { get; set; }

        /* Значение поля вызвавшего ошибку */
        public string Val { get; set; }

        /* Сообщение
         * Текст ошибки для пояснений */
        public string Msg { get; set; }
    }

    public static class ResponseTypes
    {
        public const string NULL = "NULL"; // Пустой объект запроса
        public const string REQUIRED = "REQUIRED"; // Не заполнено обязательно поле, при этом имя поля указывается в Prop, а его текущее значение в Val
        public const string EXISTS = "EXISTS"; // Объект уже существует, нарушение уникальности по полю в Prop
        public const string UNAUTH = "UNAUTH"; // Пользователь не авторизован
        public const string NOTFOUND = "NOTFOUND"; // Объект не найден, Тип объекта в Entity, поиск осуществлялся по полю в Prop со значением Val
        public const string FORBIDDEN = "FORBIDDEN"; // Пользователю запрещено выполнение этого действия
        public const string DEPENDENCY = "DEPENDENCY"; // Объект не может быть изменен/удален т.к. зависит от другого объекта.
    }
}