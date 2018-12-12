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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Page
    {
        public bool AllShipsArePlaced { get; private set; }
        private Ship currentSelection;


        public static int GetShipLengthProperty(DependencyObject obj)
        {
            return (int)obj.GetValue(ShipLengthPropertyProperty);
        }

        public static void SetShipLengthProperty(DependencyObject obj, int value)
        {
            obj.SetValue(ShipLengthPropertyProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShipLengthProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShipLengthPropertyProperty =
            DependencyProperty.RegisterAttached("ShipLengthProperty", typeof(int), typeof(BoardView), new PropertyMetadata(0));


        public Ship CurrentSelection
        {
            get
            {
                return currentSelection;
            }
            set
            {
                if (!(currentSelection is null))
                {
                    currentSelection.ClearValue(BackgroundProperty);
                }
                currentSelection = value;
                currentSelection.Background = new SolidColorBrush(Colors.Green);
            }
        }

        public Player CurrentPlayer;
        public List<Ship> Ships;
        public BoardView(Player player)
        {
            InitializeComponent();
            SetGrid(10, 10);
            CurrentPlayer = player;
            PlayerNameLabel.Content = player.Username;
            List<Ship> ShipList = new List<Ship>();
            
            /*  
             *  Ships Are Created Here, If you want the player to be able to change the shipselection,
             *  you should insert your solution here 
             */
            ShipList.AddRange(Ship.CreateShips(4, 2));
            CreateButton(ShipList, 2);
            ShipList.AddRange(Ship.CreateShips(3, 3));
            CreateButton(ShipList,3);
            ShipList.AddRange(Ship.CreateShips(2, 4));
            CreateButton(ShipList, 4);
            ShipList.AddRange(Ship.CreateShips(1, 5));
            CreateButton(ShipList, 5);
            Ships = ShipList;
            // TODO Show Board
        }
        /// <summary>
        /// Creates the Rows and Columns for the Grid
        /// </summary>
        private void SetGrid(int x, int y)
        {
            Board.RowDefinitions.Clear();
            Board.ColumnDefinitions.Clear();
            for (int i = 0; i < x; i++)
            {
                Board.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < y; i++)
            {
                Board.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void CreateButton(List<Ship> ShipList, int l)
        { 
            Button ShipListButton = new Button()
            {
                Content = $"{(ShipTypes)l}" +
                $" ({ShipList.Count(ShipSizeIs(l))})"
            };
            SetShipLengthProperty(ShipListButton, l);
            ShipListButton.Click += StartShipAdd;
            ShipListView.Children.Add(ShipListButton);
        }
        
        private void StartShipAdd(object sender, RoutedEventArgs e)
        {
            Button Target = sender as Button;
            int v = GetShipLengthProperty(Target);
            CurrentSelection = Ships.Where(x => x.ShipLength == v).ToList()[0];
            Board.MouseEnter += Board_MouseEnter;
            Board.MouseLeave += Board_MouseLeave;
            
        }
        

        private void MovePreview(object sender, MouseEventArgs e)
        {
            Grid Target = sender as Grid;
            double row;
            double col;
            bool f = getgridpointResult(out row, out col, e.GetPosition(Target), Target);
        }

        private bool getgridpointResult(out double row, out double col, Point e, Grid sender)
        {
            double W = sender.ActualWidth;
            double H = sender.ActualHeight;
            int WG = sender.ColumnDefinitions.Count;
            int HG = sender.RowDefinitions.Count;
            col = e.X * WG / W;
            row = e.Y * HG / H;
            Grid.SetColumn(PreviewBorder, (int)Math.Floor(col));
            Grid.SetRow(PreviewBorder, (int)Math.Floor(row));
            return true;
        }
        
        private void Board_MouseLeave(object sender, MouseEventArgs e)
        {
            Board.MouseMove -= MovePreview;
            Board.Children.Remove(PreviewBorder);
        }
        Border PreviewBorder = new Border() { BorderBrush = new SolidColorBrush(Colors.Purple), BorderThickness = new Thickness(2)};
        private void Board_MouseEnter(object sender, MouseEventArgs e)
        {
            Board.Children.Add(PreviewBorder);
            Board.MouseMove += MovePreview;
        }

        private Func<Ship, bool> ShipSizeIs(int v)
        {
            return new Func<Ship, bool>(x=>{
                if (x.ShipLength == v)
                    return true;
                return false;
            });
        }

        private void SavePlayerBoard(object sender, RoutedEventArgs e)
        {
            // Don't do anything if the ships are not placed
            if (!(AllShipsArePlaced))
                return;
            // Otherwise Add the Placed ships to the Player
            //Player.Ships = Board.Ships;
        }
    }
}
