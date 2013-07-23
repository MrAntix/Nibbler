using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Antix.Nibbler.Diagnostics
{
    //public class DiagnosticsExtensions
    //{
    //    public static Task<AsyncProcessResults> RunAsync(ProcessStartInfo processStartInfo)
    //    {
    //        return ProcessEx.RunAsync(processStartInfo, CancellationToken.None);
    //    }

    //    public static Task<AsyncProcessResults> RunAsync(string fileName)
    //    {
    //        return RunAsync(new ProcessStartInfo(fileName));
    //    }

    //    public static Task<AsyncProcessResults> RunAsync(string fileName, string arguments)
    //    {
    //        return RunAsync(new ProcessStartInfo(fileName, arguments));
    //    }

    //    public static Task<AsyncProcessResults> RunAsync(string fileName, string userName, SecureString password,
    //                                                string domain)
    //    {
    //        return RunAsync(new ProcessStartInfo(fileName)
    //            {
    //                UserName = userName,
    //                Password = password,
    //                Domain = domain,
    //                UseShellExecute = false
    //            });
    //    }

    //    public static Task<AsyncProcessResults> RunAsync(
    //        string fileName, string arguments, string userName,
    //        SecureString password, string domain)
    //    {
    //        return RunAsync(new ProcessStartInfo(fileName, arguments)
    //            {
    //                UserName = userName,
    //                Password = password,
    //                Domain = domain,
    //                UseShellExecute = false
    //            });
    //    }
    //}
}