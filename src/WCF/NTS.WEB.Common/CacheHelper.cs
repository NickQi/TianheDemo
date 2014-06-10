using System;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace NTS.WEB.Common
{
    public class CacheHelper
    {
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache对象值 
        /// </summary>
        /// <param name="cacheKey">索引键值</param>
        /// <returns>返回缓存对象</returns> 
        public static object GetCache(string cacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }
        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache对象值
        /// </summary>
        /// <param name="cacheKey">索引键值</param>
        /// <param name="objObject">缓存对象</param>
        public static void SetCache(string cacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null,DateTime.Now.AddMinutes(500), Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache对象值
        /// </summary>
        /// <param name="cacheKey">索引键值</param>
        /// <param name="objObject">缓存对象</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="slidingExpiration">最后一次访问所插入对象时与该对象过期时之间的时间间隔</param>
        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, slidingExpiration);
           
        }
        public static void SetCache(string cacheKey, object objObject, int slidingExpirationSeconds)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(slidingExpirationSeconds));
      
        }

        public static void RemoveCache(string cacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Remove(cacheKey);

        }
       
    }
}
