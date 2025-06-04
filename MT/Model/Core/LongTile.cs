using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Model
{
    public class LongTile : TileBase
    {
        public LongTile(int lanesCount)
        {
            ScoreValue = 2 * lanesCount;
            TileColor = Color.FromArgb(100, 255, 150); // Светло-зеленый
            RequiresHold = true;
        }

        public override void OnClick() { }
        public override void OnHold() { }
        public override void OnDoubleClick() { }
    }
}
