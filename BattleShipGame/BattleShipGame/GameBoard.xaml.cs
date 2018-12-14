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
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Grid
    {
        // The GameBoard Can show ships to the Players,
        public List<Ship> BoardShips
        {
            get { return (List<Ship>)GetValue(BoardShipsProperty); }
            set { SetValue(BoardShipsProperty, value); }
        }

        private bool showPreview;
        public bool ShowPreview
        {
            get
            {
                
                return showPreview;
            }
            set
            {
                // TODO Either remove or add events that show a preview of the shipplacement
                showPreview = value;
            }
        }

        // Using a DependencyProperty as the backing store for BoardShips.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoardShipsProperty =
            DependencyProperty.Register("BoardShips", typeof(List<Ship>), typeof(GameBoard), new PropertyMetadata(new List<Ship>(),ShipcolChanged));

        //Change the ships on the board
        private static void ShipcolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameBoard GB = d as GameBoard;
            List<Ship> NewShips = (List<Ship>)e.NewValue;
            List<Ship> OldShips = (List<Ship>)e.OldValue;
            if(OldShips.Count <= NewShips.Count)
            {
                //Add New ships
                foreach (var item in NewShips)
                {
                    if (!GB.Children.Contains(item))
                    {
                        GB.Children.Add(item);
                    }
                }
            }
            else
            {
                //Get the ships that are currently placed
                List<Ship> CurrentlyPlacedShips = (from s in GB.Children.Cast<UIElement>() where s.GetType() == typeof(Ship) select s).Cast<Ship>().ToList();
                // remove them from the UI
                foreach (var item in CurrentlyPlacedShips)
                {
                    if (!NewShips.Contains(item))
                    {
                        GB.Children.Remove(item);
                    }
                }
            }

        }

        #region MoveShip
        //The gameboard can Move ships
        public static Point GetShipPosition(DependencyObject obj)
        {
            return (Point)obj.GetValue(ShipPositionProperty);
        }

        public static void SetShipPosition(DependencyObject obj, Point value)
        {
            obj.SetValue(ShipPositionProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShipPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShipPositionProperty =
            DependencyProperty.RegisterAttached("ShipPosition", typeof(Point), typeof(GameBoard), new PropertyMetadata(default(Point),ShipPositionChanged,RoundPoint));
        //Round the values for the grid
        private static object RoundPoint(DependencyObject d, object baseValue)
        {
            Point Val = (Point)baseValue;
            Val.X = (int)Math.Floor(Val.X);
            Val.Y = (int)Math.Floor(Val.Y);
            return Val;
        }
        //Set The GridPosition
        private static void ShipPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Grid.SetRow(d as UIElement, (int)((Point)e.NewValue).Y);
            Grid.SetColumn(d as UIElement, (int)((Point)e.NewValue).Y);
        }
        #endregion
        public GameBoard()
        {
            InitializeComponent();
        }
        
        public void MoveShip(Ship arg,int column, int row)
        {
            Grid.SetRow(arg, row);
            Grid.SetColumn(arg, column);
        }
    }
}
