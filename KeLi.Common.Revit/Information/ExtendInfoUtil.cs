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
     |  |              Creation Time: 12/17/2019 01:08:41 PM |  |  |     |         |      |
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
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace KeLi.Common.Revit.Information
{
    /// <summary>
    /// Extend info utility.
    /// </summary>
    public static class ExtendInfoUtil
    {
        /// <summary>
        /// Creates a schema builder by schema name.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="decription"></param>
        /// <returns></returns>
        public static SchemaBuilder CreateSchemaBuilder(string schemaName, string decription = null)
        {
            var result = new SchemaBuilder(Guid.NewGuid());

            result.SetReadAccessLevel(AccessLevel.Public);
            result.SetWriteAccessLevel(AccessLevel.Public);
            result.SetSchemaName(schemaName);

            if (decription == null)
                decription = schemaName;

            result.SetDocumentation(decription);

            return result;
        }

        /// <summary>
        /// Creates a simple type field builder by field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schemaBuilder"></param>
        /// <param name="fieldName"></param>
        /// <param name="unitType"></param>
        /// <param name="decription"></param>
        /// <returns></returns>
        public static FieldBuilder CreateSimpleField<T>(this SchemaBuilder schemaBuilder, string fieldName, UnitType unitType, string decription = null)
        {
            var result = schemaBuilder.AddSimpleField(fieldName, typeof(T));

            result.SetUnitType(unitType);

            if (decription == null)
                decription = fieldName;

            result.SetDocumentation(decription);

            return result;
        }

        /// <summary>
        /// Creates an array type field builder by field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schemaBuilder"></param>
        /// <param name="fieldName"></param>
        /// <param name="unitType"></param>
        /// <param name="decription"></param>
        /// <returns></returns>
        public static FieldBuilder CreateArrayField<T>(this SchemaBuilder schemaBuilder, string fieldName, UnitType unitType, string decription = null)
        {
            var result = schemaBuilder.AddArrayField(fieldName, typeof(T));

            result.SetUnitType(unitType);

            if (decription == null)
                decription = fieldName;

            result.SetDocumentation(decription);

            return result;
        }

        /// <summary>
        /// Creates an dictionary type field builder by field name.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="schemaBuilder"></param>
        /// <param name="fieldName"></param>
        /// <param name="unitType"></param>
        /// <param name="decription"></param>
        /// <returns></returns>
        public static FieldBuilder CreateDictField<K, V>(this SchemaBuilder schemaBuilder, string fieldName, UnitType unitType, string decription = null)
        {
            var result = schemaBuilder.AddMapField(fieldName, typeof(K), typeof(V));

            result.SetUnitType(unitType);

            if (decription == null)
                decription = fieldName;

            result.SetDocumentation(decription);

            return result;
        }

        /// <summary>
        /// Creates an entity by schema builder.
        /// </summary>
        /// <param name="schemaBuilder"></param>
        /// <returns></returns>
        public static Entity CreateEntity(this SchemaBuilder schemaBuilder)
        {
            var schema = schemaBuilder.Finish();

            return new Entity(schema);
        }

        /// <summary>
        /// Sets the simple type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetSimpleFieldValue<T>(this Entity entity, string fieldName, T value)
        {
            entity.Set(fieldName, value);
        }

        /// <summary>
        /// Sets the array type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetFieldListValue<T>(this Entity entity, string fieldName, IEnumerable<T> value)
        {
            entity.Set(fieldName, value);
        }

        /// <summary>
        /// Sets the dictionary type field's value.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetDictFieldValue<K, V>(this Entity entity, string fieldName, IDictionary<K, V> value)
        {
            entity.Set(fieldName, value);
        }

        /// <summary>
        /// Gets the dictionary type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T GetSimpleFieldValue<T>(this Element elm, string schemaName, string fieldName)
        {
            var schema = GetSchema(elm, schemaName);
            var entity = elm.GetEntity(schema);
            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<T>(field) : default;
        }

        /// <summary>
        /// Gets the array type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T[] GetArrayFieldValue<T>(this Element elm, string schemaName, string fieldName)
        {
            var schema = GetSchema(elm, schemaName);
            var entity = elm.GetEntity(schema);
            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<T[]>(field) : default;
        }

        /// <summary>
        /// Gets the list type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static List<T> GetListFieldValue<T>(this Element elm, string schemaName, string fieldName)
        {
            var schema = GetSchema(elm, schemaName);
            var entity = elm.GetEntity(schema);
            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<List<T>>(field) : default;
        }

        /// <summary>
        /// Gets the dictionary field's value.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Dictionary<K, V> GetDictFieldValue<K, V>(this Element elm, string schemaName, string fieldName)
        {
            var schema = GetSchema(elm, schemaName);
            var entity = elm.GetEntity(schema);
            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<Dictionary<K, V>>(field) : default;
        }

        /// <summary>
        /// Gets the element's schema by schema name.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        public static Schema GetSchema(this Element elm, string schemaName)
        {
            if (string.IsNullOrWhiteSpace(schemaName))
                throw new ArgumentNullException(nameof(schemaName));

            var guids = elm.GetEntitySchemaGuids();

            return guids.Select(Schema.Lookup).FirstOrDefault(schema => schema.SchemaName == schemaName);
        }
    }
}
