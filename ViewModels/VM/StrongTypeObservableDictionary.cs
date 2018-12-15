using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ViewModels.VM
{
    public class StrongTypeObservableDictionary<TKey, TValue> : BaseVM,
    IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        #region Implementation of IEnumerable

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey,TValue>>

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionary.Add(item);
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));

        }

        public void Clear()
        {
            int keysCount = _dictionary.Keys.Count;

            _dictionary.Clear();

            if (keysCount == 0) return;
            
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool remove = _dictionary.Remove(item);

            if (!remove) return false;
            
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
            return true;
        }

        public int Count => _dictionary.Count;

        public bool IsReadOnly => _dictionary.IsReadOnly;

        #endregion

        #region Implementation of IDictionary<TKey,TValue>

        public bool ContainsKey([NotNull] TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add([NotNull] TKey key, [NotNull] TValue value)
        {
            _dictionary.Add(key, value);
            
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
        }

        public bool Remove([NotNull] TKey key)
        {
            bool remove = _dictionary.Remove(key);

            if (!remove) return false;
            
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
            return true;
        }

        public bool TryGetValue([NotNull] TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public TValue this[[NotNull] TKey key]
        {
            get { return _dictionary[key]; }
            set
            {
                bool changed = _dictionary[key].Equals(value);

                if (!changed) return;
                _dictionary[key] = value;
                
                OnPropertyChanged(nameof(Keys));
                OnPropertyChanged(nameof(Values));
            }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        #endregion
    }
}
