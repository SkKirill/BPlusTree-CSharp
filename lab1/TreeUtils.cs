using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class TreeUtils
    {
        public delegate ITree<T> TreeConstructorDelegate<T>();
        public delegate bool CheckDelegate<T>(T value);
        public delegate void ActionDelegate<T>(T value);

        public static TreeConstructorDelegate<T> ArrayTreeConstructor<T>() where T : IComparable<T> => () => new ArrayTree<T>();
        public static TreeConstructorDelegate<T> LinkedTreeConstructor<T>() where T : IComparable<T> => () => new LinkedTree<T>();

        public static bool Exists<T>(ITree<T> tree, CheckDelegate<T> check) where T : IComparable<T>
        {
            return tree.Nodes.Any(check.Invoke);
        }

        public static ITree<T> FindAll<T>(ITree<T> originalTree, CheckDelegate<T> check, TreeConstructorDelegate<T> construct) where T : IComparable<T>
        {
            var tree = construct.Invoke();
            foreach (var node in originalTree)
                if (check.Invoke(node)) tree.Add(node);
            return tree;
        }

        public static void ForEach<T>(ITree<T> tree, ActionDelegate<T> action) where T : IComparable<T>
        {
            foreach (var node in tree)
                action(node);
        }

        public static bool CheckForAll<T>(ITree<T> tree, CheckDelegate<T> check) where T : IComparable<T>
        {
            return tree.Nodes.All(check.Invoke);
        }
    }
}
