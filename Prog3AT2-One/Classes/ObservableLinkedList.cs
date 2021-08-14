/*
 *  File Name:   ObservableLinkedList.cs
 *
 *  Copyright (c) 2021 Bradley Willcott
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * ****************************************************************
 * Name: Bradley Willcott
 * ID:   M198449
 * Date: 6/08/2021
 * ****************************************************************
 */

namespace Prog3AT2_One.Classes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Runtime.Serialization;

    using static System.Collections.Specialized.NotifyCollectionChangedAction;

    /// <summary>
    /// Defines the <see cref="ObservableLinkedList" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>

    public class ObservableLinkedList<T> : LinkedList<T>, IList<T>, IList, INotifyCollectionChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableLinkedList{T}"/> class.
        /// </summary>
        public ObservableLinkedList() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableLinkedList{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection <see cref="IEnumerable{T}"/>.</param>
        public ObservableLinkedList(IEnumerable<T> collection) : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableLinkedList{T}"/> class.
        /// </summary>
        /// <param name="info">The info <see cref="SerializationInfo"/>.</param>
        /// <param name="context">The context <see cref="StreamingContext"/>.</param>
        protected ObservableLinkedList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets a value indicating whether the <see cref="IList" /> has a fixed size.
        /// </summary>
        public bool IsFixedSize { get => false; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection" /> is read-only.
        /// </summary>
        public bool IsReadOnly { get => false; }

        /// <summary>
        /// Gets or sets the <see cref="object"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <seealso cref="this[int]"/>
        /// <value>
        /// The <see cref="object"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Argument ({value}) is wrong type: {typeof(T)}</exception>
        object IList.this[int index]
        {
            get => GetNodeAt(index).Value;
            set
            {
                try
                {
                    this[index] = (T)value;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException($"Argument ({value}) is wrong type: {typeof(T)}");
                }
            }
        }

        /// <summary>
        ///  Gets or sets the element at the specified <paramref name="index"/>.
        /// </summary>
        /// <remarks>
        /// Due to the implementation requirements of this method, it is strongly advised
        /// that it NOT be used within a loop of any kind.<br/>
        /// If you need to iterate through the list, then use <see cref="LinkedList{T}.GetEnumerator"/>
        /// </remarks>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// index is not a valid index in the <see cref="IList{T}" />.
        /// </exception>
        public T this[int index]
        {
            get => GetNodeAt(index).Value;
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException($"The index ({index}) is out of range: {((index < 0) ? "0" : $">= {Count}")}");
                }

                LinkedListNode<T> node = GetNodeAt(index);

                if (!node.Value.Equals(value))
                {
                    node.Value = value;
                    OnCollectionChanged(Replace, value, index);
                }
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}" />.</param>
        public void Add(T item)
        {
            AddLast(item);
        }

        /// <summary>
        /// Adds an item to the <see cref="IList" />.
        /// </summary>
        /// <param name="value">The <see cref="object"/> to add to the <see cref="IList" />.</param>
        /// <returns>
        /// The position into which the new element was inserted, or -1 to indicate that the item was not
        /// inserted into the collection.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        int IList.Add(object value)
        {
            int rtn = -1;

            if (value is T item)
            {
                rtn = (AddLast(item) != null) ? Count - 1 : -1;
            }

            return rtn;
        }

        /// <summary>
        /// Adds a new node containing the specified value after the specified existing node in the
        /// <see cref="LinkedList{T}" />.
        /// </summary>
        /// <param name="node">The node<see cref="LinkedListNode{T}"/>.</param>
        /// <param name="value">The value to add to the <see cref="LinkedList{T}" />.</param>
        /// <returns>The <see cref="LinkedListNode{T}"/>.</returns>
        new public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = base.AddAfter(node, value);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, value);

            return result;
        }

        /// <summary>
        /// Adds the specified new node after the specified existing node in the
        /// <see cref="T:System.Collections.Generic.LinkedList`1" />.
        /// </summary>
        /// <param name="node">The node<see cref="LinkedListNode{T}"/>.</param>
        /// <param name="newNode">The newNode<see cref="LinkedListNode{T}"/>.</param>
        new public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            base.AddAfter(node, newNode);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, newNode.Value);
        }

        /// <summary>
        /// Adds a new node containing the specified value before the specified existing node in the
        /// <see cref="LinkedList{T}" />.
        /// </summary>
        /// <param name="node">The node<see cref="LinkedListNode{T}"/>.</param>
        /// <param name="value">The value to add to the <see cref="LinkedList{T}" />.</param>
        /// <returns>The <see cref="LinkedListNode{T}"/>.</returns>
        new public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = base.AddBefore(node, value);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, value);

            return result;
        }

        /// <summary>
        /// Adds the specified new node before the specified existing node in the
        /// <see cref="LinkedList{T}" />.
        /// </summary>
        /// <param name="node">The node<see cref="LinkedListNode{T}"/>.</param>
        /// <param name="newNode">The newNode<see cref="LinkedListNode{T}"/>.</param>
        new public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            base.AddBefore(node, newNode);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, newNode.Value);
        }

        /// <summary>
        /// Adds a new node containing the specified value at the start of the
        /// <see cref="LinkedList{T}" />.
        /// </summary>
        /// <param name="value">The value <see cref="T"/>.</param>
        /// <returns>The <see cref="LinkedListNode{T}"/>.</returns>
        new public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> result = base.AddFirst(value);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, value);

            return result;
        }

        /// <summary>
        /// Adds the specified new node at the start of the
        /// <see cref="LinkedList{T}" />.
        /// </summary>
        /// <param name="node">The node<see cref="LinkedListNode{T}"/>.</param>
        new public void AddFirst(LinkedListNode<T> node)
        {
            base.AddFirst(node);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, node.Value);
        }

        /// <summary>
        /// Adds a new node containing the specified value at the end of the
        /// <see cref="LinkedList{T}" />.
        /// </summary>
        /// <param name="value">The value <see cref="T"/>.</param>
        /// <returns>The <see cref="LinkedListNode{T}"/>.</returns>
        new public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> result = base.AddLast(value);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, value);

            return result;
        }

        /// <summary>
        /// Adds the specified new node at the end of the
        /// <see cref="LinkedList{T}" />.
        /// </summary>
        /// <param name="node">The node <see cref="LinkedListNode{T}"/>.</param>
        new public void AddLast(LinkedListNode<T> node)
        {
            base.AddLast(node);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, node.Value);
        }

        /// <summary>
        ///Removes all items from the <see cref="IList" />.
        /// </summary>
        void IList.Clear()
        {
            Clear();
        }

        /// <summary>
        /// Removes all nodes from the <see cref="LinkedList{T}" />.
        /// </summary>
        new public void Clear()
        {
            base.Clear();
            OnCollectionChanged(Reset);
        }

        /// <summary>
        /// Determines whether the <see cref="IList"/> contains a specific <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="IList"/>.</param>
        /// <returns>
        /// true if the <see cref="object"/> is found in the <see cref="IList"/> otherwise, false.
        /// </returns>
        bool IList.Contains(object value)
        {
            bool rtn = false;

            if (value is T item)
            {
                rtn = IndexOf(item) > -1;
            }

            return rtn;
        }

        /// <summary>
        /// Gets the node at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">.</param>
        /// <returns>the node located at <paramref name="index"/>.</returns>
        public LinkedListNode<T> GetNodeAt(int index)
        {
            LinkedListNode<T> rtn;

            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException($"index: {index} | Count: {Count}");
            }
            // First node
            else if (index == 0)
            {
                rtn = First;
            }
            // Last node
            else if (index == Count - 1)
            {
                rtn = Last;
            }
            // Node at 'index'
            else
            {
                // Find the right indexed node
                int idx = 0;
                LinkedListNode<T> current = First;

                do
                {
                    current = current.Next;
                } while (++idx < index);

                rtn = current;
            }

            return rtn;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="IList{T}" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="IList{T}" />.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int IndexOf(T item)
        {
            int index = -1;
            bool found = false;

            // Search sequentially through chain of nodes
            foreach (T value in this)
            {
                index++;

                // Do we have a match?
                if (value == null)
                {
                    if (item == null)
                    {
                        found = true;
                        break;
                    }
                }
                // What about now?
                else if (item != null && value.Equals(item))
                {
                    found = true;
                    break;
                }
            }

            return found ? index : -1;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="IList"/>.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="IList"/>.</param>
        /// <returns>The index of value if found in the list; otherwise, -1.</returns>
        int IList.IndexOf(object value)
        {
            int rtn = -1;

            if (value is T item)
            {
                rtn = IndexOf(item);
            }

            return rtn;
        }

        /// <summary>
        /// Inserts an item to the <see cref="IList{T}" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="IList{T}" />.</param>
        public void Insert(int index, T item)
        {
            // Prepend 'item'
            if (index == 0)
            {
                AddFirst(item);
            }
            // Append 'item'
            else if (index == Count)
            {
                AddLast(item);
            }
            // Insert 'item' at 'index'
            else if (Count > 0 && index < Count)
            {
                // Find the right indexed node
                int idx = 0;
                LinkedListNode<T> current = First;

                do
                {
                    current = current.Next;
                } while (++idx < index);

                // Insert 'item' before 'current'
                AddBefore(current, item);
            }
            // Bad show
            else
            {
                throw new ArgumentOutOfRangeException($"index: {index} | Count: {Count}");
            }
        }

        /// <summary>
        /// Inserts an item to the <see cref="IList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The object to insert into the <see cref="IList"/>.</param>
        public void Insert(int index, object value)
        {
            if (value is T item)
            {
                Insert(index, item);
            }
        }

        /// <summary>
        /// Removes the first occurrence of the specified value from the
        /// <see cref="LinkedList{T}" />.
        /// </summary>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        new public bool Remove(T value)
        {
            int index = IndexOf(value);
            bool rtn = base.Remove(value);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, value, index);
            return rtn;
        }

        /// <summary>
        /// Removes the specified node from the <see cref="LinkedList{T}" />.
        /// </summary>
        /// <param name="node">The node<see cref="LinkedListNode{T}"/>.</param>
        new public void Remove(LinkedListNode<T> node)
        {
            base.Remove(node);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="IList"/>.
        /// </summary>
        /// <param name="value">The object to remove from the <see cref="IList"/>.</param>
        void IList.Remove(object value)
        {
            if (value is T item)
            {
                Remove(item);
            }
        }

        /// <summary>
        /// Removes the <see cref="IList{T}"/> item at the specified index.
        /// </summary>
        /// <param name="index"> The zero-based index of the <paramref name="item" /> to remove.</param>
        public void RemoveAt(int index)
        {
            // Remove first 'item'
            if (index == 0)
            {
                RemoveFirst();
            }
            // Remove last 'item'
            else if (index == Count - 1)
            {
                RemoveLast();
            }
            // Remove 'item' at 'index'
            else if (Count > 0 && index < Count)
            {
                // Find the right indexed node
                int idx = 0;
                LinkedListNode<T> current = First;

                do
                {
                    current = current.Next;
                } while (++idx < index);

                // Remove 'current'
                base.Remove(current);
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, current.Value, index);
            }
            // Bad show
            else
            {
                throw new ArgumentOutOfRangeException($"index: {index} | Count: {Count}");
            }
        }

        /// <summary>
        /// Removes the node at the start of the <see cref=LinkedList{T}" />.
        /// </summary>
        new public void RemoveFirst()
        {
            LinkedListNode<T> node = First;
            base.RemoveFirst();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, node.Value, 0);
        }

        /// <summary>
        /// Removes the node at the end of the <see cref="LinkedList{T}" />.
        /// </summary>
        new public void RemoveLast()
        {
            LinkedListNode<T> node = Last;
            int index = Count - 1;
            base.RemoveLast();
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, node.Value, index);
        }

        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        /// <param name="action">The action.</param>
        private void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action));
        }

        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="changedItem">The changed item.</param>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object? changedItem)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItem));
        }

        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="changedItem">The changed item.</param>
        /// <param name="index">The index.</param>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object? changedItem, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItem, index));
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged" /> event.
        /// </summary>
        /// <param name="args">The args<see cref="NotifyCollectionChangedEventArgs"/>.</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }
    }
}