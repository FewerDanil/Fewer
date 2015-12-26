using Fewer.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //test Service.GetDisks();
            foreach (var item in Service.GetDisks())
            {
                listBoxMain.Items.Add(item);
            }
            
            //test Service.GetFiles();
            foreach (File item in Service.GetFiles())
            {
                int pri = item.FilePrioritySize;
                listBoxMain.Items.Add(item.FileName + " Priority to delete = " + pri );
            }

        }
    }
}
