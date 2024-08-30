using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class BPlusTreeNode<T> 
    {
        public int Order { get; set; }
        public List<T> Keys { get; set; } = new List<T>();
        public List<BPlusTreeNode<T>> Children { get; set; } = new List<BPlusTreeNode<T>>();
        public bool IsLeaf { get; set; } = true;
        public BPlusTreeNode<T>? Sibling { get; set; } = null;

        public BPlusTreeNode<T> Split()
        {
            var mid = Order / 2; // середина узла
            var newNode = new BPlusTreeNode<T>
            {
                Order = Order,
                Keys = new List<T>(Keys.Skip(mid)), // ключи из второй половины
                Children = new List<BPlusTreeNode<T>>(Children.Skip(mid)) // потомки из второй половины
            };

            Keys = Keys.Take(mid).ToList(); // ключи из первой половины
            Children = Children.Take(mid).ToList(); // потомки из второй половины

            if (newNode.Children.Count > 0)
                newNode.IsLeaf = false; // если есть потомки, то не лист

            return newNode;
        }

        public bool IsFull => Keys.Count == Order;

        public override string ToString()
        {
            return (IsLeaf ? "Leaf: " : "") + string.Join(" | ", Keys);
        }
    }
}
