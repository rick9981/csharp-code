using System.Collections.Concurrent;

namespace AppEdgeHostedService.Utils
{
    public class ObjectPool<T> where T : class
    {
        private readonly ConcurrentQueue<T> _objects = new();
        private readonly Func<T> _objectGenerator;
        private readonly int _maxObjects;
        private int _currentCount = 0;

        public ObjectPool(Func<T> objectGenerator, int maxObjects = 100)
        {
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
            _maxObjects = maxObjects;
        }

        public T Rent()
        {
            if (_objects.TryDequeue(out var item))
            {
                Interlocked.Decrement(ref _currentCount);
                return item;
            }

            return _objectGenerator();
        }

        public void Return(T item)
        {
            if (item != null && _currentCount < _maxObjects)
            {
                _objects.Enqueue(item);
                Interlocked.Increment(ref _currentCount);
            }
        }

        public int Count => _currentCount;
    }
}