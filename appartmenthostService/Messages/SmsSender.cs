using System;
using System.Diagnostics;
using SmsApi;

namespace apartmenthostService.Messages
{
    public class SmsSender : IDisposable
    {
        private readonly string _prjname;
        private readonly string _apikey;
        private readonly string _sender;

        public SmsSender()
        {
            _prjname = Environment.GetEnvironmentVariable("MAINSMS_PROJECTNAME");
            _apikey = Environment.GetEnvironmentVariable("MAINSMS_APIKEY");
            _sender = Environment.GetEnvironmentVariable("MAINSMS_SENDER");
        }


        public string Send(string recipient, string message)
        {
            try
            {
                Sms sms = new Sms(_prjname, _apikey);
                ResponseSend rsend = sms.send(_sender, recipient, message);
                return rsend.response;
                ;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return e.Message;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}