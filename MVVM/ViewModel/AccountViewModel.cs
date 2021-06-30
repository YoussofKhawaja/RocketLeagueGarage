using RocketLeagueGarage.Core;
using RocketLeagueGarage.FilesManager;
using RocketLeagueGarage.MVVM.Model;
using System.Windows.Input;

namespace RocketLeagueGarage.MVVM.ViewModel
{
    public class AccountViewModel : ObservableObject
    {
        public static AccountDataModel AppData = new AccountDataModel();

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = AppData.Name = value;
                OnPropertyChanged();
                SaveData();
            }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set
            {
                email = AppData.Email = value;
                OnPropertyChanged();
                SaveData();
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = AppData.Password = value;
                OnPropertyChanged();
                SaveData();
            }
        }

        public AccountViewModel()
        {
            LoadData();
            Name = AppData.Name;
            Email = AppData.Email;
            Password = AppData.Password;
        }

        public static void SaveData()
        {
            Save.WriteToXmlFile<AccountDataModel>(AppData, "Data", "Account");
        }

        public static void LoadData()
        {
            AppData = Save.ReadFromXmlFile<AccountDataModel>("Data", "Account");
        }
    }
}