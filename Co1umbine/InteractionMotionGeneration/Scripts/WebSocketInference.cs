using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using WebSocketSharp;

namespace Co1umbine.InteractionMotion.Generation
{
    public class WebSocketInference: IDisposable
    {
        static WebSocket ws;
        public float lastRecieveTime;
        float[] lastResult = null;

        public WebSocketInference(string address, string port)
        {
            ws = new WebSocket($"ws://{address}:{port}");
            SynchronizationContext context = SynchronizationContext.Current;


            ws.OnOpen += (sender, args) => { Debug.Log("WebSocket opened."); };
            ws.OnMessage += (sender, args) => 
            { 
                Debug.Log("WebSocket message received.");
                context.Post(_ => { ReceiveData(args.Data); }, null);
            };
            ws.OnError += (sender, args) => { Debug.Log($"WebScoket Error Message: {args.Message}"); };
            ws.OnClose += (sender, args) => { Debug.Log("WebScoket closed"); };

            ws.Connect();
        }

        public void ResetContext()
        {
            ws.Send("init");
        }

        public void Run(float[] sourceData)
        {
            var data = string.Join(",", sourceData);
            if(ws.ReadyState != WebSocketState.Open)
            {
                Debug.Log("WebSocket is not open.");
                return;
            }
            ws.Send("predict:" + data);
        }

        private void ReceiveData(string data)
        {
            var result = data.Split(',').Select(x => float.Parse(x)).ToArray();
            lastResult = result;
            lastRecieveTime = Time.time;
        }

        public float[] GetResult()
        {
            return lastResult;
        }

        public void Dispose()
        {
            ws?.Send("close");
            ws?.Close();
        }
    }
}