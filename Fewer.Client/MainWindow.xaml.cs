using Fewer.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Fewer.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int NominalComboBoxIndex = 2;
        private List<string> _disks;
        List<File> _files;
        private Thread _thread;
        private bool _isAnalyzing = false;
        private static bool _sortDirection = true;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private bool _timer;
            

        public MainWindow()
        {
            InitializeComponent();

            deleteButton.IsEnabled = false;

            _disks = Service.GetDisks();
            _files = new List<File>();
            Settings.MinSize = 1024 * 1024 * 1024;
            Settings.MaxDate = DateTime.Now.AddDays(-7);
            Settings.Disks = _disks;           

            this.Loaded += MainWindow_Loaded;
            _timer = true;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int id = 0;

            foreach (var disk in _disks)
            {
                MenuItem diskMenuItem = new MenuItem();
                diskMenuItem.Header = disk;
                diskMenuItem.IsChecked = true;
                diskMenuItem.Name = "d" + id;
                diskMenuItem.Click += diskMenuItem_Click;

                disksMenuItem.Items.Add(diskMenuItem);
                id++;
            }

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _timer = !_timer;
            if(_timer) scanProgressLabel.Visibility = Visibility.Hidden;
            else scanProgressLabel.Visibility = Visibility.Visible;
        }

        void diskMenuItem_Click(object sender, EventArgs e)
        {
            int diskId = int.Parse(((MenuItem)sender).Name.Substring(1));
            MenuItem disk = (MenuItem)disksMenuItem.Items[diskId];
            disk.IsChecked = !disk.IsChecked;
        }

        private void analyzeButton_Click(object sender, RoutedEventArgs e)
        {
            if(!_isAnalyzing)
            {
               // scanProgressBar.Value = 0;
               // scanProgressLabel.Content = "0%";
                filesListView.Items.Clear();

                List<string> settingsDisks = new List<string>();

                foreach (MenuItem diskMenuItem in disksMenuItem.Items)
                {
                    if (diskMenuItem.IsChecked)
                    {
                        settingsDisks.Add(diskMenuItem.Header.ToString());
                    }
                }

                Settings.Disks = settingsDisks;

                _thread = new Thread(startAnalyze);
                _thread.Start();
            }
            else
            {
                stopAnalyze();
            }
        }

        private void stopAnalyze()
        {
            if (_isAnalyzing)
            {
                _thread.Abort();
                dispatcherTimer.Stop();
                scanProgressBar.IsIndeterminate = false;
                scanProgressLabel.Content = "";
                scanProgressBar.Value = 0;
                //scanProgressLabel.Content = "0%";
                filesListView.Items.Clear();

                _isAnalyzing = false;
                analyzeButton.Content = "Analyze";
                deleteButton.IsEnabled = false;
                selectAllButton.IsEnabled = false;
            }
        }

        private void startAnalyze()
        {
            _isAnalyzing = true;
            
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                scanProgressBar.IsIndeterminate = true;
                scanProgressLabel.Content = "Scanning in progress...";
                dispatcherTimer.Start();
                
                analyzeButton.Content = "Cancel analyze";
                deleteButton.IsEnabled = false;
            }));
            
            _files = Service.GetFiles();

            UpdateListView(_files);

            Dispatcher.BeginInvoke(new ThreadStart(delegate {
                scanProgressBar.Value = 100;
                dispatcherTimer.Stop();
                scanProgressLabel.Content = "Scanning completed!";
            }));
            
            _isAnalyzing = false;
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                scanProgressBar.IsIndeterminate = false;
                analyzeButton.Content = "Analyze";
                //deleteButton.IsEnabled = true;
            }));
        }

        private void UpdateListView(List<File> filesToUpdate)
        {
            foreach (var file in filesListView.Items)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate { filesListView.Items.Clear(); }));
            }

            foreach (var file in filesToUpdate)
            {
                Dispatcher.BeginInvoke(new ThreadStart(delegate { filesListView.Items.Add(file); }));
            }

            Dispatcher.BeginInvoke(new ThreadStart(delegate 
                {
                    if (filesListView.Items.Count > 0) selectAllButton.IsEnabled = true;
                    else selectAllButton.IsEnabled = false;
                }));
           
        }

        
        private void settingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.ShowDialog();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            List<File> filesToStay = new List<File>();
            List<File> filesToDelete = new List<File>();
            long totalSize = 0;

            foreach (var selectedFile in filesListView.SelectedItems)
            {
                filesToDelete.Add((File)selectedFile);
                totalSize += ((File)selectedFile).Size;
            }

            var response = MessageBox.Show(String.Format("That will free {0:0.#} Mb of space. Continue ?", (float)totalSize / 1048576.0f), "Confirmation needed", MessageBoxButton.YesNo);

            if (response == MessageBoxResult.Yes)
            {
                var result = Service.DeleteFiles(filesToDelete);

                if (result.Contains(false))
                {
                    MessageBox.Show("One or more files weren't deleted.");
                    analyzeButton_Click(null, null);
                }
                else
                {
                    foreach (var item in _files)
                    {
                        if (!filesToDelete.Contains(item))
                        {
                            filesToStay.Add(item);
                        }
                    }
                    UpdateListView(filesToStay);
                    scanProgressLabel.Content = "Deleting completed!"; 
                }                               
            }
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
                
        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            string[] arr = e.OriginalSource.ToString().Split(':');
            string str = (arr[arr.Length - 1]).Trim().ToLower();            

            switch (str)
            { 
                case "name":                    
                    Service.SortFiles(_files, SortingCriteria.FileName);
                    break;
                case "path":
                    Service.SortFiles(_files, SortingCriteria.FilePath);
                    break;
                case "last change date":
                    Service.SortFiles(_files, SortingCriteria.FileUseDate);
                    break;
                case "size":
                    Service.SortFiles(_files, SortingCriteria.FileSize);
                    break;
                case "score":
                    Service.SortFiles(_files, SortingCriteria.FileScore);
                    break;
                default:
                    break;
            }
            _sortDirection = !_sortDirection;
            if (_sortDirection)
            {
                _files.Reverse();
            }
            UpdateListView(_files);
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aw = new AboutWindow();
            aw.ShowDialog();
        }

        private void selectAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (filesListView.Items.Count > 0)
            {
                foreach (var item in filesListView.Items)
                {
                    filesListView.SelectedItems.Add(item);
                }
            }
        }

        private void filesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filesListView.SelectedItems.Count > 0) deleteButton.IsEnabled = true;
            else deleteButton.IsEnabled = false;
        }
        
        
      

    }
}
