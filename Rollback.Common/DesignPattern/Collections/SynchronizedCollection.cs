namespace Rollback.Common.DesignPattern.Collections
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public class SynchronizedCollection<T> : IList<T>
    {
        private readonly List<T> _items;
        private readonly object _sync;

        public SynchronizedCollection()
        {
            _sync = new();
            _items = new();
        }

        public SynchronizedCollection(IEnumerable<T> items)
        {
            _sync = new();

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            _items = new List<T>(items);
        }

        public SynchronizedCollection(object syncRoot)
        {
            _sync = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));
            _items = new List<T>();
        }

        public SynchronizedCollection(object syncRoot, IEnumerable<T> items)
        {
            _sync = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            _items = new List<T>(items);
        }

        public SynchronizedCollection(object syncRoot, params T[] items)
        {
            _sync = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            _items = new List<T>(items.Length);
            for (int i = 0; i < items.Length; i++)
                _items.Add(items[i]);
        }

        public int Count
        {
            get { lock (_sync) { return _items.Count; } }
        }

        public T this[int index]
        {
            get
            {
                lock (_sync)
                {
                    return _items[index];
                }
            }
            set
            {
                lock (_sync)
                {
                    if (index < 0 || index >= _items.Count)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    SetItem(index, value);
                }
            }
        }

        public void Add(T item)
        {
            lock (_sync)
            {
                int index = _items.Count;
                InsertItem(index, item);
            }
        }

        public void Clear()
        {
            lock (_sync)
            {
                Clear_items();
            }
        }

        public void CopyTo(T[] array, int index)
        {
            lock (_sync)
            {
                _items.CopyTo(array, index);
            }
        }

        public bool Contains(T item)
        {
            lock (_sync)
            {
                return _items.Contains(item);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            lock (_sync)
            {
                return _items.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (_sync)
            {
                return InternalIndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (_sync)
            {
                if (index < 0 || index > _items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                InsertItem(index, item);
            }
        }

        int InternalIndexOf(T item)
        {
            int count = _items.Count;

            for (int i = 0; i < count; i++)
            {
                if (Equals(_items[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Remove(T item)
        {
            lock (_sync)
            {
                int index = InternalIndexOf(item);
                if (index < 0)
                    return false;

                RemoveItem(index);
                return true;
            }
        }

        public void RemoveAt(int index)
        {
            lock (_sync)
            {
                if (index < 0 || index >= _items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                RemoveItem(index);
            }
        }

        protected virtual void Clear_items() =>
            _items.Clear();

        protected virtual void InsertItem(int index, T item) =>
            _items.Insert(index, item);

        protected virtual void RemoveItem(int index) =>
            _items.RemoveAt(index);

        protected virtual void SetItem(int index, T item) =>
            _items[index] = item;

        bool ICollection<T>.IsReadOnly =>
            false;

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (_sync)
                return ((IList<T>)_items).GetEnumerator();
        }

        void ICollection<T>.CopyTo(T[] array, int index)
        {
            lock (_sync)
            {
                ((IList<T>)_items).CopyTo(array, index);
            }
        }

        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                VerifyValueType(value!);
                this[index] = value;
            }
        }

        static void VerifyValueType(object value)
        {
            if (value == null)
            {
                if (typeof(T).IsValueType)
                    throw new ArgumentException("Incorrect type of null type");
            }
            else if (value is not T)
                throw new ArgumentException($"Wrong value type, value is {value.GetType().FullName}...");
        }
    }
}
