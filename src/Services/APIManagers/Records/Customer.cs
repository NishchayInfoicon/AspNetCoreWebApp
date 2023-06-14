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
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("State")]
        public string State { get; set; }

        [JsonPropertyName("zip")]
        public int Zip { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("birth_date")]
        public DateTime? BirthDate { get; set; }

        [JsonPropertyName("gender")]
        public char Gender { get; set; }

        [JsonPropertyName("date_entered")]
        public DateTime DateEntered { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; } 
    }
}
