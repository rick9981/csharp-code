using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppVListView.Models;

namespace AppVListView.Services
{
    public class VirtualEquipmentDataSource : IList, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly List<EquipmentData> _cache = new List<EquipmentData>();
        private readonly Dictionary<int, EquipmentData> _itemCache = new Dictionary<int, EquipmentData>();
        private readonly Random _random = new Random();

        private const int CACHE_SIZE = 1000;
        private const int TOTAL_ITEMS = 100000; // 模拟10万条数据

        private readonly string[] _equipmentTypes = { "泵", "电机", "压缩机", "风机", "阀门", "传感器", "控制器" };
        private readonly string[] _locations = { "车间A", "车间B", "车间C", "仓库1", "仓库2", "办公区", "实验室" };
        private readonly string[] _statuses = { "正常", "警告", "故障", "维护中", "停机" };

        public int Count => TOTAL_ITEMS;
        public bool IsReadOnly => true;
        public bool IsFixedSize => true;
        public object SyncRoot => this;
        public bool IsSynchronized => false;

        public object this[int index]
        {
            get => GetItem(index);
            set => throw new NotSupportedException();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private EquipmentData GetItem(int index)
        {
            if (index < 0 || index >= TOTAL_ITEMS)
                return null;

            // 检查缓存
            if (_itemCache.TryGetValue(index, out var cachedItem))
                return cachedItem;

            // 生成新项目
            var item = GenerateEquipmentData(index);

            // 管理缓存大小
            if (_itemCache.Count >= CACHE_SIZE)
            {
                // 移除最旧的项目（简单的FIFO策略）
                var keysToRemove = new List<int>();
                int removeCount = CACHE_SIZE / 4; // 移除25%的缓存
                int count = 0;

                foreach (var key in _itemCache.Keys)
                {
                    if (count++ >= removeCount) break;
                    keysToRemove.Add(key);
                }

                foreach (var key in keysToRemove)
                {
                    _itemCache.Remove(key);
                }
            }

            _itemCache[index] = item;
            return item;
        }

        private EquipmentData GenerateEquipmentData(int index)
        {
            return new EquipmentData
            {
                EquipmentId = $"EQ{index:D6}",
                EquipmentName = $"{_equipmentTypes[index % _equipmentTypes.Length]}{(index % 100) + 1:D2}",
                Status = _statuses[_random.Next(_statuses.Length)],
                Temperature = Math.Round(20 + _random.NextDouble() * 80, 2),
                Pressure = Math.Round(1 + _random.NextDouble() * 10, 2),
                Vibration = Math.Round(_random.NextDouble() * 5, 3),
                Timestamp = DateTime.Now.AddMinutes(-_random.Next(0, 1440)),
                Location = _locations[index % _locations.Length]
            };
        }

        // 模拟数据更新
        public async Task RefreshDataAsync()
        {
            await Task.Run(() =>
            {
                // 清除部分缓存以模拟数据更新
                var keysToRemove = new List<int>();
                foreach (var key in _itemCache.Keys)
                {
                    if (_random.NextDouble() < 0.1) // 10%的概率更新
                    {
                        keysToRemove.Add(key);
                    }
                }

                foreach (var key in keysToRemove)
                {
                    _itemCache.Remove(key);
                }
            });

            // 通知UI更新
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #region IList Implementation
        public int Add(object value) => throw new NotSupportedException();
        public void Clear() => throw new NotSupportedException();
        public bool Contains(object value) => false;
        public int IndexOf(object value) => -1;
        public void Insert(int index, object value) => throw new NotSupportedException();
        public void Remove(object value) => throw new NotSupportedException();
        public void RemoveAt(int index) => throw new NotSupportedException();
        public void CopyTo(Array array, int index) => throw new NotSupportedException();

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return GetItem(i);
            }
        }
        #endregion
    }
}
