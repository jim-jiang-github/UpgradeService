using Share.Pipe.Delegate;
using System;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Share.Pipe
{
    public class PipeClient
    {
        #region 事件
        /// <summary> 接收到pipe消息事件
        /// </summary>
        public event PipeMessageHandler ReceivePipeData;
        #endregion
        private NamedPipeClientStream namedPipeClientStream = null;
        private string pipeName = string.Empty;
        private byte[] bufferRead = new byte[1024 * 1024];
        public PipeClient(string pipeName)
        {
            this.pipeName = pipeName;
        }
        public async Task<bool> Connect(int timeOut = 1000)
        {
            if (this.namedPipeClientStream?.IsConnected ?? false) { return true; }
            try
            {
                this.namedPipeClientStream = new NamedPipeClientStream(".", this.pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
                await this.namedPipeClientStream.ConnectAsync(timeOut);
                this.namedPipeClientStream.BeginRead(bufferRead, 0, bufferRead.Length, new AsyncCallback(Read), this.namedPipeClientStream);
                return true;
            }
            catch (TimeoutException tex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task Send(byte[] bytes)
        {
            try
            {
                if (this.namedPipeClientStream?.IsConnected ?? false)
                {
                    await this.namedPipeClientStream.WriteAsync(bytes, 0, bytes.Length);
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("pipeSendError");
            }
        }
        private void Read(IAsyncResult iar)
        {
            try
            {
                if (this.namedPipeClientStream?.IsConnected ?? false)
                {
                    int readLength = this.namedPipeClientStream.EndRead(iar);
                    if (readLength != 0)
                    {
                        byte[] bytes = new byte[readLength];
                        Array.Copy(bufferRead, 0, bytes, 0, readLength);
                        this.ReceivePipeData?.Invoke(bytes);
                    }
                    this.namedPipeClientStream.Flush();
                    this.namedPipeClientStream.BeginRead(bufferRead, 0, bufferRead.Length, new AsyncCallback(Read), bufferRead);
                }
            }
            catch (Exception oEX)
            {
            }
        }
    }
}
