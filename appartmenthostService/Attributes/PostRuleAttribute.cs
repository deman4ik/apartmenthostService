using System;

namespace apartmenthostService.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PostRuleAttribute : Attribute
    {
        public bool Visible { get; set; }
        public bool RequiredForm { get; set; }
        public bool RequiredTransfer { get; set; }
        public int Order { get; set; }
    }
}