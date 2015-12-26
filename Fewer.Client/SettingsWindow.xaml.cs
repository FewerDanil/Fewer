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

            switch (MainWindow.NominalComboBoxIndex)
            {
                case 0:
                    minSizeTextBox.Text = (Settings.MinSize / 1024).ToString();
                    break;
                case 1:
                    minSizeTextBox.Text = (Settings.MinSize / 1048576).ToString();
                    break;
                case 2:
                    minSizeTextBox.Text = (Settings.MinSize / 1073741824).ToString();
                    break;
            }

            maxDateDatePicker.SelectedDate = Settings.MaxDate;
            minSizeNominalComboBox.SelectedIndex = MainWindow.NominalComboBoxIndex;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (minSizeTextBox.Text.Length != 0)
            {
                switch (minSizeNominalComboBox.SelectedIndex)
                {
                    case 0:
                        Settings.MinSize = (long)(float.Parse(minSizeTextBox.Text) * 1024.0f);
                        break;
                    case 1:
                        Settings.MinSize = (long)(float.Parse(minSizeTextBox.Text) * 1048576.0f);
                        break;
                    case 2:
                        Settings.MinSize = (long)(float.Parse(minSizeTextBox.Text) * 1073741824.0f);
                        break;
                }
            }

            Settings.MaxDate = Convert.ToDateTime(maxDateDatePicker.SelectedDate);
            MainWindow.NominalComboBoxIndex = minSizeNominalComboBox.SelectedIndex;
            this.Close();
        }
    }
}
