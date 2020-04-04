using System;
using System.Collections.Generic;
using System.Linq;

namespace Focus.Service.ReportConstructor.Core.Abstract
{
    public abstract class ListContainer<T> where T : class, IOrderable
    {
        protected IList<T> _collection;

        public virtual int Add(T element, int position)
        {
            if (element is null)
                throw new ArgumentNullException(
                    $"DOMAIN EXCEPTION: Can't add null element to {nameof(T)} collection in {GetType()}");

            if (position == -1)
            {
                position = _collection.Count;

                element.Order = position;

                _collection.Add(element);
            }
            else
            {
                _collection.Insert(position, element);
            }

            UpdateOrder();

            return position;
        }

        public virtual void Remove(T element)
        {
            if (element is null)
                throw new ArgumentException(
                    $"DOMAIN EXCEPTION: Can't remove null element from {nameof(T)} collection in {GetType()}");

            _collection.Remove(element);

            UpdateOrder();
        }

        public virtual void Remove(int position)
        {
            if (position < 0 || position >= _collection.Count)
                throw new ArgumentException(
                    $"DOMAIN EXCEPTION: Can't remove element at {position} position from {nameof(T)} collection in {GetType()}");

            _collection.RemoveAt(position);

            UpdateOrder();
        }

        public virtual void Replace(T element, int position = -1)
        {
            if (element is null)
                throw new ArgumentNullException(
                    $"DOMAIN EXCEPTION: Can't replace element at {position} in {nameof(T)} collection in {GetType()}");

            position = position == -1 ? element.Order : position;

            if (position < 0 || position >= _collection.Count)
                throw new ArgumentException(
                    $"DOMAIN EXCEPTION: Can't replace null element from {nameof(T)} collection in {GetType()}");

            element.Order = position;

            _collection[position] = element;
        }

        public virtual T[] GetArray()
        {
            return _collection
                .AsEnumerable()
                .ToArray();
        }

        protected void UpdateOrder()
        {
            int i = 0;

            foreach (var e in _collection)
                e.Order = i++;
        }
    }
}