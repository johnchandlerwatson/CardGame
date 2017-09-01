using System;

namespace Vue.Domain
{
    public class Demographics 
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }

    public enum Gender 
    {
        Male, 
        Female
    }
}