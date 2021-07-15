using RocketLeagueGarage.FilesManager;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;

namespace RocketLeagueGarage.View
{
    /// <summary>
    /// Interaction logic for HistoryLogs.xaml
    /// </summary>
    public partial class HistoryLogsView : UserControl
    {
        public HistoryLogsView()
        {
            InitializeComponent();
            Group();
        }

        //group each list to its date
        private void Group()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listviewgroup.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("test");
            view.GroupDescriptions.Add(groupDescription);
        }

        //clear all saved history logs
        private void clear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Debug.WriteLine("here");
            Save.DeleteFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/RocketGarage/Data/history.xml");
        }
    }
}