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
    /// Interaction logic for BoardSetup.xaml
    /// </summary>
    public partial class BoardSetup : Page
    {
        public List<Ship> Ships { get; set; }

        private Border ShipPreviewBorder;

        private Ship selectedShip;
        private Point OldValue;

        public Ship SelectedShip
        {
            get { return selectedShip; }
            set
            {
                if (value != null)
                {
                    
                    ShipPreviewBorder.SetValue(Grid.ColumnSpanProperty, value.Length);
                }
                selectedShip = value;
            }
        }

        private void BoardSetup_CellChanged(object sender, EventArgs e)
        {
            Grid grid = sender as Grid;
            Point cell = ((MouseCellMoveArg)e).newValue;
            try
            {
            ShipPreviewBorder.SetValue(Grid.ColumnProperty, cell.X);
            ShipPreviewBorder.SetValue(Grid.RowProperty, cell.Y);

            } catch( Exception )
            {
                // TODO: Allow MouseMovement 
            }
        }

        public BoardSetup()
        {
            InitializeComponent();
        }

        internal Player Player { get; set; }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Ships = CreateShips();
            ((Grid)sender).DataContext = Player;
            ShipListView.ItemsSource = Ships;
            if (ShipPreviewBorder is null)
            {
                ShipPreviewBorder = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(2),
                    Visibility = Visibility.Hidden,
                };
            }
            Board.Children.Add(ShipPreviewBorder);
            Board.MouseEnter += CheckIfPreviewEnabled;
            Board.MouseLeave += HidePreview;
            SetBoardSize();
        }

        private void HidePreview(object sender, MouseEventArgs e)
        {
            ShipPreviewBorder.Visibility = Visibility.Hidden;
            CellChanged -= BoardSetup_CellChanged;
        }

        private void CheckIfPreviewEnabled(object sender, MouseEventArgs e)
        {
            ShipPreviewBorder.Visibility = selectedShip != null ? Visibility.Visible : Visibility.Hidden;
            CellChanged += BoardSetup_CellChanged;
        }

        private void SetBoardSize()
        {
            this.Board.ColumnDefinitions.Clear();
            this.Board.ShowGridLines = true;
            for (int i = 0; i < GameRules.BoardWidth; i++)
            {
                Board.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int j = 0; j < GameRules.BoardHeight; j++)
            {
                Board.RowDefinitions.Add(new RowDefinition());
            }
        }

        private List<Ship> CreateShips()
        {
            return new List<Ship>()
            {
                new Ship(2),new Ship(2),new Ship(2),new Ship(2),
                new Ship(3),new Ship(3),new Ship(3),
                new Ship(4),new Ship(4),
                new Ship(5)
            };
        }

        private void SelectShip(object sender, MouseButtonEventArgs e)
        {
            ListView Target = sender as ListView;
            SelectedShip = (Ship)Target.SelectedItem;
        }
        public event EventHandler CellChanged;
        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            Grid _grid = sender as Grid;
            double grid_Width = _grid.ActualWidth;
            double grid_Height = _grid.ActualHeight;
            double grid_Collumns = _grid.ColumnDefinitions.Count;
            double grid_Rows = _grid.RowDefinitions.Count;
            double mouse_X = e.GetPosition(_grid).X;
            double Mouse_Y = e.GetPosition(_grid).Y;
            int mouseRow = (int)Math.Floor(Mouse_Y * grid_Rows / grid_Height);
            int mouseCol = (int)Math.Floor(mouse_X * grid_Collumns / grid_Width);
            Point NewValue = new Point(mouseRow, mouseCol);
            if (NewValue != OldValue)
            {
                OldValue = NewValue;
                CellChanged(_grid, new MouseCellMoveArg(NewValue));
            }
            /*  Height | rows
             *  MouseY |
             */
        }
    }
}
