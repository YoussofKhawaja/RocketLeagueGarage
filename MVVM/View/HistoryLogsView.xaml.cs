﻿using RocketLeagueGarage.FilesManager;
using RocketLeagueGarage.MVVM.Model;
using RocketLeagueGarage.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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