using System;
using System.Collections.Generic;
using System.IO;
using ReadyGamerOne.Common;
using UnityEngine;

namespace ReadyGamerOne.MemorySystem
{
    public interface IHotUpdatePath
    {
        string OriginMainManifest { get; }
        string LocalMainPath { get; }
        string WebServeMainPath { get; }   
        string WebServeMainManifest { get; }
        string WebServeVersionPath { get; }
        string WebServeBundlePath { get; }
        string WebServeConfigPath { get; }

        Func<string,string> GetServeConfigPath { get; }
        Func<string,string,string> GetServeBundlePath { get; }
        Func<string,string,string> GetLocalBundlePath { get; }
    }

    public interface IOriginPathData
    {
        Dictionary<string, string> KeyToName { get; }
        Dictionary<string, string> KeyToPath { get; }
        Dictionary<string,string> NameToPath { get; }
    }
    
    public class OriginBundleKey
    {
        public const string Self = @"Self";
    }
    
    public class OriginBundleConst<T>:
        Singleton<T>,
        IOriginPathData
        where T :OriginBundleConst<T>,new()
    {
        public virtual Dictionary<string, string> KeyToName => null;
        public virtual Dictionary<string, string> KeyToPath => null;
        public virtual Dictionary<string, string> NameToPath => null;
    }
}