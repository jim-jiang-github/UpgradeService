using FileTransfe.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileTransfe.Entities
{
    [Serializable]
    public class DownloadFile
    {
        public string URL { get; set; }
        public long RangeBegin { get; set; }
        public long? Length { get; set; }
        public string MD5 { get; set; }

        /// <summary> 吧对象转为byte[]
        /// </summary>
        public byte[] ToBytes()
        {
            return SerializeTools.ObjectToBytes(this);
        }
        /// <summary> 从byte加载对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static DownloadFile FromByte(byte[] bytes)
        {
            return SerializeTools.BytesToObject(bytes) as DownloadFile;
        }
        /// <summary> 从文件流加载对象
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static DownloadFile FromFileStream(FileStream fileStream)
        {
            try
            {
                byte[] lengthBytes = new byte[4];
                fileStream.Position = fileStream.Length - 4;
                fileStream.Read(lengthBytes, 0, 4);
                int length = BitConverter.ToInt32(lengthBytes, 0);
                fileStream.Position = fileStream.Length - 4 - length;
                byte[] objBytes = new byte[length];
                fileStream.Read(objBytes, 0, length);
                fileStream.Position = fileStream.Length - 4 - length;
                return DownloadFile.FromByte(objBytes);
            }
            catch { return null; }
        }
        /// <summary> 吧对象追加到文件流的结尾
        /// </summary>
        /// <param name="fileStream"></param>
        public int AppendFileStream(FileStream fileStream)
        {
            byte[] bytes = this.ToBytes();
            int length = bytes.Length;
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Write(BitConverter.GetBytes(length), 0, 4);
            return length + 4;
        }
    }
}
