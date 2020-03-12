using System;
using System.Collections.Generic;
using System.Text;

namespace SJJG.SJJG
{
    /// <summary>
    /// 线段树
    /// </summary>
    public class TreeLineStrunct
    {
        public class Node
        {
            public Node(int sidx, int eidx, int weight)
            {
                this.sidx = sidx;
                this.eidx = eidx;
                this.weight = weight;
            }

            int weight;
            int sidx;
            int eidx;

            Node parent;
            Node left;
            Node right;

            public int Weight { get { return weight; } set { weight = value; } }
            public int StartIndex { get { return sidx; } }
            public int EndIndex { get { return eidx; } }
            public Node Parent { get { return parent; } set { parent = value; } }
            public Node Left { get { return left; } set { left = value; } }
            public Node Right { get { return right; } set { right = value; } }
            public int Reset()
            {
                int result = this.weight;
                if (left != null)
                {
                    int temp = left.Reset();
                    if (temp < result) result = temp;
                }
                if (right != null)
                {
                    int temp = right.Reset();
                    if (temp < result) result = temp;
                }
                this.weight = result;
                return result;
            }
        }

        public TreeLineStrunct(int[] weights)
        {
            root = buildTree(1, weights.Length, weights);
            root.Reset();
        }

        private Node buildTree(int start, int end, int[] weights)
        {
            Node node = new Node(start, end, int.MaxValue);
            if (start < end)
            {
                int mid = (start + end) / 2;
                Node left = buildTree(start, mid, weights);
                left.Parent = node;
                node.Left = left;
                Node right = buildTree(mid + 1, end, weights);
                right.Parent = node;
                node.Right = right;
            }
            if (start == end)
            {
                node.Left = null;
                node.Right = null;
                node.Weight = weights[start - 1];
            }
            return node;
        }

        Node root;
        public Node Root { get { return root; } }

        public void Change(int index, int weight)
        {
            Node parent = root;
            do
            {
                int mid = (parent.StartIndex + parent.EndIndex) / 2;
                if (index <= mid) parent = parent.Left;
                else parent = parent.Right;
            } while (parent.StartIndex != index || parent.EndIndex != index);
            parent.Weight = weight;
            while (parent.Parent != null)
            {
                Node temp = parent.Parent;
                temp.Weight = temp.Left.Weight > temp.Right.Weight ? temp.Right.Weight : temp.Left.Weight;
                parent = temp;
            }
        }

        public int Search(int left, int right)
        {
            return Search(root, left, right);
        }
        private int Search(Node parent, int left, int right)
        {
            if (left == parent.StartIndex && right == parent.EndIndex)
            {
                return parent.Weight;
            }
            int mid = (parent.StartIndex + parent.EndIndex) / 2;
            if (right <= mid)
            {
                return Search(parent.Left, left, right);
            }
            else if (left > mid)
            {
                return Search(parent.Right, left, right);
            }
            else
            {
                int lw = Search(parent.Left, left, mid);
                int rw = Search(parent.Right, mid + 1, right);
                return lw > rw ? rw : lw;
            }
        }
    }
}
