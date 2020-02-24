using System;
using System.Reflection;

using static System.Globalization.CultureInfo;
using static System.Reflection.BindingFlags;

namespace KeLi.Common.Tool.Other
{
    /// <summary>
    ///     Singleton factory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SingletonFactory<T> where T : class
    {
        /// <summary>
        ///     Binding flags.
        /// </summary>
        private const BindingFlags FLAGS = BindingFlags.CreateInstance | Instance | NonPublic;

        /// <summary>
        ///     It's for getting instance by reflect way.
        /// </summary>
        private static T _inst =  typeof(T).InvokeMember(typeof(T).Name, FLAGS, null, null, null, CurrentCulture) as T;

        /// <summary>
        ///     It's a singleton.
        /// </summary>
        private static readonly T _single = new Lazy<T>(() => _inst).Value;

        /// <summary>
        ///     create a T type singleton.
        /// </summary>
        /// <returns></returns>
        public static T CreateInstance()
        {
            return _single;
        }

        /// <summary>
        ///     clear the singleton.
        /// </summary>
        public static void ClearInstance()
        {
            _inst = null;
        }
    }
}