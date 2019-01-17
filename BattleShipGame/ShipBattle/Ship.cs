using System;

namespace ShipBattle
{
    public class Ship
    {
        public int Length;

        public Ship(int length)
        {
            Length = length;
        }
        public override string ToString()
        {
            return $"{Ship.getName(Length)} ({Length})";
        }

        private static object getName(int length)
        {
            switch (length)
            {
                case 2: return "Scout";
                case 3: return "Submarine";
                case 4: return "BattleShip";
                case 5: return "Aircraft Carrier";
                default:
                    return "UNDEFINED";
            }
        }
    }
}