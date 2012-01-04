using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace UniTTT.Logik
{
    public class Logger : IDisposable
    {
        public Logger(IOutputDarsteller odar)
        {
            ODar = odar;
            LoggThread = new Thread(DoWork);
        }

        public IOutputDarsteller ODar { get; private set; }
        public bool HasStarted { get; private set; }
        public bool HasStoped { get; private set; }
        public Thread LoggThread { get; private set; }

        public void Start()
        {
            HasStarted = true;
            if (LoggThread.ThreadState != System.Threading.ThreadState.Running)
            {
                LoggThread.Start();
            }
        }

        public void Stop()
        {
            HasStoped = true;
            HasStarted = false;
        }

        private long MaxMemorySizeInMB = 0;
        private TimeSpan NeededTime;
        public void DoWork()
        {
            Stopwatch ST = new Stopwatch();
            ST.Start();
            do
            {
                if (HasStarted)
                {
                    long CurrentMemorySize = Process.GetCurrentProcess().WorkingSet64;
                    CurrentMemorySize /= 1000000;

                    if (CurrentMemorySize > MaxMemorySizeInMB)
                    {
                        MaxMemorySizeInMB = CurrentMemorySize;
                    }
                }
            } while (HasStarted);
            ST.Stop();
            NeededTime += ST.Elapsed;
        }

        public void Save()
        {
            BinaryWriter binwriter = new BinaryWriter(File.OpenWrite("Log"), Encoding.UTF8);
            binwriter.Write("Maximaler RAM Verbrauch (in MB): " + MaxMemorySizeInMB);
            binwriter.Write("Ausführungsdauer: " + NeededTime);
            binwriter.Close();
        }

        public void Dispose()
        {
            Stop();
            Save();
        }
    }
}
