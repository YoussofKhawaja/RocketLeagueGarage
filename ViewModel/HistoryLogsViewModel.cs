using RocketLeagueGarage.Core;
using RocketLeagueGarage.FilesManager;
using RocketLeagueGarage.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLeagueGarage.ViewModel
{
    public class HistoryLogsViewModel : ObservableObject
    {
        private ObservableCollection<History> history;

        public ObservableCollection<History> History
        {
            get
            {
                return history;
            }
            set
            {
                history = value;
                OnPropertyChanged();
            }
        }

        public HistoryLogsViewModel()
        {
            Read();
        }

        //AddItem
        public Task AddItem(History history)
        {
            History.Add(history);

            return Task.CompletedTask;
        }

        //AddItems
        public Task AddItems(List<History> history)
        {
            foreach (var item in history)
            {
                History.Add(item);
            }

            return Task.CompletedTask;
        }

        //RemoveItem
        public Task RemoveItem(History history)
        {
            History.Remove(history);

            return Task.CompletedTask;
        }

        //RemoveItem
        public Task RemoveItem(int index)
        {
            History.RemoveAt(index);

            return Task.CompletedTask;
        }

        //GetHistoryLogs
        public Task<List<History>> GetHistoryLogs()
        {
            return Task.FromResult(History.ToList());
        }

        //GetHistoryLog
        public Task<History> GetHistoryLog(int index)
        {
            return Task.FromResult(History[index]);
        }

        public void Read()
        {
            History = new System.Collections.ObjectModel.ObservableCollection<History>(Save.ReadFromXmlFile<List<History>>("Data", "history"));
        }
    }
}