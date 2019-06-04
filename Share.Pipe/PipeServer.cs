using Share.Pipe.Delegate;
using System;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Share.Pipe
{
    public class PipeServer
    {
        #region 事件
        /// <summary> 接收到pipe消息事件
        /// </summary>
        public event PipeMessageHandler ReceivePipeData;
        #endregion
        private NamedPipeServerStream namedPipeServerStream = null;
        private string pipeName = string.Empty;
        private byte[] bufferRead = new byte[1024 * 1024];
        /// <summary>
        /// pipe服务器
        /// </summary>
        /// <param name="pipeName">管道名</param>
        public PipeServer(string pipeName)
        {
            this.pipeName = pipeName;
        }
        /// <summary> 启动监听
        /// </summary>
        public void Listen()
        {
            try
            {
                if (this.namedPipeServerStream?.IsConnected ?? false)
                {
                    this.namedPipeServerStream?.Disconnect();
                    this.namedPipeServerStream?.Close();
                    this.namedPipeServerStream?.Dispose();
                }
                this.namedPipeServerStream = new NamedPipeServerStream(this.pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                this.namedPipeServerStream.BeginWaitForConnection((iar) =>
                {
                    try
                    {
                        this.namedPipeServerStream.EndWaitForConnection(iar);
                        this.namedPipeServerStream.BeginRead(bufferRead, 0, bufferRead.Length, new AsyncCallback(Read), this.namedPipeServerStream);
                    }
                    catch (Exception ex)
                    {
                    }
                }, namedPipeServerStream);
            }
            catch (Exception ex)
            {
            }
        }
        private void Read(IAsyncResult iar)
        {
            try
            {
                int readLength = this.namedPipeServerStream.EndRead(iar);
                byte[] bytes = new byte[readLength];
                Array.Copy(bufferRead, 0, bytes, 0, readLength);
                this.ReceivePipeData?.Invoke(bytes);
                this.namedPipeServerStream.Flush();
                this.namedPipeServerStream.BeginRead(bufferRead, 0, bufferRead.Length, new AsyncCallback(Read), bufferRead);
            }
            catch (Exception ex)
            {
                this.Listen();
            }
        }
        public async Task Send(byte[] bytes)
        {
            try
            {
                if (this.namedPipeServerStream?.IsConnected ?? false)
                {
                    await this.namedPipeServerStream.WriteAsync(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
