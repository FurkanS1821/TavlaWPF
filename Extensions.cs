using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavlaWPF
{
    public static class Extensions
    {
        public static Color GetOpponent(this Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}
