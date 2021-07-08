using RocketLeagueGarage.FilesManager;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;

namespace RocketLeagueGarage.MVVM.View
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

        private void Group()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listviewgroup.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("test");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void clear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Debug.WriteLine("here");
            Save.DeleteFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/RocketGarage/Data/history.xml");
        }
    }
}