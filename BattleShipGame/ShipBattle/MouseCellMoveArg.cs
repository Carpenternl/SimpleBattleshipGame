using System;
using System.Windows;

namespace ShipBattle
{
    internal class MouseCellMoveArg : EventArgs
    {
        public Point newValue;

        public MouseCellMoveArg(Point newValue)
        {
            this.newValue = newValue;
        }
    }
}