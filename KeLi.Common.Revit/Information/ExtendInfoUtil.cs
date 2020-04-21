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

using KeLi.Common.Revit.Widgets;

using static Autodesk.Revit.DB.ExtensibleStorage.AccessLevel;

namespace KeLi.Common.Revit.Information
{
    /// <summary>
    ///     Extend info utility.
    /// </summary>
    public static class ExtendInfoUtil
    {
        /// <summary>
        ///     Creates a schema builder by schema name.
        /// </summary>
        /// <returns></returns>
        public static SchemaBuilder CreateSchemaBuilder()
        {
            var guid = Guid.NewGuid();
            var result = new SchemaBuilder(guid);

            result.SetReadAccessLevel(Public);

            result.SetWriteAccessLevel(Public);

            result.SetSchemaName("Schema" + guid.ToString().Substring(0, 6));

            result.SetDocumentation(guid.ToString());

            return result;
        }

        /// <summary>
        ///     Creates a schema builder by schema name.
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="dcrp"></param>
        /// <returns></returns>
        public static SchemaBuilder CreateSchemaBuilder(string schemaName, string dcrp = null)
        {
            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            var result = new SchemaBuilder(Guid.NewGuid());

            result.SetReadAccessLevel(Public);

            result.SetWriteAccessLevel(Public);

            result.SetSchemaName(schemaName);

            if (dcrp is null)
                dcrp = schemaName;

            result.SetDocumentation(dcrp);

            return result;
        }

        /// <summary>
        ///     Creates a simple type field builder by field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schemaBuilder"></param>
        /// <param name="fieldName"></param>
        /// <param name="unitType"></param>
        /// <param name="dcrp"></param>
        /// <returns></returns>
        public static void AddSimpleField<T>(this SchemaBuilder schemaBuilder, string fieldName, UnitType unitType, string dcrp = null)
        {
            if (schemaBuilder is null)
                throw new ArgumentNullException(nameof(schemaBuilder));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var result = schemaBuilder.AddSimpleField(fieldName, typeof(T));

            result.SetUnitType(unitType);

            if (dcrp is null)
                dcrp = fieldName;

            result.SetDocumentation(dcrp);
        }

        /// <summary>
        ///     Creates an IEnumerable type field builder by field name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schemaBuilder"></param>
        /// <param name="fieldName"></param>
        /// <param name="unitType"></param>
        /// <param name="dcrp"></param>
        /// <returns></returns>
        public static void AddListField<T>(this SchemaBuilder schemaBuilder, string fieldName, UnitType unitType, string dcrp = null)
        {
            if (schemaBuilder is null)
                throw new ArgumentNullException(nameof(schemaBuilder));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (dcrp is null)
                throw new ArgumentNullException(nameof(dcrp));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var result = schemaBuilder.AddArrayField(fieldName, typeof(IEnumerable<T>));

            result.SetUnitType(unitType);

            result.SetDocumentation(dcrp);
        }

        /// <summary>
        ///     Creates an IDictionary type field builder by field name.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="schemaBuilder"></param>
        /// <param name="fieldName"></param>
        /// <param name="unitType"></param>
        /// <param name="dcrp"></param>
        /// <returns></returns>
        public static void AddDictField<K, V>(this SchemaBuilder schemaBuilder, string fieldName, UnitType unitType, string dcrp = null)
        {
            if (schemaBuilder is null)
                throw new ArgumentNullException(nameof(schemaBuilder));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (!typeof(K).IsPrimitive)
                throw new NotSupportedException(nameof(K));

            if (!typeof(V).IsPrimitive)
                throw new NotSupportedException(nameof(V));

            var result = schemaBuilder.AddMapField(fieldName, typeof(K), typeof(V));

            result.SetUnitType(unitType);

            result.SetDocumentation(dcrp);
        }

        /// <summary>
        ///     Inits the entity by simple field and value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Entity InitSimpleFieldEntity<T>(string fieldName, T value)
        {
            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var schemaBuilder = CreateSchemaBuilder();

            schemaBuilder.AddSimpleField(fieldName, typeof(T));

            var schema = schemaBuilder.Finish();

            var result = new Entity(schema);

            result.SetSimpleFieldValue(fieldName, value);

            return result;
        }

        /// <summary>
        ///     Inits the entity by IEnumerable field and value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Entity InitListFieldEntity<T>(string fieldName, IEnumerable<T> value)
        {
            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var schemaBuilder = CreateSchemaBuilder();

            schemaBuilder.AddArrayField(fieldName, typeof(T));

            var schema = schemaBuilder.Finish();

            var result = new Entity(schema);

            result.SetListFieldValue(fieldName, value);

            return result;
        }

        /// <summary>
        ///     Inits the entity by IDictionary field and value.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Entity InitDictFieldEntity<K, V>(string fieldName, IDictionary<K, V> value)
        {
            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(K).IsPrimitive)
                throw new NotSupportedException(nameof(K));

            if (!typeof(V).IsPrimitive)
                throw new NotSupportedException(nameof(V));

            var schemaBuilder = CreateSchemaBuilder();

            schemaBuilder.AddMapField(fieldName, typeof(K), typeof(V));

            var schema = schemaBuilder.Finish();

            var result = new Entity(schema);

            result.SetDictFieldValue(fieldName, value);

            return result;
        }

        /// <summary>
        ///     Binds the entity by simple field and value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static void BindSimpleFieldEntity<T>(this Element elm, string fieldName, T value)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var entity = InitSimpleFieldEntity(fieldName, value);

            elm.Document.AutoTransaction(() => elm.SetEntity(entity));
        }

        /// <summary>
        ///     Binds the entity by IEnumerable field and value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static void BindListFieldEntity<T>(this Element elm, string fieldName, IEnumerable<T> value)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var entity = InitListFieldEntity(fieldName, value);

            elm.Document.AutoTransaction(() => elm.SetEntity(entity));
        }

        /// <summary>
        ///     Binds element entity by IDictionary field and value.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="elm"></param>
        /// <returns></returns>
        public static void BindDictFieldEntity<K, V>(this Element elm, string fieldName, IDictionary<K, V> value)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(K).IsPrimitive)
                throw new NotSupportedException(nameof(K));

            if (!typeof(V).IsPrimitive)
                throw new NotSupportedException(nameof(V));

            var entity = InitDictFieldEntity(fieldName, value);

            elm.Document.AutoTransaction(() => elm.SetEntity(entity));
        }

        /// <summary>
        ///     Sets the simple type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetSimpleFieldValue<T>(this Entity entity, string fieldName, T value)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            entity.Set(fieldName, value);
        }

        /// <summary>
        ///     Sets the simple type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetSimpleFieldValue<T>(this Element elm, string fieldName, T value)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var schema = elm.GetSchemaByFieldName(fieldName);

            var entity = elm.GetEntity(schema);

            entity.Set(fieldName, value);

            elm.Document.AutoTransaction(() => elm.SetEntity(entity));
        }

        /// <summary>
        ///     Sets the IEnumerable type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetListFieldValue<T>(this Entity entity, string fieldName, IEnumerable<T> value)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            entity.Set(fieldName, value);
        }

        /// <summary>
        ///     Sets the IEnumerable type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetListFieldValue<T>(this Element elm, string fieldName, IEnumerable<T> value)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var schema = elm.GetSchemaByFieldName(fieldName);

            var entity = elm.GetEntity(schema);

            entity.Set(fieldName, value);

            elm.Document.AutoTransaction(() => elm.SetEntity(entity));
        }

        /// <summary>
        ///     Sets the IDictionary type field's value.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetDictFieldValue<K, V>(this Entity entity, string fieldName, IDictionary<K, V> value)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(K).IsPrimitive)
                throw new NotSupportedException(nameof(K));

            if (!typeof(V).IsPrimitive)
                throw new NotSupportedException(nameof(V));

            entity.Set(fieldName, value);
        }

        /// <summary>
        ///     Sets the IDictionary type field's value.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetDictFieldValue<K, V>(this Element elm, string fieldName, IDictionary<K, V> value)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (!typeof(K).IsPrimitive)
                throw new NotSupportedException(nameof(K));

            if (!typeof(V).IsPrimitive)
                throw new NotSupportedException(nameof(V));

            var schema = elm.GetSchemaByFieldName(fieldName);

            var entity = elm.GetEntity(schema);

            entity.Set(fieldName, value);

            elm.Document.AutoTransaction(() => elm.SetEntity(entity));
        }

        /// <summary>
        ///     Gets the simple type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T GetSimpleFieldValue<T>(this Element elm, string fieldName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var schema = GetSchemaByFieldName(elm, fieldName);

            var entity = elm.GetEntity(schema);

            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<T>(field) : default;
        }

        /// <summary>
        ///     Gets the simple type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T GetSimpleFieldValue<T>(this Element elm, string schemaName, string fieldName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var schema = GetSchemaBySchemaName(elm, schemaName);

            var entity = elm.GetEntity(schema);

            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<T>(field) : default;
        }

        /// <summary>
        ///     Gets the IEnumerable type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetListFieldValue<T>(this Element elm, string fieldName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var schema = elm.GetSchemaByFieldName(fieldName);

            var entity = elm.GetEntity(schema);

            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<IEnumerable<T>>(field) : default;
        }

        /// <summary>
        ///     Gets the IEnumerable type field's value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetListFieldValue<T>(this Element elm, string schemaName, string fieldName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (!typeof(T).IsPrimitive)
                throw new NotSupportedException(nameof(T));

            var schema = elm.GetSchemaBySchemaName(schemaName);

            var entity = elm.GetEntity(schema);

            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<IEnumerable<T>>(field) : default;
        }

        /// <summary>
        ///     Gets the IDictionary field's value.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static IDictionary<K, V> GetDictFieldValue<K, V>(this Element elm, string fieldName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (!typeof(K).IsPrimitive)
                throw new NotSupportedException(nameof(K));

            if (!typeof(V).IsPrimitive)
                throw new NotSupportedException(nameof(V));

            var schema = elm.GetSchemaByFieldName(fieldName);

            var entity = elm.GetEntity(schema);

            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<IDictionary<K, V>>(field) : default;
        }

        /// <summary>
        ///     Gets the IDictionary field's value.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static IDictionary<K, V> GetDictFieldValue<K, V>(this Element elm, string schemaName, string fieldName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            if (!typeof(K).IsPrimitive)
                throw new NotSupportedException(nameof(K));

            if (!typeof(V).IsPrimitive)
                throw new NotSupportedException(nameof(V));

            var schema = elm.GetSchemaBySchemaName(schemaName);

            var entity = elm.GetEntity(schema);

            var field = schema.ListFields().FirstOrDefault(f => f.FieldName == fieldName);

            return field != null ? entity.Get<IDictionary<K, V>>(field) : default;
        }

        /// <summary>
        ///     Gets the element's schema by schema name.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        public static Schema GetSchemaBySchemaName(this Element elm, string schemaName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (schemaName is null)
                throw new ArgumentNullException(nameof(schemaName));

            var guids = elm.GetEntitySchemaGuids();

            return guids.Select(Schema.Lookup).FirstOrDefault(f => f.SchemaName == schemaName);
        }

        /// <summary>
        ///     Gets the element's schema by field name.
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Schema GetSchemaByFieldName(this Element elm, string fieldName)
        {
            if (elm is null)
                throw new ArgumentNullException(nameof(elm));

            if (fieldName is null)
                throw new ArgumentNullException(nameof(fieldName));

            var guids = elm.GetEntitySchemaGuids();

            return guids.Select(Schema.Lookup).FirstOrDefault(f => f.GetField(fieldName) != null);
        }
    }
}