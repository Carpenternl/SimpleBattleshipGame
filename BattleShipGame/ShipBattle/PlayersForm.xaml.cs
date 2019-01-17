using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for GameSetup.xaml
    /// </summary>
    public partial class PlayersForm : Page
    {
        internal ObservableCollection<Player> Players;
        private int index = 0;
        public PlayersForm()
        {
            InitializeComponent();
            Players = new ObservableCollection<Player>();
            Players.CollectionChanged += Players_CollectionChanged;
            PlayersView.ItemsSource = Players;
        }

        private void Players_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ReadyButton.IsEnabled = Players.Count > 1 ? true : false;
        }

        private void AddPlayer(object sender, RoutedEventArgs e)
        {
            Players.Add(new Player($"Player {index}"));
            index++;
        }

        private void DeletePlayer(object sender, RoutedEventArgs e)
        {
            string playername = ((Button)sender).Tag.ToString();
            IEnumerable<Player> query = (from Player p in Players where p.Name == playername select p);
            for (int i = Players.Count()-1; i >=0 ; i--)
            {
                if(Players[i].Name == playername)
                {
                    Players.Remove(Players[i]);
                }
            }
           
        }
        public event EventHandler Ready;
        private void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            Ready(this, e);
        }
    }
}
