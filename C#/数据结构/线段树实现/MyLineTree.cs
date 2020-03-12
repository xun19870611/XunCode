using System;
using System.Collections.Generic;
using System.Text;

namespace SJJG.SJJG
{
    /// <summary>
    /// 线段树
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyLineTree<T>
    {
        /// <summary>
        /// 根节点
        /// </summary>
        public MyLineTreeNode<T> Root { get; }
        /// <summary>
        /// 比较委托最小值
        /// </summary>
        public Func<T, T, bool> CompareFunction { get;}
        /// <summary>
        /// 最大的值
        /// </summary>
        private T MaxValue { get; } 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts">数组</param>
        /// <param name="func">比较两个对接的委托(最小返回TRUE)</param>
        public MyLineTree(T[] ts,Func<T, T, bool> func) 
        {
            this.CompareFunction = func;
            MaxValue = GetMaxVal(ts);
            Root = BuildTree(1, ts.Length, ts);
            Root.Reset();
        }
        /// <summary>
        /// 构造函数（默认使用HASHCODE比较）
        /// </summary>
        /// <param name="ts">数组</param>
        public MyLineTree(T[] ts)
        {
            this.CompareFunction = Compare;
            MaxValue = GetMaxVal(ts);
            Root = BuildTree(1, ts.Length, ts);
            Root.Reset();
        }
        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        protected virtual bool Compare(T t1,T t2)
        {
            return t1.GetHashCode() < t2.GetHashCode();
        }

        /// <summary>
        /// 构造树
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private MyLineTreeNode<T> BuildTree(int start, int end, T[] ts)
        {
            MyLineTreeNode<T> node = new MyLineTreeNode<T>(start, end, this.MaxValue, CompareFunction);
            if (start < end) {
                int mid = (start + end) / 2;
                MyLineTreeNode<T> left = BuildTree(start, mid, ts);
                left.Parent = node;
                node.Left = left;
                MyLineTreeNode<T> right = BuildTree(mid+1, end, ts);
                right.Parent = node;
                node.Right = right;
            }
            if (start == end)
            {
                node.Left = null;
                node.Right = null;
                node.SavedVal = ts[start - 1];
            }
            return node;    
        }
        private T GetMaxVal(T[] ts) {
            if (ts != null && ts.Length > 0) {
                T maxVal = ts[0];
                for (int i = 0; i < ts.Length; i++)
                {
                    if (i == 0) continue;
                    if (CompareFunction(maxVal, ts[i]))
                    {
                        maxVal = ts[i];
                    }
                }
                return maxVal;
            }
            return default(T);
        }
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public T Search(int left, int right)
        {
            return Root.Search(this.Root,left, right);
        }
        /// <summary>
        /// 更改节点值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="weight"></param>
        public void Change(int index, T val)
        {
            MyLineTreeNode<T> parent = Root;
            //通过循环找到索引所在的节点
            do
            {
                int mid = (parent.StartIndex + parent.EndIndex) / 2;
                if (index <= mid) { 
                    parent = parent.Left; 
                }
                else { 
                    parent = parent.Right; 
                }
            } while (parent.StartIndex != index || parent.EndIndex != index);
            //更改所在节点的值
            parent.SavedVal = val;

            //更改所在节点的值之后，需要调整所有父节点的值（通过不断和右边节点比较）
            while (parent.Parent != null)
            {
                MyLineTreeNode<T> temp = parent.Parent;
                temp.SavedVal = CompareFunction.Invoke(temp.Left.SavedVal, temp.Right.SavedVal) ? temp.Left.SavedVal : temp.Right.SavedVal;
                parent = temp;
            }
        }
    }
    public class MyLineTreeNode<T>
    {
        /// <summary>
        /// 父节点
        /// </summary>
        public MyLineTreeNode<T> Parent { get; set; }
        /// <summary>
        /// 左边节点
        /// </summary>
        public MyLineTreeNode<T> Left { get; set; }
        /// <summary>
        /// 右边节点
        /// </summary>
        public MyLineTreeNode<T> Right { get; set; }
        /// <summary>
        /// 开始索引
        /// </summary>
        public int StartIndex { get; }
        /// <summary>
        /// 结束索引
        /// </summary>
        public int EndIndex { get; }
        /// <summary>
        /// 存储的值
        /// </summary>
        public T SavedVal { get; set; }

        private Func<T, T, bool> CompareFunction { get; }

        public MyLineTreeNode(int startindex, int endindex, T val, Func<T, T, bool> func) 
        {
            this.StartIndex = startindex;
            this.EndIndex = endindex;
            this.SavedVal = val;
            this.CompareFunction = func;
        }

        public T Reset()
        {
            T result = this.SavedVal;
            
            if (this.Left!=null) {
                T tmp = Left.Reset();
                if (CompareFunction.Invoke(tmp, result)) {
                    result = tmp;
                }
            }
            if (this.Right != null)
            {
                T tmp = Right.Reset();
                if (CompareFunction.Invoke(tmp,result))
                {
                    result = tmp;
                }
            }
            this.SavedVal = result;
            return result;
        }
        public T Search(MyLineTreeNode<T> parent, int left, int right)
        {
            //如果完全相等，则直接返回保存的值
            if (left == parent.StartIndex && right == parent.EndIndex)
            {
                return parent.SavedVal;
            }
            //中间索引
            int mid = (parent.StartIndex + parent.EndIndex) / 2;
            //如果要找的结束范围小于中间索引，在直接在节点左边树找
            if (right <= mid)
            {
                return Search(parent.Left, left, right);
            }
            //如果要找的开始范围大于中间索引，在直接在节点右边树找
            else if (left > mid)
            {
                return Search(parent.Right, left, right);
            }
            else
            {
                //否则两边都找
                T leftObj = Search(parent.Left, left, mid);
                T rightObj = Search(parent.Right, mid + 1, right);
                return CompareFunction.Invoke(leftObj,rightObj) ? leftObj : rightObj;
            }
        }
    }
}
