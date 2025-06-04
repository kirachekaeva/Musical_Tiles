using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Model.Core
{
    public class TrapTile : TileBase
    {
        public TrapTile()
        {
            ScoreValue = 0;
            TileColor = Color.FromArgb(255, 100, 100); 
            IsTrap = true;
        }

        public override void OnClick() { }
        public override void OnHold() { }
        public override void OnDoubleClick() { }
    }
}
