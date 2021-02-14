using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraninigProject
{
    public static class SessionExtension
    {

        public static void WriteToSession(this HttpContext httpContext, string key, object value)
        {

            string data = JsonConvert.SerializeObject(value);
            httpContext.Session.SetString(key, data);

        }

        public static T ReadFromSession<T>(this HttpContext httpContext, string key)
        {
            T result = JsonConvert.DeserializeObject<T>("");
            if (httpContext.Session.GetString(key) != null)
            {
                 result = JsonConvert.DeserializeObject<T>(httpContext.Session.GetString(key));
                
            }            
                return result;
        }

        public static void ClearSession(this HttpContext httpContext)
        {
            httpContext.Session.Clear();
        }


        public static void DeletFromSession(this HttpContext httpContext, string key)
        {
            httpContext.Session.Remove(key);
        }



    }
}
