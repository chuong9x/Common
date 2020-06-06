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
    ///     Extend data utility.
    /// </summary>
    public static class ExtendDataUtil
    {
        /// <summary>
        ///     Sets an entity.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static void SetEntity(this Element elm, string schemaName, params FieldInfo[] fields)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            if (fields is null)
                throw new ArgumentNullException(nameof(fields));

            var guid = Guid.NewGuid();

            var schemaBuilder = new SchemaBuilder(guid);

            schemaBuilder.SetReadAccessLevel(AccessLevel.Public);

            schemaBuilder.SetWriteAccessLevel(AccessLevel.Public);

            schemaBuilder.SetSchemaName(schemaName);

            foreach (var field in fields)
            {
                switch (field.FieldType)
                {
                    case SchemaFieldType.Simple:
                        schemaBuilder.AddSimpleField(field.FieldName, field.DataType);

                        break;

                    case SchemaFieldType.List:
                        schemaBuilder.AddArrayField(field.FieldName, field.DataType);

                        break;

                    case SchemaFieldType.Dictionary:
                        schemaBuilder.AddMapField(field.FieldName, field.KeyType, field.ValueType);

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            elm.SetEntity(new Entity(schemaBuilder.Finish()));
        }

        /// <summary>
        ///     Sets an entity.
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        public static void SetEntity(this Element elm, string schemaName, IEnumerable<FieldInfo> fields)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            if (fields is null)
                throw new ArgumentNullException(nameof(fields));

            var guid = Guid.NewGuid();
            var schemaBuilder = new SchemaBuilder(guid);

            schemaBuilder.SetReadAccessLevel(AccessLevel.Public);
            schemaBuilder.SetWriteAccessLevel(AccessLevel.Public);
            schemaBuilder.SetSchemaName(schemaName);

            foreach (var field in fields)
            {
                switch (field.FieldType)
                {
                    case SchemaFieldType.Simple:
                        schemaBuilder.AddSimpleField(field.FieldName, field.DataType);

                        break;

                    case SchemaFieldType.List:
                        schemaBuilder.AddArrayField(field.FieldName, field.DataType);

                        break;

                    case SchemaFieldType.Dictionary:
                        schemaBuilder.AddMapField(field.FieldName, field.KeyType, field.ValueType);

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            elm.SetEntity(new Entity(schemaBuilder.Finish()));
        }

        /// <summary>
        ///     Gets the simple type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        public static T GetFieldValue<T>(this Element elm, string fieldName, string schemaName) where T : class
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            var schema = elm.GetSchema(schemaName);

            if (schema == null)
                return null;

            var entity = elm.GetEntity(schema);
            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            if (field == null)
                return null;

            if (!typeof(T).IsPrimitive)
            {
                var keyType = field.KeyType;
                var valueType = field.ValueType;

                if (keyType == typeof(int) && valueType == typeof(int))
                    return entity.Get<IDictionary<int, int>>(field) as T;

                if (keyType == typeof(int) && valueType == typeof(double))
                    return entity.Get<IDictionary<int, double>>(field) as T;

                if (keyType == typeof(int) && valueType == typeof(string))
                    return entity.Get<IDictionary<int, string>>(field) as T;

                if (keyType == typeof(double) && valueType == typeof(int))
                    return entity.Get<IDictionary<int, int>>(field) as T;

                if (keyType == typeof(double) && valueType == typeof(double))
                    return entity.Get<IDictionary<double, double>>(field) as T;

                if (keyType == typeof(double) && valueType == typeof(string))
                    return entity.Get<IDictionary<double, string>>(field) as T;

                if (keyType == typeof(string) && valueType == typeof(int))
                    return entity.Get<IDictionary<string, int>>(field) as T;

                if (keyType == typeof(string) && valueType == typeof(double))
                    return entity.Get<IDictionary<string, double>>(field) as T;

                if (keyType == typeof(string) && valueType == typeof(string))
                    return entity.Get<IDictionary<string, string>>(field) as T;
            }

            return entity.Get<T>(field);
        }

        /// <summary>
        ///     Sets the simple type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="schemaName"></param>
        public static void SetFieldValue<T>(this Element elm, string fieldName, T value, string schemaName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            var schema = elm.GetSchema(schemaName);

            if (schema  is null)
                throw new ArgumentNullException(nameof(value));

            var entity = elm.GetEntity(schema);

            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            if (!typeof(T).IsPrimitive)
            {
                var keyType = field.KeyType;

                var valueType = field.ValueType;

                if (keyType == typeof(int) && valueType == typeof(int))
                    entity.Set(fieldName, value as IDictionary<int, int>);

                if (keyType == typeof(int) && valueType == typeof(double))
                    entity.Set(fieldName, value as IDictionary<int, double>);

                if (keyType == typeof(int) && valueType == typeof(string))
                    entity.Set(fieldName, value as IDictionary<int, string>);

                if (keyType == typeof(double) && valueType == typeof(int))
                    entity.Set(fieldName, value as IDictionary<double, int>);

                if (keyType == typeof(double) && valueType == typeof(double))
                    entity.Set(fieldName, value as IDictionary<double, double>);

                if (keyType == typeof(double) && valueType == typeof(string))
                    entity.Set(fieldName, value as IDictionary<double, string>);

                if (keyType == typeof(string) && valueType == typeof(int))
                    entity.Set(fieldName, value as IDictionary<string, int>);

                if (keyType == typeof(string) && valueType == typeof(double))
                    entity.Set(fieldName, value as IDictionary<string, double>);

                if (keyType == typeof(string) && valueType == typeof(string))
                    entity.Set(fieldName, value as IDictionary<string, string>);
            }

            else
                entity.Set(fieldName, value);

            elm.SetEntity(entity);
        }

        /// <summary>
        ///     Gets element's schema by name.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        public static Schema GetSchema(this Element elm, string schemaName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            var guids = elm.GetEntitySchemaGuids();

            return guids.Select(Schema.Lookup).FirstOrDefault(f => f.SchemaName == schemaName);
        }
    }
}