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
        //The Boardview allows the player to place his ships on the board.
        // We have a Player
        Player Owner;
        // we have a Board
        GameBoard B;
        // We have a collection of ships
        List<Ship> TheShipCollection;
        // The player has a list of buttons that allow players to pick shiptypes
        List<ShipItem> ShipButtons;
        // When the player wants to manipulate a ship, We need to keep track of it:
        Ship ShipToManipulate;
        // We need to know if all the ships have been placed to allow the game to continue
        public bool AllShipsArePlaced { get; private set; }
        
        private Ship currentSelection;
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

        public bool PlacingShip { get; private set; }

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
             *  Ships Are Created Here, If you want the player to be able to change the ship selection,
             *  you should insert your solution here 
             */
            SetupShipButtons(ShipList, 2, 4); // 4 scouts
            SetupShipButtons(ShipList, 3, 3); // 3 submarines
            SetupShipButtons(ShipList, 4, 2); // 2 Battleships
            SetupShipButtons(ShipList, 5, 1); // 1 Aircraft Carrier
            Ships = ShipList;
        }

        private void SetupShipButtons(List<Ship> ShipList, int length, int amount)
        {
            ShipList.AddRange(Ship.CreateShips(amount, length));
            CreateButton(ShipList, length, amount);
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

        private void CreateButton(List<Ship> ShipList, int l, int amount)
        {
            ShipItem ShipListButton = new ShipItem()
            {
                ShipLength = l,
                ShipAmount = amount,
            };
            ShipListButton.Click += SetupAddShipAction;
            ShipListView.Children.Add(ShipListButton);
        }

        // Allow the player to set Ships on the board
        private void SetupAddShipAction(object sender, RoutedEventArgs e)
        {
            PlacingShip = true;
            ShipItem ClickedBttn = sender as ShipItem;
            // When the mouse moves over the grid, show a preview of the the shipplacement
            Board.ShowPreview = true;
            int v = ClickedBttn.ShipLength;
            CurrentSelection = Ships.Where(x => x.ShipLength == v).ToList()[0];
            Board.MouseEnter += Board_MouseEnter;
            Board.MouseLeave += Board_MouseLeave;
            Board.MouseLeftButtonUp += AttemptToAddShip;

        }

        private void AttemptToAddShip(object sender, MouseButtonEventArgs e)
        {
            Grid Grd = sender as Grid;
            Point Clickpostion = e.GetPosition(Grd);
            if (SpaceIsAvailable(Clickpostion))
            {
                PlaceShip(Clickpostion);
                StopShipAdd(sender, e);
            }
        }

        private void StopShipAdd(object sender, MouseButtonEventArgs e)
        {
            if (!PlacingShip)
                return;
            currentSelection = null;
            try
            {
                Board.Children.Remove(PreviewBorder);
            }
            catch (Exception)
            {
            }
            Board.MouseEnter -= Board_MouseEnter;
            Board.MouseLeave -= Board_MouseLeave;
            Board.MouseLeftButtonUp -= AttemptToAddShip;
        }

        private void PlaceShip(Point clickpostion)
        {
            Ships.Remove(currentSelection);
            double R;
            double C;
            getgridpointResult(out R, out C, clickpostion, Board);
            Grid.SetRow(currentSelection, (int)R);
            Grid.SetColumn(currentSelection, (int)C);
            Grid.SetRowSpan(currentSelection, currentSelection.ShipLength);
            Board.Children.Add(currentSelection);

        }

        private bool SpaceIsAvailable(Point clickpostion)
        {
            Ship ThisShip = CurrentSelection;
            double R;
            double C;
            UIElementCollection PlacedShips = Board.Children;
            if (PlacedShips is null || PlacedShips.Count <= 1)
                return true;
            if (getgridpointResult(out R, out C, clickpostion, Board))
            {
                //ShipRec 1;
                int Y0 = (int)R - 1;
                int X0 = (int)C - 1;
                int X1 = Grid.GetColumnSpan(ThisShip) + 2;
                int Y1 = Grid.GetRowSpan(ThisShip) + 2;
                Rect Ship1 = new Rect(X0, Y0, X1, Y1);
                for (int i = 0; i < PlacedShips.Count; i++)
                {
                    int Y01 = Grid.GetRow(PlacedShips[i]);
                    int X01 = Grid.GetColumn(PlacedShips[i]);
                    int Y11 = Grid.GetRowSpan(PlacedShips[i]);
                    int X11 = Grid.GetColumnSpan(PlacedShips[i]);
                    Rect ship2 = new Rect(X01, Y01, X11, Y11);
                    if (Ship1.IntersectsWith(ship2))
                        return false;

                }
                return true;
            }
            return false;

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
        Border PreviewBorder = new Border() { BorderBrush = new SolidColorBrush(Colors.Purple), BorderThickness = new Thickness(2) };
        private void Board_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
            Board.Children.Add(PreviewBorder);

            }
            catch (Exception)
            { 
            }
            Board.MouseMove += MovePreview;
        }

        private Func<Ship, bool> ShipSizeIs(int v)
        {
            return new Func<Ship, bool>(x =>
            {
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
