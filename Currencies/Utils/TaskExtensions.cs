using System;
using System.Threading.Tasks;

namespace WpfApp.Utils
{
    public static class TaskExtensions
    {
        public static Task FireAndForget(this Task task, bool isContinueOnCapturedContext = true, System.Action<System.Exception> onException = null)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await task.ConfigureAwait(isContinueOnCapturedContext);
                }
                catch (Exception ex) when (onException != null)
                {
                    onException.Invoke(ex);
                };
            });
        }
    }
}