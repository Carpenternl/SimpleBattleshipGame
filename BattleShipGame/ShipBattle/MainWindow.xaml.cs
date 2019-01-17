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

namespace ShipBattle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PlayersForm setupPlayers;
        BoardSetup setupBoard;
        List<Player> Players;
        private int CurrentPlayerIndex { get; set; }
        Player CurrentPlayer { get { return Players[CurrentPlayerIndex]; } }
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenGameSetup();
        }

        private void OpenGameSetup()
        {
            if (setupPlayers is null)
                setupPlayers = new PlayersForm();
            setupPlayers.Ready += Setup_Ready;
            this.Content = setupPlayers;
        }

        private void Setup_Ready(object sender, EventArgs e)
        {
            Players = ((PlayersForm)sender).Players.ToList();
            CurrentPlayerIndex = new Random().Next(Players.Count);
            BoardSetup setupBoard = new BoardSetup();
            setupBoard.Player = CurrentPlayer;
            this.Content = setupBoard;
            //Pick A Random Player

        }
        // Allows for easy looping
        private void NextPlayer()
        {
            CurrentPlayerIndex--;
            if (CurrentPlayerIndex < 0)
                CurrentPlayerIndex = Players.Count - 1;
        }
    }
}
