using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        Node root;

        public void Add(T newItem)
        {
            if (root == null)
            {
                root = new Node(newItem);
                return;
            }

            Node tmp = root;
            Node parent = root;

            while (tmp != null)
            {
                parent = tmp;
                if (newItem.CompareTo(tmp.data) < 0) tmp = tmp.left;
                else tmp = tmp.right;
            }

            Node n = new Node(newItem);
            if (newItem.CompareTo(parent.data) < 0) parent.left = n;
            else parent.right = n;

        }

        public bool AddAndSearch(T itemToSearch, out T foundItem)
        {
            foundItem=default;

            if (root == null)
            {
                root = new Node(itemToSearch);
                return false;
            }

            Node tmp = root;
            Node parent = root;

            while (tmp != null)
            {
                parent = tmp;
                if (itemToSearch.CompareTo(tmp.data) == 0)
                {
                    foundItem = tmp.data;
                    return true;
                }
                else if (itemToSearch.CompareTo(tmp.data) < 0) tmp = tmp.left;
                else tmp = tmp.right;
            }

            Node n = new Node(itemToSearch);
            if (itemToSearch.CompareTo(parent.data) < 0) parent.left = n;
            else parent.right = n;
            return false;
        }
    
        public void ScanInOrder(Action<T> action)
        {
            ScanInOrder(root, action);
        }

        private void ScanInOrder(Node tmpRoot, Action<T> action)
        {
            if (tmpRoot == null) return;

            ScanInOrder(tmpRoot.left, action);
            action(tmpRoot.data);
            ScanInOrder(tmpRoot.right, action);
        }

        public bool Search(T itemToSearch, out T foundItem)
        {
            Node temp = root;
            while (temp != null)
            {
                if (itemToSearch.CompareTo(temp.data) == 0)
                {
                    foundItem = temp.data;
                    return true;
                }
                else if (itemToSearch.CompareTo(temp.data) < 0) temp = temp.left;
                else temp = temp.right;
            }
            foundItem = default;
            return false;
        }

        public bool SearchNearest(T itemToSearch, out T nearestItem)
        {
            Node temp = root;
            nearestItem = default;

            while (temp != null)
            {
                if (itemToSearch.CompareTo(temp.data) == 0)
                {
                    nearestItem = temp.data;
                    return true;
                }
                else if (itemToSearch.CompareTo(temp.data) < 0)
                {
                    nearestItem = temp.data;
                    temp = temp.left;
                }
                else
                {
                    temp = temp.right;
                }
            }
            //when itemToSearch is larger than all the items in the tree
            if (nearestItem==null)
            {
                //nearestItem = default;
                return false;
            }
            else return true;
        }

        public int GetDepth()
        {
            return GetDepth(root);
        }

        internal bool IsTreeEmpty() => (root == null);

        public bool Remove(T value) //O(log(n))
        {
            Node parent = null;
            Node tmpNode = root;

            if (root == null) return false; //Empty tree
   
            while (tmpNode != null)
            {
                if (value.CompareTo(tmpNode.data) == 0) break;
                parent = tmpNode;
                if (value.CompareTo(tmpNode.data) < 0) tmpNode = tmpNode.left;
                else tmpNode = tmpNode.right;
            }

            if (tmpNode == null) return false; // value not exist

            if (tmpNode.left == null && tmpNode.right == null) 
                RemoveLeaf(tmpNode, parent); //if tmpNode is a leaf

            else if (tmpNode.left == null && tmpNode.right != null) 
                RemoveSingleChild(tmpNode, tmpNode.right, parent); //tmpNode has single right child  

            else if (tmpNode.left != null && tmpNode.right == null)
                RemoveSingleChild(tmpNode, tmpNode.left, parent); //tmpNode has single left child 

            else //tmpNode has two children
            {
                Node UpdateNode = tmpNode;
                Node nodeToDelete = tmpNode.right; //starting with right branch
                UpdateNode.data = GetLeftiestLeaf(ref nodeToDelete, ref tmpNode); // move to the leftiest leaf.
                                                                                      //currentRoot will be the parent of the NodeToDelete
                if (nodeToDelete.right == null) RemoveLeaf(nodeToDelete, tmpNode); //if leftiest node is a leaf
                else RemoveSingleChild(nodeToDelete, nodeToDelete.right, tmpNode); //if nodeToDelete has a right branches            
            }
            return true;
        }

        private void RemoveLeaf(Node leaf, Node parent)
        {
            if (parent == null) //tree with only one elemnt -private case 
            {
                root = null;
                return;
            }
            if (parent.right == leaf) parent.right = null;
            else parent.left = null;
        }

        private void RemoveSingleChild(Node RootToDelete, Node next, Node parent)
        {
            if (parent == null) //The root is the main root
            {
                root = next;
                return;
            }
            if (parent.right == RootToDelete) parent.right = next;
            else parent.left = next;
        }

        private T GetLeftiestLeaf(ref Node startRoot, ref Node parent)
        {
            while (startRoot.left != null)
            {
                parent = startRoot;
                startRoot = startRoot.left;
            }
            return startRoot.data; 
        }

        private int GetDepth(Node tmpRoot)
        {
            if (root == null) return 0;

            int leftDepth = GetDepth(tmpRoot.left);
            int rightDepth = GetDepth(tmpRoot.right);

            return Math.Max(leftDepth, rightDepth) + 1;
        }
        class Node
        {
            public Node right;
            public Node left;
            public T data;

            public Node(T data)
            {
                this.data = data;
            }
        }

    }
}
