using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lab1
{
    public class ImmutableTree<T> : ITree<T> where T : IComparable<T>
    {
        public ITree<T> Tree { get; }
        public BPlusTreeNode<T> Root { get; }
        public int Count => Tree.Count;
        public bool IsEmpty => Tree.IsEmpty;
        public IEnumerable<T> Nodes => Tree.Nodes;

        public ImmutableTree(ITree<T> tree)
        {
            Tree = tree;
            if (tree is ArrayTree<int>) Root = ((ArrayTree<T>)tree).Root;
            else if (tree is LinkedTree<int>) Root = ((LinkedTree<T>)tree).Root;
        }

        public void Add(T node) => throw new TreeException("Функция добавления недоступна.");
        public void Clear() => throw new TreeException("Функция очищения недоступна.");
        public bool Contains(T node) => Tree.Contains(node);
        public void Remove(T node) => throw new TreeException("Функция удаления недоступна.");

        public IEnumerator<T> GetEnumerator() => Tree.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
