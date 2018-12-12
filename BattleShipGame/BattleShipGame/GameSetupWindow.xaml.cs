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

namespace BattleShipGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameSetupWindow : Window
    {
        public Action<string, string> Ready;
        public GameSetupWindow()
        {
            InitializeComponent();
        }

        private void PlayerNameChanged(object sender, TextChangedEventArgs e)
        {
            if (IsInitialized)
            {
                if (P1.Text.Length <= 0 | P2.Text.Length <= 0 | P1.Text.ToLower() == P2.Text.ToLower())
                {
                    GameStartbutton.IsEnabled = false;
                }
                else
                {
                    GameStartbutton.IsEnabled = true;
                }
            }
        }
        private void GameStartbutton_Click(object sender, RoutedEventArgs e)
        {
            Ready(P1.Text, P2.Text);
            Visibility = Visibility.Hidden;
        }
    }
}
