using RocketLeagueGarage.Core;

namespace RocketLeagueGarage.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand AccountViewCommand { get; set; }
        public RelayCommand HistoryLogsViewCommand { get; set; }

        public HomeViewModel HomeViewModel { get; set; }
        public AccountViewModel AccountViewModel { get; set; }
        public HistoryLogsViewModel HistoryLogsViewModel { get; set; }

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
            HistoryLogsViewModel = new HistoryLogsViewModel();

            CurrentView = HomeViewModel;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeViewModel;
            });

            AccountViewCommand = new RelayCommand(o =>
            {
                CurrentView = AccountViewModel;
            });

            HistoryLogsViewCommand = new RelayCommand(o =>
            {
                CurrentView = HistoryLogsViewModel;
            });
        }
    }
}