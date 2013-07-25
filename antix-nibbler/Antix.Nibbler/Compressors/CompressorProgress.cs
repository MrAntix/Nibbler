using System;

namespace Antix.Nibbler.Tools
{
    public class CompressorProgress
    {
        public Action<string, int> Change { get; set; }
        public event EventHandler Cancelled;

        public virtual void Cancel()
        {
            if (Cancelled != null) 
                Cancelled(this, EventArgs.Empty);
        }
    }
}