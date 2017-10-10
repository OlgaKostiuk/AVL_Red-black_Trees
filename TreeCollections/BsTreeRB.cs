using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeCollections
{
    public class BsTreeRB : ITreeBalanced
    {
        protected enum Color { Red, Black };
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

        #region NodeImpl
        protected class Node
        {
            public int val;
            public Node left;
            public Node right;
            public Node parent;
            public Color color;

            public Node(int val)
            {
                this.val = val;
            }
        }
        #endregion

        #region Rotating
        private void LeftRotate(Node target)
        {
            Node right = target.right;
            target.right = right.left;

            if (right.left != null)
                right.left.parent = target;

            right.parent = target.parent;

            if (target.parent == null)
                root = right;
            else if (target == target.parent.left)
                target.parent.left = right;
            else
                target.parent.right = right;

            right.left = target;
            target.parent = right;
        }
        private void RightRotate(Node target)
        {
            Node left = target.left;
            target.left = left.right;

            if (left.right != null)
                left.right.parent = target;

            left.parent = target.parent;

            if (target.parent == null)
                root = left;
            else if (target == target.parent.left)
                target.parent.left = left;
            else
                target.parent.right = left;

            left.right = target;
            target.parent = left;
        }
        #endregion

        #region Adding
        private void BalanceInsert(Node target)
        {
            Node parent = null;
            Node granpa = null;

            while ((target != root) &&
                   (target.color != Color.Black) &&
                   (target.parent.color == Color.Red))
            {
                parent = target.parent;
                granpa = target.parent.parent;

                /*  Case : A
                    Parent of target is left child of Grand-parent of target */
                if (parent == granpa.left)
                {

                    Node uncle = granpa.right;

                    /* Case : 1
                       The uncle of target is also Color.Red
                       Only Recoloring requiColor.Red */
                    if (uncle != null && uncle.color == Color.Red)
                    {
                        granpa.color = Color.Red;
                        parent.color = Color.Black;
                        uncle.color = Color.Black;
                        target = granpa;
                    }

                    else
                    {
                        /* Case : 2
                           target is right child of its parent
                           Left-rotation requiColor.Red */
                        if (target == parent.right && target.right != null)
                        {
                            LeftRotate(target);
                            target = parent;
                            parent = target.parent;
                        }

                        /* Case : 3
                           target is left child of its parent
                           Right-rotation requiColor.Red */
                        RightRotate(granpa);
                        SwapColors(parent, granpa);
                        target = parent;
                    }
                }

                /* Case : B
                   Parent of target is right child of Grand-parent of target */
                else
                {
                    Node uncle = granpa.left;

                    /*  Case : 1
                        The uncle of target is also Color.Red
                        Only Recoloring requiColor.Red */
                    if ((uncle != null) && (uncle.color == Color.Red))
                    {
                        granpa.color = Color.Red;
                        parent.color = Color.Black;
                        uncle.color = Color.Black;
                        target = granpa;
                    }
                    else
                    {
                        /* Case : 2
                           target is left child of its parent
                           Right-rotation requiColor.Red */
                        if (target == parent.left && target.left != null)
                        {
                            RightRotate(parent);
                            target = parent;
                            parent = target.parent;
                        }

                        /* Case : 3
                           target is right child of its parent
                           Left-rotation requiColor.Red */
                        LeftRotate(granpa);
                        SwapColors(parent, granpa);
                        target = parent;
                    }
                }
            }
            root.color = Color.Black;
        }
        private void SwapColors(Node one, Node two)
        {
            Color temp = one.color;
            one.color = two.color;
            two.color = temp;
        }
        public void Add(int val)
        {
            Node node = new Node(val);
            root = InsertNode(root, node);
            BalanceInsert(node);
        }
        private Node InsertNode(Node root, Node target)
        {
            /* If the tree is empty, return a new node */
            if (root == null)
                return target;

            /* Otherwise, recur down the tree */
            if (target.val < root.val)
            {
                root.left = InsertNode(root.left, target);
                root.left.parent = root;
            }
            else if (target.val > root.val)
            {
                root.right = InsertNode(root.right, target);
                root.right.parent = root;
            }

            /* return the (unchanged) node pointer */
            return root;
        }
        #endregion

        #region Delete
        public void Del(int val)
        {
            if (root == null)
                throw new EmptyTreeEx();
            if (FindNode(root, val) == null)
                throw new ValueNotFoundEx();

            if (Size() == 1)
                root = null;
            else
                DeleteNode(FindNode(root, val));
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
        Node GetSuccessor(Node target)
        {
            if (target.right != null)
            {
                return GetMin(target.right);
            }
            Node parent = target.parent;
            while (parent != null && target == parent.right)
            {
                target = parent;
                parent = parent.parent;
            }
            return parent;
        }
        private Node GetMin(Node node)
        {
            if (node.left == null)
                return node;

            return GetMin(node.left);
        }
        Node DeleteNode(Node target)
        {
            Node temp;
            if (target.left == null || target.right == null)
                temp = target;
            else
                temp = GetSuccessor(target);

            Node cur;
            if (temp.left != null)
                cur = temp.left;
            else
                cur = temp.right;

            if (cur != null)
                cur.parent = temp.parent;

            Node parent = temp.parent;

            bool tempIsLeft = false;

            if (temp.parent == null)
            {
                root = cur;
            }
            else if (temp == temp.parent.left)
            {
                temp.parent.left = cur;
                tempIsLeft = true;
            }
            else
            {
                temp.parent.right = cur;
                tempIsLeft = false;
            }

            if (temp != target)
                target.val = temp.val;

            if (temp.color != Color.Red)
                BalanceDelete(cur, parent, tempIsLeft);

            return temp;
        }
        private void BalanceDelete(Node target, Node targetParent, bool targetIsLeft)
        {
            while (target != root && target.color == Color.Black)
            {
                Node temp;
                if (targetIsLeft == true)
                {
                    temp = targetParent.right;
                    if (temp.color == Color.Red)
                    {
                        temp.color = Color.Black;
                        targetParent.color = Color.Red;
                        LeftRotate(targetParent);
                        temp = targetParent.right;
                    }

                    if (temp.left.color == Color.Black && temp.right.color == Color.Black)
                    {
                        temp.color = Color.Red;
                        target = targetParent;
                        targetParent = target.parent;
                        targetIsLeft = (target == targetParent.left);
                    }
                    else
                    {
                        if (temp.right.color == Color.Black)
                        {
                            temp.left.color = Color.Black;
                            temp.color = Color.Red;
                            RightRotate(temp);
                            temp = targetParent.right;
                        }

                        temp.color = targetParent.color;
                        targetParent.color = Color.Black;
                        if (temp.right != null)
                        {
                            temp.right.color = Color.Black;
                        }

                        LeftRotate(targetParent);
                        target = root;
                        targetParent = null;
                    }
                }
                else
                {
                    temp = targetParent.left;
                    if (temp.color == Color.Red)
                    {
                        temp.color = Color.Black;
                        targetParent.color = Color.Red;
                        RightRotate(targetParent);
                        temp = targetParent.left;
                    }

                    if (temp.left.color == Color.Black && temp.right.color == Color.Black)
                    {
                        temp.color = Color.Red;
                        target = targetParent;
                        targetParent = target.parent;
                        targetIsLeft = (target == targetParent.left);
                    }
                    else
                    {
                        if (temp.left.color == Color.Black)
                        {
                            temp.right.color = Color.Black;
                            temp.color = Color.Red;
                            LeftRotate(temp);
                            temp = targetParent.left;
                        }

                        temp.color = targetParent.color;
                        targetParent.color = Color.Black;

                        if (temp.left != null)
                            temp.left.color = Color.Black;

                        RightRotate(targetParent);
                        target = root;
                        targetParent = null;
                    }
                }
            }
            target.color = Color.Black;
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
            return CompareNodes(root, (tree as BsTreeRB).root);
        }

        private bool CompareNodes(Node curTree, Node tree)
        {
            if (curTree == null && tree == null)
                return true;
            if (curTree == null || tree == null)
                return false;

            bool equal = true;
            equal &= CompareNodes(curTree.left, tree.left);
            equal &= (curTree.val == tree.val) & (curTree.color == tree.color);
            equal &= CompareNodes(curTree.right, tree.right);
            return equal;
        }
        #endregion

        #region Balance
        public bool IsBalanced()
        {
            return GetBalance(root, 0, 0);
        }
        private bool GetBalance(Node root, int min, int max)
        {
            // Base case
            if (root == null)
            {
                min = 0;
                max = 0;
                return true;
            }

            int lmax = 0;
            int lmin = 0;
            int rmax = 0;
            int rmin = 0;

            // Check if left subtree is balanced, also set lmxh and lmnh
            if (GetBalance(root.left, lmax, lmin) == false)
                return false;

            // Check if right subtree is balanced, also set rmxh and rmnh
            if (GetBalance(root.right, rmax, rmin) == false)
                return false;

            // Set the max and min heights of this node for the parent call
            max = Math.Max(lmax, rmax) + 1;
            min = Math.Min(lmin, rmin) + 1;

            // See if this node is balanced
            if (max <= 2 * min)
                return true;

            return false;
        }
        #endregion
    }
}
