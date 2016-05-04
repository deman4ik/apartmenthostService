using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apartmenthostService.DataObjects
{
    public class MailLists
    {
        public List<MailList> NewsletterList { get; set; }
        public List<MailList> NorificationsList { get; set; } 
        public List<MailList> AllUsersList { get; set; } 
    }

    public class MailList
    {
        public MailList()
        {
            Email = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
        }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}