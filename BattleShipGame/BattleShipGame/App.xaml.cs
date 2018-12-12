using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BattleShipGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Player P1 = new Player();
        Player P2 = new Player();

        GameSetupWindow SetupWindow = new GameSetupWindow();
        SetupboardWindow BoardSetup;

        //Begin Off App
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Let players create Usernames
            SetupWindow.Ready = new Action<string, string>(StartSetup);
            SetupWindow.Show();
        }
        private void StartSetup(string p1, string p2)
        {
            // Set the Usernames of the Players
            P1.Username = p1;
            P2.Username = p2;
            Player[] Players = { P1, P2 };
            // Boardsetup handles the setup of all players, it is already programmed with variable player count in mind
            BoardSetup = new SetupboardWindow(Players);
            BoardSetup.Title = "Setup Game";
            BoardSetup.Closed += TryStart;
            BoardSetup.Show();
        }

        private void TryStart(object sender, EventArgs e)
        {
            if (BoardSetup is null || BoardSetup.SetupIsComplete)
            {
                // TODO Actually start the game
            }
            else
            {
                // Reset Setup
                SetupWindow.Visibility = Visibility.Visible;
            }
        }
    }
}
