using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace CutieroidUtility
{
    public static class DebugUtility 
    {
        public static string ArraytoString(Int32[] contents)
        {
            String str = "";
            for (var i = 0; i < contents.Length; i++)
            {
                str = str + contents[i].ToString() + " ";
            }
            return str;
        }
        public static string ArraytoString(byte[] contents)
        {
            String str = "";
            for (var i = 0; i < contents.Length; i++)
            {
                str = str + contents[i].ToString() + " ";
            }
            return str;
        }
        public static string ArraytoString(double[] contents)
        {
            String str = "";
            for (var i = 0; i < contents.Length; i++)
            {
                str = str + contents[i].ToString() + " ";
            }
            return str;
        }
        public static string ArraytoString(string[] contents)
        {
            String str = "";
            for (var i = 0; i < contents.Length; i++)
            {
                str = str + contents[i].ToString() + " ";
            }
            return str;
        }
        public static string ArraytoString(UInt32[] contents)
        {
            String str = "";
            for (var i = 0; i < contents.Length; i++)
            {
                str = str + contents[i].ToString() + " ";
            }
            return str;
        }
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
    

}
