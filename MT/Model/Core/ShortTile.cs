using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Model
{
    public class ShortTile : TileBase
    {
        public ShortTile(int lanesCount)
        {
            ScoreValue = 1 * lanesCount;
            TileColor = Color.FromArgb(100, 200, 255); // Яркий голубой
        }

        public override void OnClick() { }
        public override void OnHold() { }
        public override void OnDoubleClick() { }
    }
}
