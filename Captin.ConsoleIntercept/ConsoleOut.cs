using System;
using System.IO;

namespace Captin.ConsoleIntercept
{
    /// <summary>
    /// Contains methods to begin capturing <see cref="Console.Out"/> into a variable.
    /// </summary>
    public static class ConsoleOut
    {
        private static ConsoleOutProxyWriter writerNotifierOut;
        private static ConsoleOutProxyWriter writerNotifierErr;
        private static readonly TextWriter consoleOut = Console.Out;
        private static readonly TextWriter consoleErr = Console.Error;

        /// <summary>
        /// Start observing changes to <see cref="Console.Out"/>.
        ///
        /// <para>This leaves the original console out intact.</para>
        /// </summary>
        /// <returns></returns>
        public static Observer Observe()
        {
            InitNotifier();
            var subscription = writerNotifierOut.Subscribe(new StringWriter());
            return subscription;
        }

        /// <summary>
        /// Start observing changes to <see cref="Console.Error"/>.
        ///
        /// <para>This leaves the original console out intact.</para>
        /// </summary>
        /// <returns></returns>
        public static Observer ObserveError()
        {
            InitNotifierErr();
            var subscription = writerNotifierErr.Subscribe(new StringWriter());
            return subscription;
        }

        private static void InitNotifier()
        {
            if (writerNotifierOut == null)
            {
                writerNotifierOut = new ConsoleOutProxyWriter(consoleOut);
                writerNotifierOut.OnObserversChanged += (sender, activeObservers) =>
                {
                    if (activeObservers == 0)
                    {
                        Console.SetOut(consoleOut);
                    }
                    else
                    {
                        Console.SetOut(writerNotifierOut);
                    }
                };
            }
        }

        private static void InitNotifierErr()
        {
            if (writerNotifierErr == null)
            {
                writerNotifierErr = new ConsoleOutProxyWriter(consoleErr);
                writerNotifierErr.OnObserversChanged += (sender, activeObservers) =>
                {
                    if (activeObservers == 0)
                    {
                        Console.SetError(consoleErr);
                    }
                    else
                    {
                        Console.SetError(writerNotifierErr);
                    }
                };
            }
        }
    }

}