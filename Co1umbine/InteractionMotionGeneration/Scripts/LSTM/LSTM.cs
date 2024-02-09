using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.ML.OnnxRuntime;
using System.Numerics;
using System;
using System.IO;
using System.Linq;

namespace Co1umbine.InteractionMotion.Generation
{
    public interface IInteractionModel
    {
        public float[] Run(float[] sourceData);
    }

    public class LSTM : IDisposable, IInteractionModel
    {
        public static float[] Preprocess(float[] rawData, float[] mean, float[] std)
        {
            int dataLength = rawData.Length;

            float[] preprocessedData = new float[dataLength];

            Debug.Assert(dataLength == mean.Length, "Mean length are not equal to data length.");
            Debug.Assert(dataLength == std.Length, "Std length are not equal to data length.");

            for(int i = 0; i < rawData.Length; i++)
            {
                preprocessedData[i] = (rawData[i] - mean[i]) / std[i];
            }
            return preprocessedData;
        }
        public static float[] Postprocess(float[] outData, float[] mean, float[] std)
        {
            int dataLength = outData.Length;

            float[] postprocessedData = new float[dataLength];

            Debug.Assert(dataLength == mean.Length, "Mean length are not equal to data length.");
            Debug.Assert(dataLength == std.Length, "Std length are not equal to data length.");

            for (int i = 0; i < outData.Length; i++)
            {
                postprocessedData[i] = outData[i] * std[i] + mean[i];
            }

            return postprocessedData;
        }

        public static class Factory
        {
            static public LSTM CreateInteractionLSTM(
                string modelPath,
                long inputSize = 941,
                long outputSize = 941,
                int hiddenSize = 512,
                int n_layers = 2
                )
            {
                return new LSTM(modelPath, inputSize, outputSize, hiddenSize, n_layers);
            }
        }

        public static int dataLength = 1;

        private string modelPath;

        private InferenceSession session;

        private readonly long[] dimensions;
        private readonly long[] hiddenDimensions;

        private int hiddenSize;
        private int n_layers;

        public float[] hidden;
        public float[] cell;

        private LSTM(string modelPath, long inputSize, long outputSize, int hiddenSize, int n_layers)
        {
            this.modelPath = modelPath;
            
            session = new InferenceSession(System.IO.Path.Combine(Application.dataPath, modelPath));
            dimensions = new long[] { 1, dataLength, inputSize };
            hiddenDimensions = new long[] { n_layers, 1, hiddenSize };
            this.hiddenSize = hiddenSize;
            this.n_layers = n_layers;

            hidden = new float[hiddenSize * n_layers];
            cell = new float[hiddenSize * n_layers];
        }

        public void ResetContext()
        {
            hidden = new float[hiddenSize * n_layers];
            cell = new float[hiddenSize * n_layers];
        }

        public float[] Run(float[] sourceData)
        {
            using var inputOrtValue = OrtValue.CreateTensorValueFromMemory<float>(sourceData, dimensions);
            using var hiddenOrtValue = OrtValue.CreateTensorValueFromMemory<float>(hidden, hiddenDimensions);
            using var cellOrtValue = OrtValue.CreateTensorValueFromMemory<float>(cell, hiddenDimensions);

            var inputs = new Dictionary<string, OrtValue>
            {
                {"motion", inputOrtValue},
                {"hidden.1", hiddenOrtValue},
                {"cell.1", cellOrtValue},
            };

            using var runOptions = new RunOptions();

            using var outputs = session.Run(runOptions, inputs, new string[] { "generation", "hidden", "cell" });

            using var generation = outputs[0];
            using var hiddenOut = outputs[1];
            using var cellOut = outputs[2];

            hidden = hiddenOut.GetTensorDataAsSpan<float>().ToArray();
            cell = cellOut.GetTensorDataAsSpan<float>().ToArray();

            return generation.GetTensorDataAsSpan<float>().ToArray();
        }

        public void Dispose()
        {
            session?.Dispose();
        }
    }
}