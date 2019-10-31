using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using System.Linq;
using System.Reflection;

namespace TavlaWPF
{
    /// <summary>
    /// GameWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class GameWindow : Window
    {
        public string LocalPlayerName;
        public string RemotePlayerName = "RuskiBot";

        public Color TeamAtBottom = Color.Black;

        public Dictionary<Checker, Image> Checkers = new Dictionary<Checker, Image>();

        public GameWindow()
        {
            InitializeComponent();
            GetThingsReady();
        }

        public void GetThingsReady()
        {
            Checkers.Clear();
            for (var i = 1; i <= 30; i++)
            {
                var checkerImage = (Image)typeof(GameWindow).GetField($"Checker{i}", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
                Checkers.Add(new Checker(), checkerImage);
            }
        }

        public void UpdateComponent()
        {
            PlayerNameBox.Text = LocalPlayerName;
            OpponentNameBox.Text = RemotePlayerName;

            var topTeam = TeamAtBottom.GetOpponent();
            var slots = new byte[24];

            int blackBorneOffCount = 0, whiteBorneOffCount = 0, blackHitCount = 0, whiteHitCount = 0;

            foreach (var checker in Checkers)
            {
                if (checker.Key.State == CheckerState.InGame)
                {
                    slots[checker.Key.Slot]++;
                    checker.Value.Visibility = slots[checker.Key.Slot] <= 5 ? Visibility.Visible : Visibility.Hidden;
                    SetOverflowText(checker.Key.Slot, slots[checker.Key.Slot]);
                    var gridCoordinates = Coordinates.GetGridPosition(TeamAtBottom, checker.Key.Slot, slots[checker.Key.Slot]);
                    Grid.SetRow(MainGrid, (int)gridCoordinates.X);
                    Grid.SetColumn(MainGrid, (int)gridCoordinates.Y);
                }
                else if (checker.Key.State == CheckerState.BorneOff)
                {
                    checker.Value.Visibility = Visibility.Hidden;
                    if (checker.Key.Color == Color.White)
                    {
                        whiteBorneOffCount++;
                    }
                    else
                    {
                        blackBorneOffCount++;
                    }
                }
                else if (checker.Key.State == CheckerState.Hit)
                {
                    checker.Value.Visibility = Visibility.Hidden;
                    if (checker.Key.Color == Color.White)
                    {
                        whiteHitCount++;
                    }
                    else
                    {
                        blackHitCount++;
                    }
                }
            }

            if (whiteBorneOffCount > 0)
            {
                // todo
            }

            if (blackBorneOffCount > 0)
            {
                // todo
            }

            if (whiteHitCount > 0)
            {
                // todo
            }

            if (blackHitCount > 0)
            {
                // todo
            }
        }

        public void SetOverflowText(byte slot, byte checkerCount)
        {
            var textBox = (TextBox)typeof(GameWindow).GetField($"Slot{slot}OverflowIndicator", (BindingFlags)0x24).GetValue(this);
            textBox.Text = checkerCount <= 5 ? string.Empty : checkerCount.ToString();
        }

        public ImageSource GetImage(string assetName)
        {
            return new BitmapImage(new Uri($"/Assets/{assetName}", UriKind.Relative));
        }

        public ImageSource GetCheckerImage(Color color)
        {
            return GetImage($"{color}Checker.png");
        }

        public void SetCheckerColor(Image image, Color color)
        {
            image.Source = GetCheckerImage(color);
        }

        public void SetDiceNumber(Image image, byte number)
        {
            image.Source = GetDiceImage(number);
        }

        public ImageSource GetDiceImage(byte number)
        {
            if (number <= 0 || number > 6)
            {
                throw new Exception("Excuse me, what the fuck?");
            }

            return GetImage($"Dice_{number}.png");
        }

        private void GameWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var response = MessageBox.Show("Maçtan çıkmak için \"Evet\"e, programı kapamak için \"Hayır\"a basınız.",
                "Daha karpuz kesecektik!", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (response)
            {
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
                case MessageBoxResult.Yes:
                    e.Cancel = false;
                    break;
                case MessageBoxResult.No:
                    Application.Current.Shutdown();
                    break;
            }
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private void DisableMaximizeButton(object sender, EventArgs e)
        {
            var hwnd = new WindowInteropHelper((Window)sender).Handle;
            var value = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, value & ~WS_MAXIMIZEBOX);
        }
    }

    public class Checker
    {
        public Color Color;
        public CheckerState State;
        public byte Slot;
    }

    public enum CheckerState
    {
        BorneOff,
        Hit,
        InGame
    }

    public enum Color
    {
        White,
        Black
    }
}