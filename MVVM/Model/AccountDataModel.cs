using System;

namespace RocketLeagueGarage.MVVM.Model
{
    [Serializable]
    public class AccountDataModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}