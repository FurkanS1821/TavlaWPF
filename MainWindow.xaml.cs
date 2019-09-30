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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TavlaWPF
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EnterIPBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[0-9.]+");
            e.Handled = !regex.IsMatch(e.Text);
        }

        public bool IsIPValid(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                return false;
            }

            var splitValues = ip.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            return splitValues.All(x => byte.TryParse(x, out _));
        }

        private void EnterPort_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[0-9]+");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void StartBotMatchButton_OnClick(object sender, RoutedEventArgs e)
        {
            var playerName = PlayerNameBox.Text;

            if (string.IsNullOrWhiteSpace(playerName))
            {
                MessageBox.Show("Lütfen isminizi giriniz!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var game = new GameWindow {LocalPlayerName = playerName};
            Hide();
            game.Show();
            game.Closed += delegate { Show(); };
        }
    }
}
