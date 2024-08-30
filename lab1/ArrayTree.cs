using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class ArrayTree<T> : ITree<T> where T : IComparable<T>
    {
        // возвращает количество значений в листьях дерева
        public int Count
        {
            get
            {
                if (Root is null) return 0;

                var node = Root;
                while (node.IsLeaf == false)
                    node = node.Children[0];

                var count = 0;
                while (node is not null)
                {
                    count += node.Keys.Count;
                    node = node.Sibling;
                }

                return count;
            }
        }

        public bool IsEmpty => Count == 0;

        // возвращает все значения в листьях дерева
        public IEnumerable<T> Nodes
        {
            get
            {
                if (Root is null) return Enumerable.Empty<T>();

                var node = Root;
                while (node.IsLeaf == false)
                    node = node.Children[0];

                var nodes = new T[Count];
                var index = 0;
                while (node is not null)
                {
                    foreach (var key in node.Keys)
                        nodes[index++] = key;
                    node = node.Sibling;
                }
                
                return nodes;
            }
        }

        public BPlusTreeNode<T> Root { get; set; }
        public int Order { get; set; }

        public ArrayTree(int order = 4)
        {
            Root = new BPlusTreeNode<T> { Order = order };
            Order = order;
        }

        public void Add(T value)
        {
            // если значение в дереве, return
            if (Contains(value)) return;

            if (Root is null)
            {
                Root = new BPlusTreeNode<T> { Order = Order };
                Root.Keys.Add(value);
                return;
            }

            // поиск листа для добавления значения
            var stack = new Stack<BPlusTreeNode<T>>();
            var current = Root;
            while (current.IsLeaf == false)
            {
                stack.Push(current);
                var index = 0;
                while (index < current.Keys.Count && current.Keys[index].CompareTo(value) < 0) index++;
                current = current.Children[index];
            }

            current.Keys.Add(value);
            current.Keys.Sort();

            // если количество значений больше допустимого
            while (current.IsFull)
            {
                var newNode = current.Split(); // вторая половина после разделения
                var midKey = current.Keys[^1]; // current стал первой половиной, поэтому срединный элемент стал последним
                if (current.Children.Count > 0)
                    current.Keys.Remove(midKey);

                if (stack.Count == 0)
                {
                    if (current.IsLeaf) current.Sibling = newNode; // листья ссылаются на соседа
                    Root = new BPlusTreeNode<T> // новый родитель половинок
                    {
                        Order = Order,
                        Keys = new List<T> { midKey },
                        Children = new List<BPlusTreeNode<T>> { current, newNode },
                        IsLeaf = false
                    };
                    return;
                }

                var parent = stack.Pop();
                var index = 0;
                while (index < parent.Keys.Count && parent.Keys[index].CompareTo(midKey) < 0) index++; // ищем индекс для midKey

                parent.Keys.Insert(index, midKey);
                if (newNode.IsLeaf)
                {
                    newNode.Sibling = parent.Children[index].Sibling; // листья ссылаются на соседа
                    parent.Children[index].Sibling = newNode; // листья ссылаются на соседа
                }
                parent.Children.Insert(index + 1, newNode);
                current = parent;
            }
        }

        public void Clear()
        {
            Root = null;
        }

        public bool Contains(T node)
        {
            return Nodes.Contains(node);
        }

        public void Remove(T value)
        {
            if (Root is null) return;

            // поиск листа со значением
            var stack = new Stack<BPlusTreeNode<T>>();
            var current = Root;
            while (current.IsLeaf == false)
            {
                stack.Push(current);
                var index = 0;
                while (index < current.Keys.Count && current.Keys[index].CompareTo(value) < 0) index++;
                current = current.Children[index];
            }

            if (current.Keys.Contains(value) == false) return;

            current.Keys.Remove(value);

            // балансировка
            while (current != Root)
            {
                var parent = stack.Pop();
                var index = parent.Children.IndexOf(current);

                // если недостаточное количество значений в узле
                if (current.Keys.Count < Order / 2 - 1)
                {
                    // Пытаемся одолжить значение у левого соседа
                    if (index > 0 && parent.Children[index - 1].Keys.Count > Order / 2 - 1)
                    {
                        var leftSibling = parent.Children[index - 1];
                        current.Keys.Insert(0, leftSibling.Keys[^1]);
                        leftSibling.Keys.Remove(leftSibling.Keys[^1]);
                        parent.Keys[index - 1] = leftSibling.Keys[^1];
                    }
                    // Пытаемся одолжить значение у правого соседа
                    else if (index < parent.Children.Count - 1 && parent.Children[index + 1].Keys.Count > Order / 2 - 1)
                    {
                        var rightSibling = parent.Children[index + 1];
                        current.Keys.Add(parent.Keys[0]);
                        parent.Keys.RemoveAt(0);
                        current.Children.Add(rightSibling.Children[0]);
                        rightSibling.Children.RemoveAt(0);
                        parent.Keys.Add(rightSibling.Keys[0]);
                        rightSibling.Keys.RemoveAt(0);
                    }
                    // Слияние с левым соседом
                    else if (index > 0)
                    {
                        var leftSibling = parent.Children[index - 1];
                        leftSibling.Sibling = parent.Children[index].Sibling;

                        if (parent == Root && parent.Children.Count == Order / 2)
                        {
                            parent.Keys.Insert(0, leftSibling.Keys[^1]);
                            leftSibling.Keys.Remove(leftSibling.Keys[^1]);
                            parent.Children.Clear();
                            parent.Children.AddRange(leftSibling.Children);
                            parent.Children.AddRange(current.Children);
                        }
                        else
                        {
                            leftSibling.Keys.AddRange(current.Keys);
                            leftSibling.Children.AddRange(current.Children);
                            parent.Keys.RemoveAt(index - 1);
                            parent.Children.RemoveAt(index);
                        }
                    }
                    // Слияние с правым соседом
                    else
                    {
                        var rightSibling = parent.Children[index + 1];
                        parent.Children[index].Sibling = rightSibling.Sibling;

                        if (parent == Root && parent.Children.Count == Order / 2)
                        {
                            parent.Keys.Add(rightSibling.Keys[^1]);
                            rightSibling.Keys.Remove(rightSibling.Keys[^1]);
                            parent.Children.Clear();
                            parent.Children.AddRange(current.Children);
                            parent.Children.AddRange(rightSibling.Children);
                        }
                        else
                        {
                            current.Keys.AddRange(rightSibling.Keys);
                            current.Children.AddRange(rightSibling.Children);
                            parent.Keys.RemoveAt(index);
                            parent.Children.RemoveAt(index + 1);
                        }

                        // Если корень стал пустым, то заменяем корень его единственным потомком
                        if (parent == Root && parent.Keys.Count == 0)
                        {
                            Root = current;
                            current.IsLeaf = true;
                        }
                    }
                }

                // если родитель содержит фиктивное (все реальные значения хранятся в листах) значение, которое мы удалили из листа
                if (parent.Keys.Contains(value))
                {
                    index = parent.Keys.IndexOf(value);
                    parent.Keys[index] = FindReplacement(parent.Children[index]); // находим самое правое значение в листе
                }

                current = parent;
            }

            if (Root is { IsLeaf: true, Keys.Count: 0 })
                Root = null;
        }

        private T FindReplacement(BPlusTreeNode<T> node)
        {
            while (node.IsLeaf == false) node = node.Children[^1];
            return node.Keys[^1];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
