using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

using TripPoint.Model.Domain;

namespace TripPoint.WindowsPhone.Utils.View
{
    /// <summary>
    /// Memory cache for picture bitmaps
    /// </summary>
    public class PictureBitmapCache
    {
        private static PictureBitmapCache _instance;

        private IDictionary<int, CacheItem> _cache;
        private int _cacheMaxSize;

        /// <summary>
        /// Initializes the cache with the given maximum number of items
        /// </summary>
        /// <param name="maxSize"></param>
        public static void Initialize(int maxSize)
        {
            if (_instance != null) return;

            _instance = new PictureBitmapCache(maxSize);
        }

        protected PictureBitmapCache(int maxSize)
        {
            if (maxSize <= 0) throw new ArgumentException("Cache max size must be > 0");

            _cache = new Dictionary<int, CacheItem>();
            _cacheMaxSize = maxSize;
        }
        
        public static PictureBitmapCache Instance
        {
            get
            {
                if (_instance == null) throw new InvalidOperationException("Cache must be initialized");

                return _instance;
            }
        }

        /// <summary>
        /// Retrieves a bitmap which is associated with the given picture from cache
        /// Returns null if there is no bitmap
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public BitmapSource Get(Picture picture)
        {
            if (picture == null) throw new ArgumentException("picture");

            var item = _cache.ContainsKey(picture.ID) ? _cache[picture.ID] : null;

            return Get(item);
        }

        private BitmapSource Get(CacheItem item)
        {
            if (item == null) return null;

            item.Hits++;

            return item.Bitmap;
        }

        /// <summary>
        /// Adds the given bitmap to the cache and associates it with the given picture
        /// Does nothing if the bitmap already cached
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="bitmap"></param>
        public void Add(Picture picture, BitmapSource bitmap)
        {
            if (picture == null) throw new ArgumentException("picture");
            if (bitmap == null) return;

            Add(new CacheItem
            {
               Picture = picture,
               Bitmap = bitmap
            });
        }

        private void Add(CacheItem item)
        {
            if (_cache.ContainsKey(item.Picture.ID)) return;

            EvictLeastFrequentlyUsedItemIfNecessary();

            _cache[item.Picture.ID] = item;
        }

        private void EvictLeastFrequentlyUsedItemIfNecessary()
        {
            if (_cache.Count < _cacheMaxSize) return;

            var item = _cache
                .OrderBy(pair => pair.Value.Hits)
                .ThenBy(pair => pair.Value.AddedOn)
                .FirstOrDefault();

            _cache.Remove(item);
        }

        /// <summary>
        /// Removes all cache entries
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }

        #region CacheItem Class

        class CacheItem
        {
            public CacheItem()
            {
                Hits = 0;
                AddedOn = DateTime.Now;
            }

            public Picture Picture { get; set; }

            public BitmapSource Bitmap { get; set; }

            public int Hits { get; set; }

            public DateTime AddedOn { get; set; }
        }

        #endregion
    }
}
