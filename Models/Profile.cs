using System;
using System.Collections.Generic;
using Vue.Domain;

namespace Vue.Models
{
    public class Profile
    {
        public Demographics Demographics { get; set; }
        public List<Experience> Experience { get; set; }
    }
}