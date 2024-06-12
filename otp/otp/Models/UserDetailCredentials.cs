using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace otp.Models
{
   public class UserDetailCredentials
    {

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
