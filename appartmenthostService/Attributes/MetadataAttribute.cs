using System;

namespace apartmenthostService.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MetadataAttribute : Attribute
    {
        public string DataType { get; set; }
        public string Dictionary { get; set; }
        public bool Multi { get; set; }
    }
}