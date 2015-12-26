using Fewer.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Fewer.Client
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            minSizeTextBox.Text = ((float)Settings.MinSize / 1048576.0f).ToString();
            minDateDatePicker.SelectedDate = Settings.MinDate;

            this.Closing += SettingsWindow_Closing;
        }

        public void SettingsWindow_Closing(object sender, EventArgs e)
        {
            Settings.MinSize = (long)(float.Parse(minSizeTextBox.Text) * 1048576.0f);
            Settings.MinDate = Convert.ToDateTime(minDateDatePicker.SelectedDate);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
