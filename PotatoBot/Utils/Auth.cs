using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PotatoBot.Utils
{
    public static class Auth
    {
        static HttpClient client = new HttpClient();
        public static async Task<bool> Check(HttpRequest req)
        {
            if (!req.Headers.ContainsKey("Authorization")) return false;
            var code = req.Headers["Authorization"];
            return false;
        }
    }
}
