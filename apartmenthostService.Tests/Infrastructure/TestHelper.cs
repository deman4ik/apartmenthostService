using System;
using System.Net.Http;
using apartmenthostService.DataObjects;
using Newtonsoft.Json;

namespace apartmenthostService.Tests.Infrastructure
{
    /// <summary>
    /// Статичный класс для обработки результата выполнения API контроллера
    /// и вывода ответа в консоль
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Метод разбирает ответ API контроллера
        /// </summary>
        /// <param name="response">Стандартный ответ API контроллера</param>
        /// <returns></returns>
        public static ControllerResult ParseResponse(HttpResponseMessage response)
        {
            if (response == null)
            {
                Console.WriteLine("HttpResponseMessage is NULL");
                return null;
            }
            var result = JsonConvert.DeserializeObject<ResponseDTO>(response.Content.ReadAsStringAsync().Result);
                var controllerResult = new ControllerResult
                {
                    StatusCode = response.StatusCode.ToString(),
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    ResponseCode = result.Code,
                    ResponseData = result.Data
                };
                Log(controllerResult);
                return controllerResult;
            
        }

        /// <summary>
        /// Метод выводит ответ API контроллера в консоль
        /// </summary>
        /// <param name="result"></param>
        public static void Log(ControllerResult result)
        {
            Console.WriteLine("# Status Code:");
            Console.WriteLine(result.StatusCode);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("# Is Success Status Code:");
            Console.WriteLine(result.IsSuccessStatusCode);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("# Result Code:");
            Console.WriteLine(result.ResponseCode);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("# Result Data:");
            if (result.ResponseData != null)
            {
                foreach (var data in result.ResponseData)
                {
                    Console.WriteLine(data);
                }
            }
            
        } 
    }
}
