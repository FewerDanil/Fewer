using Fewer.Library;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Fewer.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> disks;

        public MainWindow()
        {
            InitializeComponent();

            disks = Service.GetDisks();

            Settings.MinSize = 1 * 1024 * 1024;
            Settings.MinDate = DateTime.Now.AddDays(-7);
            Settings.Disks = disks;

            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int id = 0;

            foreach (var disk in disks)
            {
                MenuItem diskMenuItem = new MenuItem();
                diskMenuItem.Header = disk;
                diskMenuItem.IsChecked = true;
                diskMenuItem.Name = "d" + id;
                diskMenuItem.Click += diskMenuItem_Click;

                disksMenuItem.Items.Add(diskMenuItem);
                id++;
            }
        }

        void diskMenuItem_Click(object sender, EventArgs e)
        {
            int diskId = int.Parse(((MenuItem)sender).Name.Substring(1));
            MenuItem disk = (MenuItem)disksMenuItem.Items[diskId];
            disk.IsChecked = !disk.IsChecked;
        }

        private void analyzeButton_Click(object sender, RoutedEventArgs e)
        {
            scanProgressBar.Value = 0;
            scanProgressLabel.Content = "0%";

            List<string> settingsDisks = new List<string>();

            foreach (MenuItem diskMenuItem in disksMenuItem.Items)
            {
                if(diskMenuItem.IsChecked)
                {
                    settingsDisks.Add(diskMenuItem.Header.ToString());
                }
            }

            Settings.Disks = settingsDisks;

            filesListView.Items.Clear();

            List<File> files = Service.GetFiles();

            foreach (var file in files)
            {
                filesListView.Items.Add(file);
            }

            scanProgressBar.Value = 100;
            scanProgressLabel.Content = "100%";
        }

        private void settingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.ShowDialog();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            List<File> filesToDelete = new List<File>();
            long totalSize = 0;

            foreach(var selectedFile in filesListView.SelectedItems)
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
                }

                analyzeButton_Click(null, null);
            }
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
