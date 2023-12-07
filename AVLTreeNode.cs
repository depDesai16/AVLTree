internal class AvlTreeNode<T> where T : IComparable<T>
    {
        internal T Data { get; set; }
        internal AvlTreeNode<T>? Left { get; set; }
        internal AvlTreeNode<T>? Right { get; set; }
        internal int Height { get; set; }

        internal AvlTreeNode(T data)
        {
            this.Data = data;
            this.Height = 0;
        }

        internal void UpdateHeight()
        {
            // Recall that the height of a node in a tree 
            // is exactly one higher than its highest child node.
            int leftChildHeight = this.Left?.Height ?? -1;
            int rightchildHeight = this.Right?.Height ?? -1;
            int highestChild = Math.Max(leftChildHeight, rightchildHeight);
            this.Height = highestChild + 1;
        }

        internal int GetBalanceFactor()
        {
            int rightHeight = 0;
            int leftHeight = 0;
            if(this.Right != null)
                rightHeight = this.Right.Height;
            if(this.Left != null)
                leftHeight = this.Left.Height;
            int BalanceFactor = rightHeight - leftHeight;
            return BalanceFactor;
        }

        private AvlTreeNode<T> BSTAdd(AvlTreeNode<T> root, T data) 
        {
            if (root == null)
            {
                root = new AvlTreeNode<T>(data);
            }
            else if (data.CompareTo(root.Data) < 0)
            {
                root.Left = Add(root.Left, data);
            }
            else if (data.CompareTo(root.Data) > 0)
            {
                root.Right = Add(root.Right, data);
            }
            return root;
        }

        public AvlTreeNode<T>? BSTRemove(AvlTreeNode<T> root, T data) 
        {
            // Is the root node null?
            // we've reached the bottom of the tree
            if (root == null) return null;
            
            if (data.CompareTo(root.Data) < 0)
            {
                // Take a left
                root.Left = Remove(root.Left, data);
            }
            else if (data.CompareTo(root.Data) > 0)
            {
                // Take a right
                root.Right = Remove(root.Right, data);
            }
            // We found the data
            else
            {
                // Determine which of the 3 cases has occurred
                // leaf node
                if (root.Left == null && root.Right == null)
                {
                    root = null;
                }
                // Has right child
                // OR Has two children
                else if (root.Right != null)
                {
                    // Deal with the child on the right
                    var successor = SuccessorSearch(root);
                    root.Data = successor.Data;
                    root.Right = Remove(root.Right, successor.Data);
                }
                // Has left child
                else if (root.Left != null)
                {
                    // Deal with the child on the left
                    var predecessor = PredecessorSearch(root);
                    root.Data = predecessor.Data;
                    root.Left = Remove(root.Left, predecessor.Data);
                }
            }
            
            return root;
        }

        public AvlTreeNode<T>? SuccessorSearch(AvlTreeNode<T> root)
        // Find the node with the next highest value
        {
        var currentNode = root.Right;
        while (currentNode.Left != null)
            currentNode = currentNode.Left;
        return currentNode;
        }

        public AvlTreeNode<T>? PredecessorSearch(AvlTreeNode<T> root)
        // Find the node with the next lowest value
        {
        var currentNode = root.Left;
        while (currentNode.Right != null)
            currentNode = currentNode.Right;
        return currentNode;
        }

        public AvlTreeNode<T> RotateRight(AvlTreeNode<T> root)
        {
            var newRoot = root.Left;
            root.Left = newRoot.Right;
            newRoot.Right = root;
            root.UpdateHeight();
            newRoot.UpdateHeight();
            return newRoot;
        }

        public AvlTreeNode<T> RotateLeft(AvlTreeNode<T> root)
        {
            var newRoot = root.Right;
            root.Right = newRoot.Left;
            newRoot.Left = root;
            root.UpdateHeight();
            newRoot.UpdateHeight();
            return newRoot;
        }

        private AvlTreeNode<T> Add(AvlTreeNode<T> root, T data)
        {
            // Use the same code as before to insert into the tree.
            var node = BSTAdd(root, data);
            node.UpdateHeight();
            return Rebalance(node);
        }

        private AvlTreeNode<T> Remove(AvlTreeNode<T> root, T data)
        {
            // Use the same code as before to insert into the tree.
            var node = BSTRemove(root, data);

            // Return null if the tree doesn't contain the node.
            // This check relies on the BST_Remove() method behaving
            // correctly, and returning null if the tree doesn't contain
            // the node.
            if(node == null) return null;

            // Update the height and rebalance just like when adding a new node to the tree.
            node.UpdateHeight();
            return Rebalance(node);
        }



    }