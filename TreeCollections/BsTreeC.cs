using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCollections
{
    public class BsTreeC : ITree
    {

        protected class Node
        {
            public int val;
            public Node left;
            public Node right;
            public Node(int val)
            {
                this.val = val;
            }
        }

        protected Node root = null;

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

        #region Add
        public void Add(int val)
        {
            if (root == null)
            {
                root = new Node(val);
                return;
            }

            Node temp = root;
            Queue<Node> q = new Queue<Node>();
            q.Enqueue(temp);

            // Do level order traversal until we find
            // an empty place. 
            while (q.Count != 0)
            {
                temp = q.Dequeue();

                if (val < temp.val)
                {
                    if (temp.left == null)
                    {
                        temp.left = new Node(val);
                        break;
                    }
                    else
                        q.Enqueue(temp.left);
                }
                else if (temp.right == null)
                {
                    temp.right = new Node(val);
                    break;
                }
                else
                    q.Enqueue(temp.right);
            }
        }
        #endregion

        #region Del
        public void Del(int val)
        {
            if (root == null)
                throw new EmptyTreeEx();
            if (FindNode(root, val) == null)
                throw new ValueNotFoundEx();
            if (Size() == 1)
                root = null;

            DeleteNode(root, val);
        }
        private Node FindNode(Node node, int val)
        {
            if (node == null || val == node.val)
                return node;
            if (val < node.val)
                return FindNode(node.left, val);
            else
                return FindNode(node.right, val);
        }
        private Node DeleteNode(Node node, int val)
        {
            if (node == null)
                return node;

            if (val < node.val)
            {
                node.left = DeleteNode(node.left, val);
            }
            else if (val > node.val)
            {
                node.right = DeleteNode(node.right, val);
            }
            else if (node.left != null && node.right != null)
            {
                node.val = Min(node.right).val;
                node.right = DeleteNode(node.right, node.val);
            }
            else
            {
                if (node.left != null)
                    node = node.left;
                else
                    node = node.right;
            }
            return node;
        }
        private Node Min(Node node)
        {
            if (node.left == null)
                return node;

            return Min(node.left);
        }
        #endregion

        #region Height
        public int Height()
        {
            Node node = root;
            // Base Case
            if (node == null)
                return 0;

            // Create an empty queue for level order tarversal
            Queue<Node> q = new Queue<Node>();

            // Enqueue Root and initialize height
            q.Enqueue(node);
            int height = 0;

            while (true)
            {
                // nodeCount (queue size) indicates number of nodes
                // at current lelvel.
                int nodeCount = q.Count;
                if (nodeCount == 0)
                    return height;
                height++;

                // Dequeue all nodes of current level and Enqueue all
                // nodes of next level
                while (nodeCount > 0)
                {
                    Node newnode = q.Peek();
                    q.Dequeue();
                    if (newnode.left != null)
                        q.Enqueue(newnode.left);
                    if (newnode.right != null)
                        q.Enqueue(newnode.right);
                    nodeCount--;
                }
            }
        }
        #endregion

        #region Width
        public int Width()
        {
            if (root == null)
                return 0;

            int maxwidth = 0;

            Queue<Node> q = new Queue<Node>();
            q.Enqueue(root);
            while (q.Count != 0)
            {
                int count = q.Count;
                maxwidth = Math.Max(maxwidth, count);
                while (count-- > 0)
                {
                    Node temp = q.Dequeue();

                    if (temp.left != null)
                        q.Enqueue(temp.left);

                    if (temp.right != null)
                        q.Enqueue(temp.right);
                }
            }
            return maxwidth;
        }
        #endregion

        #region Leaves
        public int Leaves()
        {
            if (root == null)
                return 0;
            Node p = root;
            int leaves = 0;
            Stack<Node> stack = new Stack<Node>();

            while (p != null)
            {
                stack.Push(p);
                p = p.left;
            }
            while (stack.Count > 0)
            {
                p = stack.Pop();
                if (p.left == null && p.right == null)
                    leaves++;
                if (p.right != null)
                {
                    p = p.right;
                    while (p != null)
                    {
                        stack.Push(p);
                        p = p.left;
                    }
                }
            }
            return leaves;
        }
        #endregion

        #region Nodes
        public int Nodes()
        {
            if (root == null)
                return 0;
            Node p = root;
            int nodes = 0;
            Stack<Node> stack = new Stack<Node>();

            while (p != null)
            {
                stack.Push(p);
                p = p.left;
            }
            while (stack.Count > 0)
            {
                p = stack.Pop();
                if (p.left != null || p.right != null)
                    nodes++;
                if (p.right != null)
                {
                    p = p.right;
                    while (p != null)
                    {
                        stack.Push(p);
                        p = p.left;
                    }
                }
            }
            return nodes;
        }
        #endregion

        #region Reverse
        public void Reverse()
        {
            Node cur = root;
            Stack<Node> stack = new Stack<Node>();
            bool done = false;

            while (!done)
            {
                if (cur != null)
                {
                    stack.Push(cur);
                    cur = cur.left;
                }
                else
                {
                    if (stack.Count != 0)
                    {
                        cur = stack.Pop();
                        Node temp = cur.left;
                        cur.left = cur.right;
                        cur.right = temp;
                        cur = cur.left;
                    }
                    else
                    {
                        done = true;
                    }
                }
            }
        }
        #endregion

        #region Size
        public int Size()
        {
            if (root == null)
                return 0;
            Node p = root;
            int count = 0;
            Stack<Node> stack = new Stack<Node>();

            while (p != null)
            {
                stack.Push(p);
                p = p.left;
            }
            while (stack.Count > 0)
            {
                p = stack.Pop();
                count++;
                if (p.right != null)
                {
                    p = p.right;
                    while (p != null)
                    {
                        stack.Push(p);
                        p = p.left;
                    }
                }
            }
            return count;
        }

        #endregion

        #region ToArray
        public int[] ToArray()
        {
            if (root == null)
                return new int[] { };

            int[] ret = new int[Size()];
            int i = 0;
            Node p = root;
            Stack<Node> stack = new Stack<Node>();

            while (p != null)
            {
                stack.Push(p);
                p = p.left;
            }
            while (stack.Count > 0)
            {
                p = stack.Pop();
                ret[i++] = p.val;
                if (p.right != null)
                {
                    p = p.right;
                    while (p != null)
                    {
                        stack.Push(p);
                        p = p.left;
                    }
                }
            }
            return ret;
        }
        #endregion

        #region ToString
        public override String ToString()
        {
            if (root == null)
                return "";
            Node p = root;
            String str = "";
            Stack<Node> stack = new Stack<Node>();

            while (p != null)
            {
                stack.Push(p);
                p = p.left;
            }
            while (stack.Count > 0)
            {
                p = stack.Pop();
                str += p.val + ", ";
                if (p.right != null)
                {
                    p = p.right;
                    while (p != null)
                    {
                        stack.Push(p);
                        p = p.left;
                    }
                }
            }
            return str.TrimEnd(',', ' ');
        }
        public void Clear()
        {
            root = null;
        }
        #endregion

        #region Equal
        public bool Equal(ITree tree)
        {
            Node root1 = root;
            Node root2 = (tree as BsTreeC).root;
            // Return true if both trees are empty
            if (root1 == null && root2 == null)
                return true;

            // Return false if one is empty and other is not
            if (root1 == null || root2 == null)
                return false;

            // Create an empty queues for simultaneous traversals 
            Queue<Node> q1 = new Queue<Node>();
            Queue<Node> q2 = new Queue<Node>();

            // Enqueue Roots of trees in respective queues
            q1.Enqueue(root1);
            q2.Enqueue(root2);

            while (q1.Count != 0 && q2.Count != 0)
            {
                // Get front nodes and compare them
                Node n1 = q1.Dequeue();
                Node n2 = q2.Dequeue();

                if (n1.val != n2.val)
                    return false;

                /* Enqueue left children of both nodes */
                if (n1.left != null && n2.left != null)
                {
                    q1.Enqueue(n1.left);
                    q2.Enqueue(n2.left);
                }
                // If one left child is empty and other is not
                else if (n1.left != null || n2.left != null)
                    return false;

                // Right child code (Similar to left child code)
                if (n1.right != null && n2.right != null)
                {
                    q1.Enqueue(n1.right);
                    q2.Enqueue(n2.right);
                }
                else if (n1.right != null || n2.right != null)
                    return false;
            }

            return true;
        }
        #endregion
    }
}
