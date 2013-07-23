using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace Antix.Nibbler.Diagnostics
{
    public class AsyncProcessProgress
    {
        readonly ObservableCollection<string> _output;
        readonly ObservableCollection<string> _error;
        readonly CancellationTokenSource _cancellationTokenSource;

        public AsyncProcessProgress()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _output = new ObservableCollection<string>();
            _error = new ObservableCollection<string>();
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        public void RegisterCancel(Action action)
        {
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
            _cancellationTokenSource.Token.Register(action);
        }

        public ObservableCollection<string> Output
        {
            get { return _output; }
        }

        public ObservableCollection<string> Error
        {
            get { return _error; }
        }

    }
}