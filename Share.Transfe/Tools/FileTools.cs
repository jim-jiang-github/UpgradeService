using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Share.Transfe.Tools
{
    public static class FileTools
    {
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteFile(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(5000);
                    return await Task<bool>.Run(() =>
                    {
                        while (System.IO.File.Exists(filePath) && !cancellationTokenSource.IsCancellationRequested)
                        {
                            Task.Delay(100);
                        }
                        return !cancellationTokenSource.IsCancellationRequested;
                    }, cancellationTokenSource.Token);
                }
                else
                {
                    return true;
                }
            }
            catch { return false; }
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<bool> MoveFile(string fromFilePath, string toFilePath)
        {
            try
            {
                if (await DeleteFile(toFilePath))
                {
                    string dir = System.IO.Path.GetDirectoryName(toFilePath);
                    if (!System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }
                    System.IO.File.Move(fromFilePath, toFilePath);
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(5000);
                    return await Task<bool>.Run(() =>
                    {
                        while (!System.IO.File.Exists(toFilePath) && !cancellationTokenSource.IsCancellationRequested)
                        {
                            Task.Delay(100);
                        }
                        return !cancellationTokenSource.IsCancellationRequested;
                    }, cancellationTokenSource.Token);
                }
                return false;
            }
            catch { return false; }
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteDirectory(string dir)
        {
            try
            {
                System.IO.Directory.Delete(dir, true);
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(5000);
                return await Task<bool>.Run(() =>
                {
                    while (System.IO.Directory.Exists(dir) && !cancellationTokenSource.IsCancellationRequested)
                    {
                        Task.Delay(100);
                    }
                    return !cancellationTokenSource.IsCancellationRequested;
                }, cancellationTokenSource.Token);
            }
            catch { return false; }
        }

    }
}
