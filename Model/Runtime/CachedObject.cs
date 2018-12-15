using System;
using JetBrains.Annotations;
using Tools.Attributes;
using Wukker.Tools;

namespace Model.Runtime
{
    public class CachedObject<T> where T: class
    {
        public CachedObject([NotNull] T obj, CacheTerm expirationType = CacheTerm.LongTerm)
        {
            CachedMoment = DateTime.Now;
            Object = obj;
            _timeExpiration = TimeSpan.FromMinutes(expirationType.GetPeriod());
        }

        private T _object;
        private readonly TimeSpan _timeExpiration;
        public DateTime CachedMoment { get; }

        public T Object
        {
            get
            {
                if (IsExpirated) return null;
                return _object;
            }
            private set { _object = value; }
        }

        public bool IsExpirated => DateTime.Now - CachedMoment > _timeExpiration;
    }

    public enum CacheTerm
    {
        [Period(5)]
        ShortTerm,
        [Period(30)]
        LongTerm,
        [Period(9999)]
        NeverExpired
    }
}
