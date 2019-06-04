using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Transfe.Http.File.Tools
{
    /// <summary> MD5工具类
    /// </summary>
    public static class MD5Tools
    {
        /// <summary> 获取文件的MD5特征码
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileMd5(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return GetStreamMd5(fs);
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }
        /// <summary> 获取文件的MD5特征码
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static byte[] GetFileMd5Bytes(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return GetStreamMd5Bytes(fs);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary> 获取流的MD5特征码
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetStreamMd5(Stream stream)
        {
            try
            {
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    return Md5BytesToMd5(md5.ComputeHash(stream));
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }
        /// <summary> 获取流的MD5特征码
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static byte[] GetStreamMd5Bytes(Stream stream)
        {
            try
            {
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    return md5.ComputeHash(stream);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /** * 16进制字符集 */

        private static char[] HEX_DIGITS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        /** * 将单个字节码转换成16进制字符串 * @param bt 目标字节 * @return 转换结果 */

        public static String byteToHex(byte bt)
        {

            return HEX_DIGITS[(bt & 0xf0) >> 4] + "" + HEX_DIGITS[bt & 0xf];

        }
        public static string bytesToHex(byte[] bytes)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {

                sb.Append(byteToHex(bytes[i]));

            }

            return sb.ToString();

        }
        /// <summary> byte[]转MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetBytesMd5(byte[] data)
        {
            try
            {
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    return Md5BytesToMd5(md5.ComputeHash(data));
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }
        /// <summary> Md5的byte[]转MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5BytesToMd5(byte[] data)
        {
            try
            {
                return BitConverter.ToString(data).Replace("-", "");
            }
            catch (Exception e)
            {
                return "";
            }
        }
        /// <summary> 字符串转MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStringMd5(string str, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(str);
            try
            {
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    return Md5BytesToMd5(md5.ComputeHash(data));
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }
        /// <summary> 字符串转MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetStringMd5(string str)
        {
            return GetStringMd5(str, Encoding.GetEncoding("GB2312"));
        }
    }
}
