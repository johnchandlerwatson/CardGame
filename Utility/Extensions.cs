using System.Collections.Generic;
using Newtonsoft.Json;
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

        public static string ToJson<T>(this T model)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
            return JsonConvert.SerializeObject(model, settings);
        }
    }
}