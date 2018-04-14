using System.Collections.Generic;
using Vue.Domain;
using Vue.Domain.Cards;
using Vue.Models;

namespace Vue.Utility
{
    public static class Extensions
    {
        public static BasicUser BasicUser(this User user)
        {
            return new BasicUser { Username = user.Username };
        }
    }
}