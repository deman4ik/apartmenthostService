﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apartmenthostService.DataObjects
{
    public class FeedbackDTO
    {
        
        /// <summary>
        /// Имя пользователя (указывается только для гостя)
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Электронная почта пользователя (указывается только для гостя)
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Признак необходимости ответа по электронной почте
        /// </summary>
        public bool AnswerByEmail { get; set; }

        public string UserId { get; set; }
    }
}