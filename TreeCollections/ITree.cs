using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCollections
{
    public interface ITree
    {
        void Init(int[] ini);
        void Add(int val);
        void Del(int val);
        int[] ToArray();
        int Size();
        int Height(); //возвращает максимальное количество уровней не важно на какой ветке
        int Width(); //возвращает максимальное количество узлов не важно на каком уровне
        int Nodes(); //количество узлов, у которых есть хотя бы один потомок
        int Leaves(); //количество узлов, которые не имеют потомков
        void Reverse();
        void Clear();
        bool Equal(ITree tree);
    }
}
