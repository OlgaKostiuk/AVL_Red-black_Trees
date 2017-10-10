using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCollections
{
    public interface ITreeBalanced : ITree
    {
        bool IsBalanced();
    }
}
