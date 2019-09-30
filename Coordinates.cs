using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TavlaWPF
{
    public static class Coordinates
    {
        public const byte BOTTOM_ROW = 11;
        public const byte TOP_ROW = 1;
        public const byte LEFTMID_COLUMN = 6;
        public const byte RIGHTMOST_COLUMN = 13;

        public static Point GetGridPosition(Color relativeTeam, byte slot, byte orderInSlot)
        {
            orderInSlot = (byte)(Math.Min(orderInSlot, (byte)5) - 1);
            var x = GetXPosition(slot);
            var y = GetYPosition(slot, relativeTeam);

            if (y == BOTTOM_ROW)
            {
                y -= orderInSlot;
            }
            else if (y == TOP_ROW)
            {
                y += orderInSlot;
            }

            return new Point(x, y);
        }

        private static byte GetXPosition(byte slot)
        {
            if (slot < 1 || slot > 25)
            {
                throw new IndexOutOfRangeException();
            }

            if (slot > 12)
            {
                slot = (byte)(25 - slot);
            }

            if (slot < 7)
            {
                return (byte)(RIGHTMOST_COLUMN - (slot - 1));
            }

            return (byte)(LEFTMID_COLUMN - (slot - 7));
        }

        private static byte GetYPosition(byte slot, Color color)
        {
            return color == Color.Black ?
                (slot < 13 ? BOTTOM_ROW : TOP_ROW) :
                (slot < 13 ? TOP_ROW : BOTTOM_ROW);
        }
    }
}
