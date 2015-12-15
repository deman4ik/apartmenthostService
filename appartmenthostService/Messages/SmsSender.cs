using System;
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

        public SmsSender(string prjname, string apikey, string sender)
        {
            _prjname = prjname;
            _apikey = apikey;
            _sender = sender;
        }

        public void Send(string recipient, string message)
        {
            Sms sms = new Sms(_prjname, _apikey);
            sms.send(_sender, recipient, message);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}