using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Transfe.Server.File.Entities
{
    public class PartialFileStream : Stream
    {
        #region 变量
        private long from;
        private long to;
        private long position;
        private FileStream fileStream;
        #endregion

        public override bool CanRead => this.fileStream.CanRead;

        public override bool CanSeek => this.fileStream.CanSeek;

        public override bool CanWrite => this.fileStream.CanWrite;

        public override long Length => this.to - this.from + 1;

        public override long Position
        {
            get => this.position;
            set
            {
                this.position = value;
                this.fileStream.Seek(this.position, SeekOrigin.Begin);
            }
        }
        public PartialFileStream(string filePath, long from, long to)
        {
            this.from = from;
            this.position = from;
            this.to = to;
            this.fileStream =System.IO. File.OpenRead(filePath);
            if (from > 0)
            {
                this.fileStream.Seek(from, SeekOrigin.Begin);
            }
        }
        public override void Flush() => this.fileStream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            int byteCountToRead = count;
            if (this.position + count > this.to)
            {
                byteCountToRead = (int)(this.to - this.position) + 1;
            }
            var result = this.fileStream.Read(buffer, offset, byteCountToRead);
            this.position += byteCountToRead;
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                this.position = this.from + offset;
                return this.fileStream.Seek(this.from + offset, origin);
            }
            else if (origin == SeekOrigin.Current)
            {
                this.position += offset;
                return this.fileStream.Seek(this.position + offset, origin);
            }
            else
            {
                throw new NotImplementedException("SeekOrigin.End未实现");
            }
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            int byteCountToRead = count;
            if (this.position + count > this.to)
            {
                byteCountToRead = (int)(this.to - this.position);
            }
            var result = this.fileStream.BeginRead(buffer, offset, count, (s) =>
            {
                this.position += byteCountToRead;
                callback(s);
            }, state);
            return result;
        }
        public override int EndRead(IAsyncResult asyncResult)
        {
            return this.fileStream.EndRead(asyncResult);
        }
        public override int ReadByte()
        {
            int result = this.fileStream.ReadByte();
            this.position++;
            return result;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.fileStream.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
