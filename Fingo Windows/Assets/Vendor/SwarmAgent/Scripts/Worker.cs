using UnityEngine;
using System.Threading;

namespace Swarm
{

    public class Worker<T>
    {
        Thread thread;
        AutoResetEvent start, done;
        bool die = false;
        System.Action<int, int> task;
        int offset, count;

        public Worker()
        {
            start = new AutoResetEvent(false);
            done = new AutoResetEvent(false);
            thread = new Thread(WorkerFunction);
            thread.Start();
        }

        public void Terminate()
        {
            die = true;
            if (!this.thread.Join(100))
                thread.Abort();
        }

        public void Run(System.Action<int, int> task, int offset, int count)
        {
            if (this.task != null)
            {
                throw new System.InvalidOperationException();
            }
            this.task = task;
            this.offset = offset;
            this.count = count;
            start.Set();
        }

        public bool Wait(int timeout = -1)
        {
            if (timeout > 0)
                return done.WaitOne(timeout);
            return done.WaitOne();
        }

        void WorkerFunction()
        {
            while (!die)
            {
                if (start.WaitOne(10))
                {
                    if (task == null) continue;
                    var localTask = task;
                    task = null;
                    localTask(offset, count);
                    localTask = null;
                    done.Set();
                }
            }
        }

    }

}

