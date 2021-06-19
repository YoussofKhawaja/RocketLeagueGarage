using RocketLeagueGarage.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketLeagueGarage.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand AccountViewCommand { get; set; }

        public HomeViewModel HomeViewModel { get; set; }
        public AccountViewModel AccountViewModel { get; set; }

        private object _CurrentView;

        public object CurrentView
        {
            get { return _CurrentView; }
            set
            {
                _CurrentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeViewModel = new HomeViewModel();
            AccountViewModel = new AccountViewModel();
            CurrentView = HomeViewModel;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeViewModel;
            });

            AccountViewCommand = new RelayCommand(o =>
            {
                CurrentView = AccountViewModel;
            });
        }
    }
}