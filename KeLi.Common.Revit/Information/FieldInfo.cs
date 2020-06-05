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
     |  |              Creation Time: 06/06/2020 01:08:00 AM |  |  |     |         |      |
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

namespace KeLi.Common.Revit.Information
{
    /// <summary>
    ///     The field info of element's schema.
    /// </summary>
    public class FieldInfo
    {
        /// <summary>
        ///     Inits list type or simple type field info.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="dataType"></param>
        /// <param name="fieldType"></param>
        public FieldInfo(string fieldName, Type dataType, SchemaFieldType fieldType)
        {
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));

            DataType = dataType ?? throw new ArgumentNullException(nameof(dataType));

            FieldType = fieldType;
        }

        /// <summary>
        ///     Inits dictionary type field info.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        public FieldInfo(string fieldName, Type keyType, Type valueType)
        {
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));

            KeyType = keyType ?? throw new ArgumentNullException(nameof(keyType));

            ValueType = valueType ?? throw new ArgumentNullException(nameof(valueType));

            FieldType = SchemaFieldType.Dictionary;
        }

        /// <summary>
        ///     Name of the field.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        ///     Data type of simple or list field.
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        ///     Key type of dictionary field.
        /// </summary>
        public Type KeyType { get; set; }

        /// <summary>
        ///     Value type of dictionary field.
        /// </summary>
        public Type ValueType { get; set; }

        /// <summary>
        ///     SchemaFieldType of the field.
        /// </summary>
        public SchemaFieldType FieldType { get; set; }
    }
}