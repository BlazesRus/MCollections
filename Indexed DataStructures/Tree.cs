﻿using System;
using System.Collections.Generic;

namespace Indexed_DataStructures
{
    internal sealed class Tree<T>
    {
        Node<T> root = NIL<T>.Instance;
        private IComparer<T> comparer;
        private int count;

        public Tree(IComparer<T> comparer)
        {
            this.comparer = comparer;
        }

        public int Count => this.root.Count;

        public bool AddIfNotPresent(T item)
        {
            var y = NIL<T>.Instance;
            var x = this.root;
            Node<T> z = new Node<T>(item);
            z.Left = NIL<T>.Instance;
            z.Right = NIL<T>.Instance;

            int c;
            while (!x.IsNil())
            {
                y = x;
                c = this.comparer.Compare(z.Item, x.Item);
                if (c == 0)
                {
                    return false;
                }
                else if (c < 0)
                {
                    x = x.Left;
                }
                else
                {
                    x = x.Right;
                }
            }
            z.Parent = y;
            if (y.IsNil())
            {
                this.root = z;
            }
            c = this.comparer.Compare(z.Item, y.Item);
            if (c < 0)
            {
                y.Left = z;
            }
            else
            {
                y.Right = z;
            }
            z.Left = NIL<T>.Instance;
            z.Right = NIL<T>.Instance;
            z.MarkRed();
            var temp = z;
            while (!temp.IsNil())
            {
                temp.Count++;
                temp = temp.Parent;
            }
            Balance(z);
            return true;
        }

        internal T GetNthItem(int index)
        {
            Node<T> node = this.root;
            while (true)
            {
                if (index < node.Left.Count)
                {
                    node = node.Left;
                }
                else if (index > node.Left.Count)
                {
                    index = index - (node.Left.Count + 1);
                    node = node.Right;
                }
                else
                {
                    return node.Item;
                }
            }
        }

        private void Balance(Node<T> z)
        {
            Node<T> y;
            while (z.Parent.IsRed())
            {
                if (z.Parent == z.Parent.Parent.Left)
                {
                    y = z.Parent.Parent.Right;
                    if (y.IsRed())
                    {
                        z.Parent.MarkBlack();
                        y.MarkBlack();
                        z.Parent.Parent.MarkRed();
                        z = z.Parent.Parent;
                    }
                    else
                    {
                        if (z == z.Parent.Right)
                        {
                            z = z.Parent;
                            LeftRotate(z);
                        }
                        z.Parent.MarkBlack();
                        z.Parent.Parent.MarkRed();
                        RightRotate(z.Parent.Parent);
                    }
                }
                else
                {
                    y = z.Parent.Parent.Left;
                    if (y.IsRed())
                    {
                        z.Parent.MarkBlack();
                        y.MarkBlack();
                        z.Parent.Parent.MarkRed();
                        z = z.Parent.Parent;
                    }
                    else
                    {
                        if (z == z.Parent.Left)
                        {
                            z = z.Parent;
                            RightRotate(z);
                        }
                        z.Parent.MarkBlack();
                        z.Parent.Parent.MarkRed();
                        LeftRotate(z.Parent.Parent);
                    }
                }
            }
            this.root.MarkBlack();
        }

        private void LeftRotate(Node<T> x)
        {
            Node<T> y = x.Right;
            x.Right = y.Left;
            if(!y.Left.IsNil())
            {
                y.Left.Parent = x;
            }
            y.Parent = x.Parent;
            if (x.Parent.IsNil())
            {
                this.root = y;
            }
            else if (x == x.Parent.Left)
            {
                x.Parent.Left = y;
            }
            else
            {
                x.Parent.Right = y;
            }
            y.Left = x;
            x.Parent = y;

            y.Count = x.Count;
            x.Count = x.Left.Count + x.Right.Count + 1;
        }

        private void RightRotate(Node<T> x)
        {
            Node<T> y = x.Left;
            x.Left = y.Right;
            if (!y.Right.IsNil())
            {
                y.Right.Parent = x;
            }
            y.Parent = x.Parent;
            if (x.Parent.IsNil())
            {
                this.root = y;
            }
            else if (x == x.Parent.Right)
            {
                x.Parent.Right = y;
            }
            else
            {
                x.Parent.Left = y;
            }
            y.Right = x;
            x.Parent = y;

            y.Count = x.Count;
            x.Count = x.Left.Count + x.Right.Count + 1;
        }

        public IEnumerator<T> DFS()
        {
            List<T> list = new List<T>();
            this.DFS(this.root, list);
            return list.GetEnumerator();
        }

        private void DFS(Node<T> node, List<T> list)
        {
            if (node == null)
            {
                return;
            }
            DFS(node.Left, list);
            list.Add(node.Item);
            DFS(node.Right, list);
        }
    }
}
