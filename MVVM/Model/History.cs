using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RocketLeagueGarage.MVVM.Model
{
    [Serializable]
    public class History
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        public String DateTime { get; set; }

        public History()
        {
        }

        public History(string n)
        {
            name = n;
        }
    }
}