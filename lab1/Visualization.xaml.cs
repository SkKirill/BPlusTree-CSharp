using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Msagl.Drawing;

namespace lab1
{
    /// <summary>
    /// Interaction logic for Visualization.xaml
    /// </summary>
    public partial class Visualization : Window
    {
        private Graph graph = new Graph();

        public Visualization()
        {
            InitializeComponent();
        }

        public void DrawBPlusTree<T>(BPlusTreeNode<T> node)
        {
            if (node is null || node.Keys.Count == 0)
            {
                Close();
                MessageBox.Show("Tree is empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AddNodes(node);
            AddEdges(node);

            graph.Attr.LayerDirection = LayerDirection.TB;
            GraphControl.Graph = graph;
        }

        // добавление узлов дерева
        public void AddNodes<T>(BPlusTreeNode<T> node)
        {
            if (node == null) return;

            graph.AddNode(node.ToString());

            for (int i = node.Children.Count - 1; i >= 0; i--)
            {
                AddNodes(node.Children[i]);
            }
        }

        // добавление связей между узлами
        public void AddEdges<T>(BPlusTreeNode<T> node)
        {
            if (node == null) return;

            for (int i = node.Children.Count - 1; i >= 0; i--)
            {
                graph.AddEdge(node.ToString(), node.Children[i].ToString());
                if (node.Children[i].Sibling is not null)
                    graph.AddEdge(node.Children[i].ToString(), node.Children[i].Sibling.ToString());
            }

            for (int i = node.Children.Count - 1; i >= 0; i--)
                AddEdges(node.Children[i]);
        }
    }
}
