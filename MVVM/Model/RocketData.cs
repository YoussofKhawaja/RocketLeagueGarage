using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketLeagueGarage.MVVM.Model
{
    [Serializable]
    public class RocketData
    {
        public string WhatDoing { get; set; }
        public string OnOff { get; set; }
        public string TimeLabel { get; set; }
    }
}