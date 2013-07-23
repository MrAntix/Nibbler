using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Antix.Nibbler.Diagnostics
{
    public class AsyncProcessResults
    {
        readonly Process _process;
        readonly IEnumerable<string> _standardOutput;
        readonly IEnumerable<string> _standardError;

        public AsyncProcessResults(
            Process process,
            AsyncProcessProgress progress)
        {
            _process = process;
            _standardOutput = progress.Output.ToArray();
            _standardError = progress.Error.ToArray();
        }

        public Process Process
        {
            get { return _process; }
        }

        public IEnumerable<string> StandardOutput
        {
            get { return _standardOutput; }
        }

        public IEnumerable<string> StandardError
        {
            get { return _standardError; }
        }
    }
}