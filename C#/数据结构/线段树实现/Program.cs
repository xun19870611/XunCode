using SJJG.SJJG;
using System;

namespace SJJG
{
    class Program
    {
        static void Main(string[] args)
        {
            
            int sMin = 3;
            int sMax = 7;
            Console.WriteLine("20, 2, 33, 14, 5, 46, 5, 18, 99, 100");
            {
                int[] weights = new int[10] { 20, 2, 33, 14, 5, 46, 5, 18, 99, 100 };
                TreeLineStrunct t = new TreeLineStrunct(weights);
                Console.WriteLine("--------------TreeLineStrunct------------");
                //Print(t.Root, "root");
                Console.WriteLine($"TreeLineStrunct 查找[{sMin}-{sMax}]的值为{ t.Search(sMin, sMax)}");
                t.Change(3, 2);
                Console.WriteLine($"TreeLineStrunct 改变后查找[{sMin}-{sMax}]的值为{t.Search(sMin, sMax)}");
            }

            {
                int[] weights2 = new int[10] { 20, 2, 33, 14, 5, 46, 5, 18, 99, 100 };
                MyLineTree<int> myTree = new MyLineTree<int>(weights2, (x1, x2) => { return x1 < x2; });
                Console.WriteLine("--------------MyLineTree------------");
                //Print2(myTree.Root, "root");
                Console.WriteLine($"MyLineTree 查找[{sMin}-{sMax}]的值为{myTree.Search(sMin, sMax)}");
                myTree.Change(3, 2);
                Console.WriteLine($"MyLineTree 改变后查找[{sMin}-{sMax}]的值为{myTree.Search(sMin, sMax)}");

                Print2(myTree.Root, "root");
            }
            Console.ReadLine();
        }
        public static void Print(TreeLineStrunct.Node node,string leftOrright) {
            if (node != null)
            {
                Console.WriteLine($"{leftOrright}--StartIndex：{node.StartIndex},EndIndex：{node.EndIndex},Weight：{node.Weight}");
                if (node.Left != null) {
                    Print(node.Left,"left");
                }
                if (node.Right != null)
                {
                    Print(node.Right,"right");
                }
                
            }
        }
        public static void Print2<T>(MyLineTreeNode<T> node, string leftOrright)
        {
            if (node != null)
            {
                Console.WriteLine($"{leftOrright}--StartIndex：{node.StartIndex},EndIndex：{node.EndIndex}," +
                    $"Weight：{ node.SavedVal.ToString() }");
                if (node.Left != null)
                {
                    Print2(node.Left, "left");
                }
                if (node.Right != null)
                {
                    Print2(node.Right, "right");
                }

            }
        }
    }

}
