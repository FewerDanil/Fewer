using Fewer.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            List<File> files = Service.GetFiles(new Settings(1000000, DateTime.Now.Add(TimeSpan.FromDays(-100)), disks));

            foreach(var file in files)
            {
                filesListView.Items.Add(file);
            }
        }

        void diskMenuItem_Click(object sender, EventArgs e)
        {
            int diskId = int.Parse(((MenuItem)sender).Name.Substring(1));
            MenuItem disk = (MenuItem)disksMenuItem.Items[diskId];
            disk.IsChecked = !disk.IsChecked;
        }
    }
}
