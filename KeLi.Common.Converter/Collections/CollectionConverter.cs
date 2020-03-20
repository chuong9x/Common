/*
 * MIT License
 *
 * Copyright(c) 2019 KeLi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/*
             ,---------------------------------------------------,              ,---------,
        ,----------------------------------------------------------,          ,"        ,"|
      ,"                                                         ,"|        ,"        ,"  |
     +----------------------------------------------------------+  |      ,"        ,"    |
     |  .----------------------------------------------------.  |  |     +---------+      |
     |  | C:\>FILE -INFO                                     |  |  |     | -==----'|      |
     |  |                                                    |  |  |     |         |      |
     |  |                                                    |  |  |/----|`---=    |      |
     |  |              Author: KeLi                          |  |  |     |         |      |
     |  |              Email: kelistudy@163.com              |  |  |     |         |      |
     |  |              Creation Time: 10/30/2019 07:08:41 PM |  |  |     |         |      |
     |  | C:\>_                                              |  |  |     | -==----'|      |
     |  |                                                    |  |  |   ,/|==== ooo |      ;
     |  |                                                    |  |  |  // |(((( [66]|    ,"
     |  `----------------------------------------------------'  |," .;'| |((((     |  ,"
     +----------------------------------------------------------+  ;;  | |         |,"
        /_)_________________________________________________(_/  //'   | +---------+
           ___________________________/___  `,
          /  oooooooooooooooo  .o.  oooo /,   \,"-----------
         / ==ooooooooooooooo==.o.  ooo= //   ,`\--{)B     ,"
        /_==__==========__==_ooo__ooo=_/'   /___________,"
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using static System.Reflection.BindingFlags;

namespace KeLi.Common.Converter.Collections
{
    /// <summary>
    ///     A data conllection converter.
    /// </summary>
    public static class CollectionConverter
    {
        /// <summary>
        ///     Binding flags.
        /// </summary>
        private const BindingFlags _flags = Public | NonPublic | Instance;

        /// <summary>
        ///     Converts the DataTable to the List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable dt) where T : new()
        {
            if (dt is null)
                throw new ArgumentNullException(nameof(dt));

            var results = new List<T>();

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var tab = Activator.CreateInstance<T>();

                var properties = tab.GetType().GetProperties();

                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    var index = j;

                    foreach (var property in properties.Where(w => w.Name.Equals(dt.Columns[index].ColumnName)))
                    {
                        property.SetValue(tab, dt.Rows[i][j] != DBNull.Value ? dt.Rows[i][j] : null, null);

                        break;
                    }
                }

                results.Add(tab);
            }

            return results;
        }

        /// <summary>
        ///     Converts the DataTable to the IList.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList ToIList(DataTable dt, Type type)
        {
            if (dt is null)
                throw new ArgumentNullException(nameof(dt));

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            var listType = typeof(List<>).MakeGenericType(type);

            var results = Activator.CreateInstance(listType) as IList;

            var constructor = type.GetConstructors(_flags).OrderBy(c => c.GetParameters().Length).FirstOrDefault();

            if (constructor is null)
                return results;

            var parms = constructor.GetParameters();

            var values = new object[parms.Length];

            foreach (DataRow dr in dt.Rows)
            {
                var index = 0;

                foreach (var item in parms)
                {
                    object val = null;

                    if (dr[item.Name] != null && dr[item.Name] != DBNull.Value)
                        val = Convert.ChangeType(dr[item.Name], item.ParameterType);

                    values[index++] = val;
                }

                results?.Add(constructor.Invoke(values));
            }

            return results;
        }

        /// <summary>
        ///     Converts the SqlDataReader to the List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(SqlDataReader reader) where T : new()
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            var results = new List<T>();

            if (!reader.HasRows)
                return results;

            var properties = typeof(T).GetProperties();

            while (reader.Read())
            {
                var inst = Activator.CreateInstance<T>();

                foreach (var property in properties)
                    property.SetValue(inst, reader[property.Name] is DBNull ? null : reader[property.Name]);

                results.Add(inst);
            }

            reader.Close();

            return results;
        }

        /// <summary>
        ///     Converts the SqlDataReader to the IList.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList ToIList(SqlDataReader reader, Type type)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (type is null)
                throw new ArgumentNullException(nameof(type));

            var listType = typeof(List<>).MakeGenericType(type);

            if (!reader.HasRows)
                return Activator.CreateInstance(listType) as IList;

            var results = Activator.CreateInstance(listType) as IList;

            var constructors = type.GetConstructors(Public | NonPublic | Instance);

            var constructor = constructors.OrderBy(c => c.GetParameters().Length).First();

            var parms = constructor.GetParameters();

            var values = new object[parms.Length];

            while (reader.Read())
            {
                var index = 0;

                foreach (var parm in parms)
                {
                    var val = reader[parm.Name];

                    if (val != DBNull.Value)
                        val = Convert.ChangeType(val, parm.ParameterType);

                    values[index++] = val;
                }

                results?.Add(constructor.Invoke(values));
            }

            reader.Close();

            return results;
        }

        /// <summary>
        ///     Converts the List to the DataTable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static DataTable ToTable<T>(IEnumerable<T> ts)
        {
            if (ts is null)
                throw new ArgumentNullException(nameof(ts));

            var tmpTs = ts.ToList();

            var results = new DataTable();

            var props = tmpTs[0].GetType().GetProperties();

            foreach (var prop in props)
                results.Columns.Add(prop.Name, prop.PropertyType);

            foreach (var t in ts)
            {
                var tmps = new ArrayList();

                foreach (var prop in props)
                    tmps.Add(prop.GetValue(t, null));

                results.LoadDataRow(tmps.ToArray(), true);
            }

            return results;
        }

        /// <summary>
        ///     Converts the SqlDataReader to the DataTable.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static DataTable ToTable(SqlDataReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            var results = new DataTable();

            if (!reader.HasRows)
                return results;

            // Adds the DataTable's columns.
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var col = new DataColumn
                {
                    DataType = reader.GetFieldType(i),

                    ColumnName = reader.GetName(i)
                };

                results.Columns.Add(col);
            }

            // Adds the DataTable's content.
            while (reader.Read())
            {
                var row = results.NewRow();

                for (var i = 0; i < reader.FieldCount; i++)
                    row[i] = reader[i];

                results.Rows.Add(row);
            }

            reader.Close();

            return results;
        }

        /// <summary>
        ///     Converts the T to the S.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static S ToAnyType<T, S>(this T t) where T : class where S : new()
        {
            if (t is null)
                throw new ArgumentNullException(nameof(t));

            var result = Activator.CreateInstance<S>();

            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var value = prop.GetValue(t);

                var info = typeof(S).GetProperty(prop.Name);

                if (value is null || info is null)
                    continue;

                var type = info.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var val = Convert.ChangeType(value, type.GetGenericArguments()[0]);

                    info.SetValue(result, val);
                }
                else
                {
                    var val = Convert.ChangeType(value, info.PropertyType);

                    info.SetValue(result, val);
                }
            }

            return result;
        }

        /// <summary>
        ///     Convert the object to the SqlDbType.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static SqlDbType ToDbType(object obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            var result = SqlDbType.NChar;

            var type = obj.GetType();

            switch (type.Name)
            {
                case "Boolean":
                    result = SqlDbType.Bit;

                    break;

                case "Byte":
                    result = SqlDbType.TinyInt;

                    break;

                case "Int16":
                    result = SqlDbType.SmallInt;

                    break;

                case "Int32":
                    result = SqlDbType.SmallInt;

                    break;

                case "Single":
                    result = SqlDbType.Real;

                    break;

                case "Double":
                    result = SqlDbType.Float;

                    break;

                case "String":
                    result = SqlDbType.NChar;

                    break;

                case "Guid":
                    result = SqlDbType.UniqueIdentifier;

                    break;

                case "XmlReader":
                    result = SqlDbType.Xml;

                    break;

                case "Decimal":
                    result = SqlDbType.Money;

                    break;

                case "DateTime":
                    result = SqlDbType.DateTime2;

                    break;

                case "Byte[]":
                    result = SqlDbType.Binary;

                    break;

                case "Object":
                    result = SqlDbType.Variant;

                    break;
            }

            return result;
        }

        /// <summary>
        ///     Converts the NameValueCollection to the Dictionary.
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static Dictionary<string, string[]> ToDictionary(this NameValueCollection pairs)
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            return pairs.AllKeys.ToDictionary(t => t, pairs.GetValues);
        }

        /// <summary>
        ///     Converts the NameValueCollection to the Dictionary.
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary2(this NameValueCollection pairs)
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            return pairs.AllKeys.ToDictionary(t => t, t => pairs.GetValues(t)?.FirstOrDefault());
        }

        /// <summary>
        ///     Converts the NameValueCollection to the ILookup.
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static ILookup<string, string[]> ToLookup(this NameValueCollection pairs)
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            return pairs.AllKeys.ToLookup(t => t, pairs.GetValues);
        }

        /// <summary>
        ///     Converts the NameValueCollection to the ILookup.
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static ILookup<string, string> ToLookup2(this NameValueCollection pairs)
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            return pairs.AllKeys.ToLookup(t => t, t => pairs.GetValues(t)?.FirstOrDefault());
        }

        /// <summary>
        ///     Converts the NameValueCollection to the pair string.
        /// </summary>
        /// <param name="pairs"></param>
        public static string ToNvcString(this NameValueCollection pairs)
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            return string.Join(Environment.NewLine, pairs.AllKeys.SelectMany(pairs.GetValues, (k, v) => k + ": " + v));
        }

        /// <summary>
        ///     Converts the NameValueCollection to the List.
        /// </summary>
        /// <param name="pairs"></param>
        public static List<string> ToList(this NameValueCollection pairs)
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            return pairs.AllKeys.ToList();
        }

        /// <summary>
        ///     Converts the IDictionary to the NameValueCollection.
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection(this IDictionary<string, string[]> pairs)
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            var result = new NameValueCollection();

            foreach (var pair in pairs)
            {
                foreach (var val in pair.Value)
                    result.Add(pair.Key, val);
            }

            return result;
        }

        /// <summary>
        ///     Converts the IDictionary to the NameValueCollection.
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection(this IDictionary<string, string> pairs)
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            var result = new NameValueCollection();

            foreach (var pair in pairs)
                result.Add(pair.Key, pair.Value);

            return result;
        }

        /// <summary>
        ///     Converts the ILookup to the NameValueCollection.
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection(this ILookup<string, string[]> pairs)
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            var result = new NameValueCollection();

            foreach (var pair in pairs)
            {
                foreach (var item in pair.SelectMany(s => s))
                    result.Add(pair.Key, item);
            }

            return result;
        }

        /// <summary>
        ///     Converts the ILookup to the NameValueCollection.
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection(this ILookup<string, string> pairs)
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            var result = new NameValueCollection();

            foreach (var pair in pairs)
                result.Add(pair.Key, pair.FirstOrDefault());

            return result;
        }

        /// <summary>
        ///     Converts the IEnumerable to the NameValueCollection.
        /// </summary>
        /// <param name="pairs"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection(this IEnumerable<string> pairs, string delimiter = ":")
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            var results = new NameValueCollection();

            foreach (var pair in pairs)
            {
                var index = pair.IndexOf(delimiter, StringComparison.Ordinal);

                results.Add(pair.Substring(0, index), pair.Substring(index + 1).Trim());
            }

            return results;
        }

        /// <summary>
        ///     Converts the pair string to the NameValueCollection.
        /// </summary>
        /// <param name="pairs"></param>
        /// <param name="pattern"></param>
        /// <param name="delimiter"></param>
        public static NameValueCollection ToNameValueCollection(string pairs, string pattern = "\r\n", string delimiter = ":")
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            var kvs = Regex.Split(pairs, pattern, RegexOptions.IgnoreCase);

            return ToNameValueCollection(kvs, delimiter);
        }

        /// <summary>
        ///     Converts the pair string to the Dictionary.
        /// </summary>
        /// <param name="pairs"></param>
        /// <param name="pattern"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static Dictionary<string, string[]> ToDictionary(string pairs, string pattern = "\r\n", string delimiter = ":")
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            var nvc = ToNameValueCollection(pairs, pattern, delimiter);

            return ToDictionary(nvc);
        }

        /// <summary>
        ///     Converts the pair string to the Dictionary.
        /// </summary>
        /// <param name="pairs"></param>
        /// <param name="pattern"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary2(string pairs, string pattern = "\r\n", string delimiter = ":")
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            var nvc = ToNameValueCollection(pairs, pattern, delimiter);

            return ToDictionary2(nvc);
        }

        /// <summary>
        ///     Converts the IEnumerable to the Dictionary.
        /// </summary>
        /// <param name="pairs"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static Dictionary<string, string[]> ToDictionary(this IEnumerable<string> pairs, string delimiter = ":")
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            var nvc = ToNameValueCollection(pairs, delimiter);

            return ToDictionary(nvc);
        }

        /// <summary>
        ///     Converts the IEnumerable to the Dictionary.
        /// </summary>
        /// <param name="pairs"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary2(this IEnumerable<string> pairs, string delimiter = ":")
        {
            if (pairs is null)
                throw new ArgumentNullException(nameof(pairs));

            var nvc = ToNameValueCollection(pairs, delimiter);

            return ToDictionary2(nvc);
        }

        /// <summary>
        ///     Whether Type givenType and Type genericType is inheritance.
        /// </summary>
        /// <param name="givenType"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsInheritance(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == genericType))
                return true;

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;

            return baseType != null && IsInheritance(baseType, genericType);
        }
    }
}