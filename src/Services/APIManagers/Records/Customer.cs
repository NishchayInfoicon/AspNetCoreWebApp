using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Practice.Services.APIManagers.Records
{
    public class Customer
    {
        [JsonPropertyName("first_name")]
        public string first_name { get; set; }

        [JsonPropertyName("last_name")]
        public string last_name { get; set; }

        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("company")]
        public string company { get; set; }

        [JsonPropertyName("street")]
        public string street { get; set; }

        [JsonPropertyName("city")]
        public string city { get; set; }

        [JsonPropertyName("state")]
        public string state { get; set; }

        [JsonPropertyName("zip")]
        public int zip { get; set; }

        [JsonPropertyName("phone")]
        public string phone { get; set; }

        [JsonPropertyName("birth_date")]
        public DateTime birth_date { get; set; }

        [JsonPropertyName("gender")]
        public char gender { get; set; }

        [JsonPropertyName("date_entered")]
        public DateTime date_entered { get; set; }

        [JsonPropertyName("id")]
        public int id { get; set; } 
    }
}
