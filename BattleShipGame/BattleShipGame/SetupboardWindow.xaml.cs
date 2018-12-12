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
using System.Windows.Shapes;

namespace BattleShipGame
{
    /// <summary>
    /// Interaction logic for SetupboardWindow.xaml
    /// </summary>
    public partial class SetupboardWindow : Window
    {
        Player[] Players;
        bool[] HasCompleted;
        int startindex;
        public SetupboardWindow(Player[] players)
        {
            InitializeComponent();
            Players = players;
            //We separate the booleans, because we shouldn't need them in the rest of the application
            HasCompleted = new bool[players.Length];
            for (int i = 0; i < HasCompleted.Length; i++)
            {
                HasCompleted[i] = false;
            }
            //Pick A Random player
            startindex = new Random().Next(Players.Length);
            this.Content = new BoardView(players[startindex]);
        }

        public bool SetupIsComplete { get; internal set; }
    }
}
