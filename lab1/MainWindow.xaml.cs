using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ITree<int> intTree = new ArrayTree<int>();
        private ITree<string> stringTree = new ArrayTree<string>();
        private ITree<Book> bookTree = new ArrayTree<Book>();

        private TreeVariations variation = TreeVariations.Int;
        private bool isIntTreeImmutable;
        private bool isStringTreeImmutable;
        private bool isBookTreeImmutable;

        public MainWindow()
        {
            InitializeComponent();
            TreeClassComboBox.SelectedIndex = 0;
            TreeTypeComboBox.SelectedIndex = 0;
            var elements = new List<int> { 1, 4, 7, 10, 17, 21, 31, 25, 19, 20, 8, 9 }; // готовое дерево, которое создается при старте программы
            foreach (var element in elements)
                intTree.Add(element);
        }

        private void CreateTree()
        {
            switch (variation)
            {
                case TreeVariations.Int:
                    if (TreeClassComboBox.SelectedIndex == 0) intTree = new ArrayTree<int>();
                    else intTree = new LinkedTree<int>();
                    break;
                case TreeVariations.String:
                    if (TreeClassComboBox.SelectedIndex == 0) stringTree = new ArrayTree<string>();
                    else stringTree = new LinkedTree<string>();
                    break;
                case TreeVariations.Book:
                    if (TreeClassComboBox.SelectedIndex == 0) bookTree = new ArrayTree<Book>();
                    else bookTree = new LinkedTree<Book>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetTreeClass(object sender, RoutedEventArgs e)
        {
            CreateTree();
        }

        private void SetTreeType(object sender, RoutedEventArgs e)
        {
            var treeType = TreeTypeComboBox.SelectedIndex;
            variation = (TreeVariations)Enum.Parse(typeof(TreeVariations), treeType.ToString());
            CreateTree();
        }

        // отображение дерева в новом окне
        private void DrawBinaryTree(object sender, RoutedEventArgs e)
        {
            var window = new Visualization();
            window.Show();

            switch (variation)
            {
                case TreeVariations.Int:
                    if (intTree is ArrayTree<int>) window.DrawBPlusTree(((ArrayTree<int>)intTree).Root);
                    else if (intTree is LinkedTree<int>) window.DrawBPlusTree(((LinkedTree<int>)intTree).Root);
                    else window.DrawBPlusTree(((ImmutableTree<int>)intTree).Root);
                    break;
                case TreeVariations.String:
                    if (stringTree is ArrayTree<string>) window.DrawBPlusTree(((ArrayTree<string>)stringTree).Root);
                    else if (stringTree is LinkedTree<string>) window.DrawBPlusTree(((LinkedTree<string>)stringTree).Root);
                    else window.DrawBPlusTree(((ImmutableTree<string>)stringTree).Root);
                    break;
                case TreeVariations.Book:
                    if (bookTree is ArrayTree<Book>) window.DrawBPlusTree(((ArrayTree<Book>)bookTree).Root);
                    else if (bookTree is LinkedTree<Book>) window.DrawBPlusTree(((LinkedTree<Book>)bookTree).Root);
                    else window.DrawBPlusTree(((ImmutableTree<Book>)bookTree).Root);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // добавление значения
        private void AddValue(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (variation)
                {
                    case TreeVariations.Int:
                        intTree.Add(int.Parse(TreeValueTextBox.Text));
                        break;
                    case TreeVariations.String:
                        stringTree.Add(TreeValueTextBox.Text);
                        break;
                    case TreeVariations.Book:
                        bookTree.Add(Book.Parse(TreeValueTextBox.Text));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (TreeException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Value type error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            TreeValueTextBox.Clear();
        }

        // удаление значения
        private void RemoveValue(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (variation)
                {
                    case TreeVariations.Int:
                        intTree.Remove(int.Parse(TreeValueTextBox.Text));
                        break;
                    case TreeVariations.String:
                        stringTree.Remove(TreeValueTextBox.Text);
                        break;
                    case TreeVariations.Book:
                        bookTree.Remove(Book.Parse(TreeValueTextBox.Text));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (TreeException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Value type error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            TreeValueTextBox.Clear();
        }

        // удаление всех значений
        private void ClearTree(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (variation)
                {
                    case TreeVariations.Int:
                        intTree.Clear();
                        break;
                    case TreeVariations.String:
                        stringTree.Clear();
                        break;
                    case TreeVariations.Book:
                        bookTree.Clear();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (TreeException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            TreeValueTextBox.Clear();
        }

        // содержится ли значение
        private void ContainsValue(object sender, RoutedEventArgs e)
        {
            try
            {
                var contains = variation switch
                {
                    TreeVariations.Int => intTree.Contains(int.Parse(TreeValueTextBox.Text)),
                    TreeVariations.String => stringTree.Contains(TreeValueTextBox.Text),
                    TreeVariations.Book => bookTree.Contains(Book.Parse(TreeValueTextBox.Text)),
                    _ => throw new ArgumentOutOfRangeException()
                };

                var message = contains ? "Значение в дереве" : "Значение не в дереве";
                MessageBox.Show(message, "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (TreeException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Value type error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            TreeValueTextBox.Clear();
        }

        // сделать дерево изменяемым
        private void MakeMutable(object sender, RoutedEventArgs e)
        { 
            switch (variation)
            {
                case TreeVariations.Int:
                    if (isIntTreeImmutable == false) return;
                    intTree = ((ImmutableTree<int>)intTree).Tree;
                    isIntTreeImmutable = false;
                    break;
                case TreeVariations.String:
                    if (isStringTreeImmutable == false) return;
                    stringTree = ((ImmutableTree<string>)stringTree).Tree;
                    isStringTreeImmutable = false;
                    break;
                case TreeVariations.Book:
                    if (isBookTreeImmutable == false) return;
                    bookTree = ((ImmutableTree<Book>)bookTree).Tree;
                    isBookTreeImmutable = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // сделать дерево неизменяемым
        private void MakeImmutable(object sender, RoutedEventArgs e)
        {
            switch (variation)
            {
                case TreeVariations.Int:
                    if (isIntTreeImmutable) return;
                    intTree = new ImmutableTree<int>(intTree);
                    isIntTreeImmutable = true;
                    break;
                case TreeVariations.String:
                    if (isStringTreeImmutable) return;
                    stringTree = new ImmutableTree<string>(stringTree);
                    isStringTreeImmutable = true;
                    break;
                case TreeVariations.Book:
                    if (isBookTreeImmutable) return;
                    bookTree = new ImmutableTree<Book>(bookTree);
                    isBookTreeImmutable = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // если ли значения удовлетворяющие условию
        private void Exists(object sender, RoutedEventArgs e)
        {
            try
            {
                var exists = variation switch
                {
                    TreeVariations.Int => TreeUtils.Exists(intTree, value => value.CompareTo(int.Parse(TreeValueTextBox.Text)) > 0),
                    TreeVariations.String => TreeUtils.Exists(stringTree, value => value.CompareTo(TreeValueTextBox.Text) > 0),
                    TreeVariations.Book => TreeUtils.Exists(bookTree, value => value.CompareTo(Book.Parse(TreeValueTextBox.Text)) > 0),
                    _ => throw new ArgumentOutOfRangeException()
                };

                var message = exists ? "Хотя бы один элемент в дереве больше значения" : "Ни один элемент в дереве не больше значения";
                MessageBox.Show(message, "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (TreeException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Value type error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // все ли значения удовлетворяют условию
        private void CheckForAll(object sender, RoutedEventArgs e)
        {
            try
            {
                var exists = variation switch
                {
                    TreeVariations.Int => TreeUtils.CheckForAll(intTree, value => value.CompareTo(int.Parse(TreeValueTextBox.Text)) > 0),
                    TreeVariations.String => TreeUtils.CheckForAll(stringTree, value => value.CompareTo(TreeValueTextBox.Text) > 0),
                    TreeVariations.Book => TreeUtils.CheckForAll(bookTree, value => value.CompareTo(Book.Parse(TreeValueTextBox.Text)) > 0),
                    _ => throw new ArgumentOutOfRangeException()
                };

                var message = exists ? "Все элементы в дереве больше значения" : "Не все элементы в дереве больше значения";
                MessageBox.Show(message, "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (TreeException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Value type error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // для каждого значения выполнить действие
        private void ForEach(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (variation)
                {
                    case TreeVariations.Int:
                        TreeUtils.ForEach(intTree, value => intTree.Add(value + 100));
                        break;
                    case TreeVariations.String:
                        TreeUtils.ForEach(stringTree, value => stringTree.Add(value + 100));
                        break;
                    case TreeVariations.Book:
                        TreeUtils.ForEach(bookTree, value => bookTree.Add(new Book
                        {
                            Name = value.Name,
                            Year = value.Year + 100
                        }));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (TreeException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // найти все значения удовлетворяющие условию
        private void FindAll(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new Visualization();
                window.Show();

                switch (variation)
                {
                    case TreeVariations.Int:
                        var newIntTree = TreeUtils.FindAll(intTree, value => value.CompareTo(int.Parse(TreeValueTextBox.Text)) > 0, TreeUtils.ArrayTreeConstructor<int>());
                        if (newIntTree is ArrayTree<int>) window.DrawBPlusTree(((ArrayTree<int>)newIntTree).Root);
                        else if (newIntTree is LinkedTree<int>) window.DrawBPlusTree(((LinkedTree<int>)newIntTree).Root);
                        else window.DrawBPlusTree(((ImmutableTree<int>)newIntTree).Root);
                        break;
                    case TreeVariations.String:
                        var newStringTree = TreeUtils.FindAll(stringTree, value => value.CompareTo(TreeValueTextBox.Text) > 0, TreeUtils.LinkedTreeConstructor<string>());
                        if (newStringTree is ArrayTree<string>) window.DrawBPlusTree(((ArrayTree<string>)newStringTree).Root);
                        else if (newStringTree is LinkedTree<string>) window.DrawBPlusTree(((LinkedTree<string>)newStringTree).Root);
                        else window.DrawBPlusTree(((ImmutableTree<string>)newStringTree).Root);
                        break;
                    case TreeVariations.Book:
                        var newBookTree = TreeUtils.FindAll(bookTree, value => value.CompareTo(Book.Parse(TreeValueTextBox.Text)) > 0, TreeUtils.LinkedTreeConstructor<Book>());
                        if (newBookTree is ArrayTree<Book>) window.DrawBPlusTree(((ArrayTree<Book>)newBookTree).Root);
                        else if (newBookTree is LinkedTree<Book>) window.DrawBPlusTree(((LinkedTree<Book>)newBookTree).Root);
                        else window.DrawBPlusTree(((ImmutableTree<string>)newBookTree).Root);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (TreeException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public enum TreeVariations
    {
        Int,
        String,
        Book
    }
}
