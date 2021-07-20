using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
     internal class DateLinkedList<T> : IEnumerable<T>
    {
        Node start;
        Node end;

        public Node Start { get { return start; } }
        public Node End { get { return end; } }

        public void AddFirst(T val)
        {
            Node n = new Node(val);

            if (end == null)
            {
                end = n;
                start = n;
                return;
            }

            start.previous = n;
            n.next = start;
            start = n;
        }

        public bool RemoveFirst(out T saveFirstValue)
        {
            saveFirstValue = default(T);
            if (start == null) return false;

            saveFirstValue = start.data;
            start = start.next;

            if (start == null)
            {
                end = null;
                return true;
            }

            start.previous = null;
            return true;
        }

        public void AddLast(T val)
        {
            if (start == null)
            {
                AddFirst(val);
                return;
            }
            Node n = new Node(val);
            end.next = n;
            n.previous = end;
            end = n;
        }

        public bool RemoveLast(out T saveLastValue)
        {
            saveLastValue = default(T);
            if (start == null) return false;

            saveLastValue = end.data;
            end = end.previous;

            if (end == null)
            {
                start = null;
                return true;
            }

            end.next = null;
            return true;
        }

        public bool GetAt(int position, out T val)
        {
            val = default(T);
            Node temp = start;
            int counter = 0;

            while (temp != null && counter <= position)
            {
                if (counter == position)
                {
                    val = temp.data;
                    return true;
                }

                temp = temp.next;
                counter++;
            }
            return false;
        }

        public bool AddAt(int position, T val)
        {
            if (start == null || position == 0)
            {
                AddFirst(val);
                return true;
            }

            Node n = new Node(val);
            Node temp = start;
            int counter = 0;

            while (temp != null && counter <= position)
            {
                if (counter == position)
                {
                    n.previous = temp.previous;
                    temp.previous.next = n;
                    temp.previous = n;
                    n.next = temp;
                    return true;
                }

                temp = temp.next;
                counter++;
            }

            if (temp == null && counter == position)
            {
                AddLast(val);
                return true;
            }

            return false;
        }

        public void DeleteNode(Node node)
        {
            if (node.next == null) 
            {
                RemoveLast(out node.data);
                return;
            }

            if (node.previous == null)
            {
                //delete first
                RemoveFirst(out node.data);
                return;
            }

            Node tmp;
            tmp = node.previous;
            tmp.next = node.next;
            node.next.previous = tmp;

            node.next = null;
            node.previous = null;
        }

        public void ReplaceNodePosition(Node node)
        {
            if (node.next == null) return;

            Node tmp;

            if (node.previous != null)
            {
                tmp = node.previous;
                tmp.next = node.next;
                node.next.previous = tmp;
            }
            else
            {    //delete first
                start = node.next;
                start.previous = null;
            }
    
            node.next = null;
            end.next = node;
            node.previous = end;
            end = node;
        }

        public override string ToString()
        {
            Node temp = start;
            StringBuilder st = new StringBuilder();

            while (temp != null)
            {
                st.AppendLine(temp.data.ToString());
                temp = temp.next;
            }

            return st.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            //return new ListEnumerator(start);

            Node currentNode = start;
            while (currentNode != null)
            {
                yield return currentNode.data;
                currentNode = currentNode.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }


        internal class Node
        {
            public T data;
            public Node next;
            public Node previous;

            public Node(T data)
            {
                this.data = data;
                next = null;
                previous = null;
            }
        }
    }
}
