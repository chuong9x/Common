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
using System.IO;
using System.Security.Cryptography;

namespace KeLi.Common.Tool.Security
{
    /// <summary>
    ///     AES encrypt.
    /// </summary>
    public class AesEncrypt
    {
        /// <summary>
        ///     The secret key.
        /// </summary>
        private static readonly byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        ///     The vectors
        /// </summary>
        private static readonly byte[] Ivs = { 0xEF, 0xCD, 0xAB, 0x90, 0x78, 0x56, 0x34, 0x12 };

        /// <summary>
        ///     Encrypts the content.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="keys"></param>
        /// <param name="ivs"></param>
        /// <returns></returns>
        public static string Encrypt(string content, byte[] keys = null, byte[] ivs = null)
        {
            if (content is null)
                throw new ArgumentNullException(nameof(content));

            if (string.IsNullOrWhiteSpace(content))
                return null;

            if (keys is null || keys.Length == 0)
                keys = Keys;

            if (ivs is null || ivs.Length == 0)
                ivs = Ivs;

            var dcsp = new DESCryptoServiceProvider();

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, dcsp.CreateEncryptor(keys, ivs), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(content);

                sw.Flush();

                cs.FlushFinalBlock();

                ms.Flush();

                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }

        /// <summary>
        ///     Decrypts the ciphertext.
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="keys"></param>
        /// <param name="ivs"></param>
        /// <returns></returns>
        public static string Decrypt(string ciphertext, byte[] keys = null, byte[] ivs = null)
        {
            if (ciphertext is null)
                throw new ArgumentNullException(nameof(ciphertext));

            if (string.IsNullOrWhiteSpace(ciphertext))
                return null;

            if (keys is null || keys.Length == 0)
                keys = Keys;

            if (ivs is null || ivs.Length == 0)
                ivs = Ivs;

            var dcsp = new DESCryptoServiceProvider();
            var context = Convert.FromBase64String(ciphertext);

            using (var ms = new MemoryStream(context))
            using (var cs = new CryptoStream(ms, dcsp.CreateDecryptor(keys, ivs), CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}