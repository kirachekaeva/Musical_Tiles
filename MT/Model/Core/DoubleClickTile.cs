using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DoubleClickTile : TileBase
    {
        public DoubleClickTile(int lanesCount)
        {
            ScoreValue = 3 * lanesCount;
            TileColor = Color.FromArgb(255, 255, 100);
            RequiresDoubleClick = true;
        }

        public override void OnClick() { }
        public override void OnHold() { }
        public override void OnDoubleClick() { }
    }
}
