using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core
{
    public class TileGenerator
    {
        private readonly Random _random = new Random();
        private readonly int _lanesCount;
        private int _score = 0;

        public TileGenerator(int lanesCount)
        {
            _lanesCount = lanesCount;
        }

        public void UpdateScore(int score)
        {
            _score = score;
        }

        public TileBase GenerateTile()
        {
            int tileType = _random.Next(0, 100);

            if (_score > 50 && tileType > 90 || _score > 100 && tileType > 80)
            {
                return new DoubleClickTile(_lanesCount);
            }

            if (tileType < 5)
            {
                return new TrapTile();
            }

            if (tileType < 30)
            {
                return new LongTile(_lanesCount);
            }

            return new ShortTile(_lanesCount);
        }
    }
}
