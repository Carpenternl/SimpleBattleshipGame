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
    /// Interaction logic for Ship.xaml
    /// </summary>
    public enum ShipTypes { Scout=2,Submarine=3,BattleShip=4, AirCraft_Carrier =5};

    public partial class Ship : UserControl
    {
        private int shipLength;

        public int ShipLength
        {
            get { return shipLength; }
            set { shipLength = value; }
        }

        public Ship(int v)
        {
            shipLength = v;
            InitializeComponent();
        }

        internal static IEnumerable<Ship> CreateShips(int length, int amount)
        {
            List<Ship> Result = new List<Ship>();
            for (int i = 0; i < amount; i++)
            {
                Result.Add(new Ship(length));
            }
            return Result;
        }
        public override string ToString()
        {
            return $"{(ShipTypes)shipLength}({shipLength})";
        }
    }
}
