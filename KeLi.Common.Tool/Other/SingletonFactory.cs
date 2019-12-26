using System;
using System.Globalization;
using System.Reflection;

namespace KeLi.Common.Tool.Other
{
    /// <summary>
    /// Singleton factory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SingletonFactory<T> where T : class
    {
        /// <summary>
        /// Binding flags.
        /// </summary>
        private const BindingFlags FLAGS = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic;

        /// <summary>
        /// It's for getting instance by reflect way.
        /// </summary>
        private static T _instance = typeof(T).InvokeMember(typeof(T).Name, FLAGS, null, null, null, CultureInfo.CurrentCulture) as T;

        /// <summary>
        /// It's a singleton.
        /// </summary>
        private static readonly T _single = new Lazy<T>(() => _instance).Value;

        /// <summary>
        /// create a T type singleton.
        /// </summary>
        /// <returns></returns>
        public static T CreateInstance()
        {
            return _single;
        }

        /// <summary>
        /// clear the singleton.
        /// </summary>
        public static void ClearInstance()
        {
            _instance = null;
        }
    }
}
