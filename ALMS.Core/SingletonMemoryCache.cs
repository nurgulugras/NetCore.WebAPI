using System;
using Microsoft.Extensions.Caching.Memory;

namespace ALMS.Core
{
    public sealed class SingletonMemoryCache
    {
        private static volatile SingletonMemoryCache instance; //  Locks var until assignment is complete for double safety
        private static MemoryCache memoryCache;
        private static object syncRoot = new Object();
        private SingletonMemoryCache() { }

        /// <summary>
        /// Singleton Cache Instance
        /// </summary>
        public static SingletonMemoryCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            InitializeInstance();

                        }
                    }
                }
                return instance;
            }
        }

        private static void InitializeInstance()
        {
            instance = new SingletonMemoryCache();
            memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        /// <summary>
        /// Writes Key Value Pair to Cache
        /// </summary>
        /// <param name="Key">Key to associate Value with in Cache</param>
        /// <param name="Value">Value to be stored in Cache associated with Key</param>
        public void Write<TObject>(string Key, TObject Value)
        {
            memoryCache.Set(Key, Value);
        }
        public void Write<TObject>(string Key, TObject Value, DateTimeOffset absoluteExpiration)
        {
            memoryCache.Set(Key, Value, absoluteExpiration);
        }
        public bool IsCacheCreated(string Key)
        {
            return memoryCache.TryGetValue(Key, out object values);
        }

        /// <summary>
        /// Returns Value stored in Cache
        /// </summary>
        /// <param name="Key"></param>
        /// <returns>Value stored in cache</returns>
        public TObject Read<TObject>(string Key)
        {
            memoryCache.TryGetValue(Key, out TObject values);
            return values;
        }

        /// <summary>
        /// Returns Value stored in Cache, null if non existent
        /// </summary>
        /// <param name="Key"></param>
        /// <returns>Value stored in cache</returns>
        public TObject TryRead<TObject>(string Key)
        {
            try
            {
                return (TObject)memoryCache.Get(Key);
            }
            catch (Exception)
            {
                return default(TObject);
            }
        }
        public void Remove(string Key)
        {
            memoryCache.Remove(Key);
        }
    }

}