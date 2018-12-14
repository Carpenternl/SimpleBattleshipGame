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
    /// Interaction logic for ShipItem.xaml
    /// </summary>
    public partial class ShipItem : Button
    {


        public int ShipAmount
        {
            get { return (int)GetValue(ShipAmountProperty); }
            set { SetValue(ShipAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShipAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShipAmountProperty =
            DependencyProperty.Register("ShipAmount", typeof(int), typeof(ShipItem), new PropertyMetadata(0,amountchanged));

        private static void amountchanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShipItem)d).Amount.Content = $"({(int)e.NewValue})";
        }

        public int ShipLength
        {
            get { return (int)GetValue(ShipLengthProperty); }
            set { SetValue(ShipLengthProperty, value); }
        }


        // Using a DependencyProperty as the backing store for ShipLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShipLengthProperty =
            DependencyProperty.Register("ShipLength", typeof(int), typeof(ShipItem), new PropertyMetadata(2,lchanged));

        private static void lchanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShipItem)d).ShipTtitle.Content = (ShipTypes)e.NewValue;
        }

        public ShipItem()
        {
            InitializeComponent();
            
            ShipTtitle.Content = (ShipTypes)ShipLength;
        }
    }
}
