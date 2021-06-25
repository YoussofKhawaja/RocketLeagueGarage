using RocketLeagueGarage.Core;
using RocketLeagueGarage.FilesManager;
using RocketLeagueGarage.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLeagueGarage.MVVM.ViewModel
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
            Write();
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

        public void Write()
        {
            if (History != null && History.Count != 0)
                if (RocketData.WhatDoing == History[History.Count - 1].name)
                    return;

            AddItems(new List<History>() { new History() { name = RocketData.WhatDoing, DateTime = DateTime.UtcNow.ToString() } });

            Save.WriteToXmlFile<List<History>>(History.ToList(), "Data", "history");
        }

        public void Read()
        {
            History = new System.Collections.ObjectModel.ObservableCollection<History>(Save.ReadFromXmlFile<List<History>>("Data", "history"));
        }
    }
}