using RocketLeagueGarage.FilesManager;
using RocketLeagueGarage.MVVM.Model;
using RocketLeagueGarage.MVVM.ViewModel;
using System.Windows.Controls;

namespace RocketLeagueGarage.MVVM.View
{
    /// <summary>
    /// Interaction logic for HistoryLogs.xaml
    /// </summary>
    public partial class HistoryLogsView : UserControl
    {
        private HistoryLogsViewModel viewModel;

        public HistoryLogsView()
        {
            InitializeComponent();
            this.DataContext = viewModel = new HistoryLogsViewModel();
            Data();
        }

        private void Data()
        {
            viewModel.AddItem(new History() { name = "Name:" + " " + RocketData.WhatDoing });
            Save.WriteToXmlFile(viewModel, "Data", "HistoryLogs");
        }
    }
}