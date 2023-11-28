using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using Identity.Application.Abstraction.Generals;

namespace Identity.Application.Services
{
    public class CachingService : ICachingService
    {
        ObjectCache _memoryCache = MemoryCache.Default;
        public T GetData<T>(string key)
        {
            try
            {
                T item = (T)_memoryCache.Get(key);
                return item;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime, bool slidingExpireTime = false)
        {
            bool res = true;
            try
            {
                if (!string.IsNullOrEmpty(key) && value is not null)
                {
                    CacheItemPolicy policy = new CacheItemPolicy()
                    {
                        SlidingExpiration = expirationTime.TimeOfDay,
                    };

                    if (slidingExpireTime)
                        _memoryCache.Set(key, value, policy);
                    else
                        _memoryCache.Set(key, value, expirationTime);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return res;
        }
        public object RemoveData(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    return _memoryCache.Remove(key);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return false;
        }
    }
}
