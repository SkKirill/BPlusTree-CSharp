using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public interface ITree<T> : IEnumerable<T>
    {
        int Count { get; }
        bool IsEmpty { get; }
        IEnumerable<T> Nodes { get; }

        void Add(T node);
        void Clear();
        bool Contains(T node);
        void Remove(T node);
    }
}
