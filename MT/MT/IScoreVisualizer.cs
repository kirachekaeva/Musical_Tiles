using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT
{
    public interface IScoreVisualizer
    {
        void UpdateScoreDisplay(int score);
        void ShowGameOver(bool isWin);
    }
}
