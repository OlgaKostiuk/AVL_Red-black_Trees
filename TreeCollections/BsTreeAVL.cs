using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCollections
{
    public class Node
    {
        public int val;
        public Node left;
        public Node right;
        public Node(int val)
        {
            this.val = val;
        }
    }
    public class BsTreeAVL : ITreeBalanced
    {
        Node root = null;
        public Node Root { get => root; }
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
        #region Rotate
        private int balance_factor(Node node)
        {
            int l = GetHeight(node.left);
            int r = GetHeight(node.right);
            int b_factor = l - r;
            return b_factor;
        }
        private Node RotateRR(Node parent)
        {
            Node pivot = parent.right;
            parent.right = pivot.left;
            pivot.left = parent;
            return pivot;
        }
        private Node RotateLL(Node parent)
        {
            Node pivot = parent.left;
            parent.left = pivot.right;
            pivot.right = parent;
            return pivot;
        }
        private Node RotateLR(Node parent)
        {
            Node pivot = parent.left;
            parent.left = RotateRR(pivot);
            return RotateLL(parent);
        }
        private Node RotateRL(Node parent)
        {
            Node pivot = parent.right;
            parent.right = RotateLL(pivot);
            return RotateRR(parent);
        }
        #endregion

        #region Add
        public void Add(int val)
        {
            if (root == null)
                root = new Node(val);
            else
                AddNode(ref root, val);
        }
        private void AddNode(ref Node node, int val)
        {
            if (node == null)
            {
                node = new Node(val);
                return;
            }

            if (val < node.val)
            {
                AddNode(ref node.left, val);
                node = BalanceTree(node);
            }
            else if (val > node.val)
            {
                AddNode(ref node.right, val);
                node = BalanceTree(node);
            }
        }
        private Node BalanceTree(Node node)
        {
            int b_factor = balance_factor(node);
            if (b_factor > 1)
            {
                if (balance_factor(node.left) > 0)
                {
                    node = RotateLL(node);
                }
                else
                {
                    node = RotateLR(node);
                }
            }
            else if (b_factor < -1)
            {
                if (balance_factor(node.right) > 0)
                {
                    node = RotateRL(node);
                }
                else
                {
                    node = RotateRR(node);
                }
            }
            return node;
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

            DeleteNode(ref root, val);
            if (root != null)
                root = BalanceTree(root);
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
        private void DeleteNode(ref Node node, int val)
        {
            if (node == null)
                return;
            if (val < node.val)
            {
                DeleteNode(ref node.left, val);
            }
            else if (val > node.val)
            {
                DeleteNode(ref node.right, val);
            }
            else if (node.left != null && node.right != null)
            {
                node.val = Min(node.right).val;
                DeleteNode(ref node.right, node.val);
            }
            else
            {
                if (node.left != null)
                    node = node.left;
                else
                    node = node.right;
            }
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
            return GetHeight(root);
        }
        private int GetHeight(Node node)
        {
            if (node == null)
                return 0;

            return Math.Max(GetHeight(node.left), GetHeight(node.right)) + 1;
        }
        #endregion

        #region Width
        public int Width()
        {
            if (root == null)
                return 0;

            int[] ret = new int[Height()];
            GetWidth(root, ret, 0);
            return ret.Max();
        }
        private void GetWidth(Node node, int[] levels, int level)
        {
            if (node == null)
                return;

            GetWidth(node.left, levels, level + 1);
            levels[level]++;
            GetWidth(node.right, levels, level + 1);
        }
        #endregion

        #region Leaves
        public int Leaves()
        {
            return GetLeaves(root);
        }
        private int GetLeaves(Node node)
        {
            if (node == null)
                return 0;

            int leaves = 0;
            leaves += GetLeaves(node.left);
            if (node.left == null && node.right == null)
                leaves++;
            leaves += GetLeaves(node.right);
            return leaves;
        }
        #endregion

        #region Nodes
        public int Nodes()
        {
            return GetNodes(root);
        }
        private int GetNodes(Node node)
        {
            if (node == null)
                return 0;

            int nodes = 0;
            nodes += GetNodes(node.left);
            if (node.left != null || node.right != null)
                nodes++;
            nodes += GetNodes(node.right);
            return nodes;
        }
        #endregion

        #region Reverse
        public void Reverse()
        {
            SwapSides(root);
        }
        private void SwapSides(Node node)
        {
            if (node == null)
                return;

            SwapSides(node.left);
            Node temp = node.right;
            node.right = node.left;
            node.left = temp;
            SwapSides(node.left);
        }
        #endregion

        #region Size
        public int Size()
        {
            return GetSize(root);
        }
        private int GetSize(Node node)
        {
            if (node == null)
                return 0;

            int count = 0;
            count += GetSize(node.left);
            count++;
            count += GetSize(node.right);
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
            NodeToArray(root, ret, ref i);
            return ret;


        }
        private void NodeToArray(Node node, int[] ini, ref int n)
        {
            if (node == null)
                return;

            NodeToArray(node.left, ini, ref n);
            ini[n++] = node.val;
            NodeToArray(node.right, ini, ref n);

        }
        #endregion

        #region ToString
        public override String ToString()
        {
            return NodeToString(root).TrimEnd(new char[] { ',', ' ' });
        }

        private String NodeToString(Node node)
        {
            if (node == null)
                return "";

            String str = "";
            str += NodeToString(node.left);
            str += node.val + ", ";
            str += NodeToString(node.right);
            return str;
        }

        public void Clear()
        {
            root = null;
        }
        #endregion

        #region Equal
        public bool Equal(ITree tree)
        {
            return CompareNodes(root, (tree as BsTreeAVL).root);
        }

        private bool CompareNodes(Node curTree, Node tree)
        {
            if (GetHeight(curTree) == GetHeight(tree))
                return true;
            else return false;
        }
        #endregion

        #region Balance
        public bool IsBalanced()
        {
            return GetBalance(root);
        }

        private bool GetBalance(Node node)
        {
            int lh; /* for height of left subtree */

            int rh; /* for height of right subtree */

            /* If tree is empty then return true */
            if (node == null)
                return true;

            /* Get the height of left and right sub trees */
            lh = GetHeight(node.left);
            rh = GetHeight(node.right);

            if (Math.Abs(lh - rh) <= 1
                    && GetBalance(node.left)
                    && GetBalance(node.right))
                return true;

            /* If we reach here then tree is not height-balanced */
            return false;
        }
        #endregion
    }
}
