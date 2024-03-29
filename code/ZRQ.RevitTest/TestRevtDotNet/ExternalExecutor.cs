﻿/*************************************************************************************
 *
 * 创建人员:  zrq 
 * 创建时间:  2022/1/3 17:43:28
 * 文件描述:  
 * 
*************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RQ.Test.RevtDotNet
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
            return request.Tcs.Task;
        }


        private class Request
        {
            public readonly Action<UIApplication> Command;
            public readonly TaskCompletionSource<object> Tcs = new();

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
                        request.Tcs.SetResult(null);
                    }
                    catch (Exception e)
                    {
                        request.Tcs.SetException(e);
                    }
            }

            public string GetName()
            {
                return "RevitLookup::ExternalExecutor::ExternalEventHandler";
            }
        }
    }
}