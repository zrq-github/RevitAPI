using Autodesk.Revit.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HWLoadBinDLL
{
    internal static class ExternalExecutor
    {
        private static ExternalEvent _externalEvent;

        public static void CreateExternalEvent()
        {
            _externalEvent = ExternalEvent.Create(new ExternalEventHandler());
        }

        public static Task ExecuteInRevitContextAsync(Action<UIApplication> command)
        {
            var request = new Request(command);
            ExternalEventHandler.Queue.Enqueue(request);
            _externalEvent.Raise();
            return request.CompletionSource.Task;
        }


        private class Request
        {
            public readonly Action<UIApplication> Command;
            public readonly TaskCompletionSource<object> CompletionSource = new();

            public Request(Action<UIApplication> command)
            {
                Command = command;
            }
        }

        private class ExternalEventHandler : IExternalEventHandler
        {
            public static readonly ConcurrentQueue<Request> Queue = new();

            public void Execute(UIApplication app)
            {
                while (Queue.TryDequeue(out var request))
                    try
                    {
                        request.Command(app);
                        request.CompletionSource.SetResult(null);
                    }
                    catch (Exception e)
                    {
                        request.CompletionSource.SetException(e);
                    }
            }

            public string GetName()
            {
                return "RevitLookup::ExternalExecutor::ExternalEventHandler";
            }
        }
    }
}
