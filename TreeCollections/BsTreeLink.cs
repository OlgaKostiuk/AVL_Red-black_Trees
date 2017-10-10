using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCollections
{
    public class BsTreeLink : ITree
    {
        private class Link
        {
            public Node node;
            public Link()
            {

            }
            public Link(Node node)
            {
                this.node = node;
            }
        }

        private class Node
        {
            public int val;
            public Link left;
            public Link right;
            public Node(int val)
            {
                this.val = val;
                left = new Link();
                right = new Link();
            }
        }

        private Link root = null;

        public void Clear()
        {
            root = new Link();
        }

        public void Add(int val)
        {
            if (root.node == null)
            {
                root.node = new Node(val);
            }                
            else
                AddNode(root, val);
        }
        private void AddNode(Link link, int val)
        {
            if (val < link.node.val)
            {
                if (link.node.left.node == null)
                    link.node.left.node = new Node(val);
                else
                    AddNode(link.node.left, val);
            }
            else if (val > link.node.val)
            {
                if (link.node.right.node == null)
                    link.node.right.node = new Node(val);
                else
                    AddNode(link.node.right, val);
            }
        }

        public bool Equal(ITree tree)
        {
            return CompareNodes(root, (tree as BsTreeLink).root);
        }

        private bool CompareNodes(Link curTree, Link tree)
        {
            if (curTree.node == null && tree.node == null)
                return true;
            if (curTree.node == null || tree.node == null)
                return false;

            bool equal = true;
            equal &= CompareNodes(curTree.node.left, tree.node.left);
            equal &= (curTree.node.val == tree.node.val);
            equal &= CompareNodes(curTree.node.right, tree.node.right);
            return equal;
        }

        public int Height()
        {
            return GetHeight(root);
        }
        private int GetHeight(Link link)
        {
            if (link.node == null)
                return 0;

            return Math.Max(GetHeight(link.node.left), GetHeight(link.node.right)) + 1;
        }

        public void Init(int[] ini)
        {
            if (ini == null)
                return;

            Clear();
            for (int i = 0; i < ini.Length; i++)
            {
                Add(ini[i]);
            }
        }

        public int Leaves()
        {
            return GetLeaves(root);
        }
        private int GetLeaves(Link link)
        {
            if (link.node == null)
                return 0;

            int leaves = 0;
            leaves += GetLeaves(link.node.left);
            if (link.node.left.node == null && link.node.right.node == null)
                leaves++;
            leaves += GetLeaves(link.node.right);
            return leaves;
        }

        public int Nodes()
        {
            return GetNodes(root);
        }
        private int GetNodes(Link link)
        {
            if (link.node == null)
                return 0;

            int nodes = 0;
            nodes += GetNodes(link.node.left);
            if (link.node.left.node != null || link.node.right.node != null)
                nodes++;
            nodes += GetNodes(link.node.right);
            return nodes;
        }

        public void Reverse()
        {
            SwapSides(root);
        }
        private void SwapSides(Link link)
        {
            if (link.node == null)
                return;

            SwapSides(link.node.left);
            Link temp = link.node.right;
            link.node.right = link.node.left;
            link.node.left = temp;
            SwapSides(link.node.left);
        }

        public int Size()
        {
            return GetSize(root);
        }
        private int GetSize(Link link)
        {
            if (link.node == null)
                return 0;

            int count = 0;
            count += GetSize(link.node.left);
            count++;
            count += GetSize(link.node.right);
            return count;
        }

        public int[] ToArray()
        {
            if (root.node == null)
                return new int[] { };

            int[] ret = new int[Size()];
            int i = 0;
            NodeToArray(root, ret, ref i);
            return ret;
        }
        private void NodeToArray(Link link, int[] ini, ref int n)
        {
            if (link.node == null)
                return;

            NodeToArray(link.node.left, ini, ref n);
            ini[n++] = link.node.val;
            NodeToArray(link.node.right, ini, ref n);

        }

        public override String ToString()
        {
            return NodeToString(root).TrimEnd(new char[] { ',', ' ' });
        }

        private String NodeToString(Link link)
        {
            if (link.node == null)
                return "";

            String str = "";
            str += NodeToString(link.node.left);
            str += link.node.val + ", ";
            str += NodeToString(link.node.right);
            return str;
        }

        public int Width()
        {
            if (root.node == null)
                return 0;

            int[] ret = new int[Height()];
            GetWidth(root, ret, 0);
            return ret.Max();
        }
        private void GetWidth(Link link, int[] levels, int level)
        {
            if (link.node == null)
                return;

            GetWidth(link.node.left, levels, level + 1);
            levels[level]++;
            GetWidth(link.node.right, levels, level + 1);
        }

        public void Del(int val)
        {
            if (root.node == null)
                throw new EmptyTreeEx();
            Link link = FindNode(root, val);
            if (link.node == null)
                throw new ValueNotFoundEx();
            Del(link);
        }
        private Link FindNode(Link link, int val)
        {
            if (link.node == null || val == link.node.val)
                return link;
            if (val < link.node.val)
                return FindNode(link.node.left, val);
            else
                return FindNode(link.node.right, val);
        }
        private void Del(Link link)
        {
            Link rmin = Min(link.node.right);
            if(rmin.node == null)
            {
                link.node = link.node.left.node;
                return;
            }
            Node p = rmin.node;
            rmin.node = rmin.node.right.node;
            p.left.node = link.node.left.node;
            p.right.node = link.node.right.node;
            link.node = p;
        }

        private Link Min(Link link)
        {
            if (link.node == null)
                return link;
            if (link.node.left.node == null)
                return link;
            return Min(link.node.left);
        }
    }
}
