using Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public interface ISerializableState
    {
        void SaveState(GameState state);
        GameState LoadState();
        void ValidateBeforeSerialization(object data);
        void ValidateState();
    }
}
