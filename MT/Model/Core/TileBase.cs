using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Model
{
    public abstract class TileBase : ITileAction
    {
        public int ScoreValue { get; protected set; }
        public Color TileColor { get; protected set; }
        public bool IsTrap { get; protected set; } = false;
        public bool RequiresHold { get; protected set; } = false;
        public bool RequiresDoubleClick { get; protected set; } = false;
        public bool WasClicked { get; set; } = false;

        public abstract void OnClick();
        public abstract void OnHold();
        public abstract void OnDoubleClick();
    }
}
