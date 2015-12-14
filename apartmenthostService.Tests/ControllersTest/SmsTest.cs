using System;
using apartmenthostService.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace apartmenthostService.Tests.ControllersTest
{
    [TestClass]
    public class SmsTest
    {
        [TestMethod]
        public void SmsSendSuccess()
        {
            using (SmsSender sender = new SmsSender("petforaweek", "1d0aa72cdd149", "petforaweek"))
            {
                sender.Send("79260353551", "1234");
               
            }
        }
    }
}
