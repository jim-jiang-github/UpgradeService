using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Upgrade.Program.Release
{
    /// <summary>
    /// 频率限制器 Debounce（防抖） Throttle（节流）
    /// </summary>
    public class RateLimitor
    {
        #region Private Properties

        private Timer throttleTimer;
        private Timer debounceTimer;
        #endregion

        #region Debounce

        /// <summary>
        /// 防抖       
        /// </summary>
        /// <param name="obj">Your object</param>
        /// <param name="interval">Milisecond interval</param>
        /// <param name="debounceAction">Called when last item call this method and after interval was finished</param>
        public void Debounce(int interval, Action debounceAction)
        {
            debounceTimer?.Dispose();
            debounceTimer = new Timer((state) =>
            {
                debounceTimer?.Dispose();
                if (debounceTimer != null)
                {
                    debounceAction?.Invoke();
                }
                debounceTimer = null;
            }, null, interval, 0);
        }

        #endregion

        #region Throttle

        /// <summary>
        /// 节流
        /// </summary>
        /// <param name="obj">Your object</param>
        /// <param name="interval">Milisecond interval</param>
        /// <param name="throttleAction">Invoked last object when timer ticked invoked</param>
        public void Throttle(int interval, Action throttleAction)
        {
            if (throttleTimer == null)
            {
                throttleTimer = new Timer((state) =>
                {
                    throttleTimer?.Dispose();
                    throttleTimer = null;
                    throttleAction?.Invoke();
                }, null, interval, 0);
            }
        }
        #endregion
    }
}
