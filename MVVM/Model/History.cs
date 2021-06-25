using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RocketLeagueGarage.MVVM.Model
{
    public class History
    {
        public string DateTime { get; set; }
        public string name { get; set; }

        public History()
        {
        }
    }
}