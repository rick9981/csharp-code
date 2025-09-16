using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEdgeCollector.Utils
{
    public class ObjectPool<T> where T : class
    {
        private readonly Func<T> _factory;
        private readonly ConcurrentBag<T> _items = new ConcurrentBag<T>();
        private readonly int _maxSize;
        private int _count = 0;

        public ObjectPool(Func<T> factory, int maxSize = 32)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _maxSize = maxSize;
        }

        public T Rent()
        {
            if (_items.TryTake(out var item))
                return item;

            if (System.Threading.Interlocked.Increment(ref _count) <= _maxSize)
                return _factory();

            System.Threading.Interlocked.Decrement(ref _count);
            // 达到池上限，直接返回新实例（或等待/抛错）
            return _factory();
        }

        public void Return(T item)
        {
            if (item == null) return;

            if (_items.Count >= _maxSize) return;
            _items.Add(item);
        }
    }
}
