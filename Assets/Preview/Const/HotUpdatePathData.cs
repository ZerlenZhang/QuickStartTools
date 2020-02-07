using ReadyGamerOne.MemorySystem;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using ReadyGamerOne.Common;
namespace Preview.Const
{
	/// <summary>
	/// 使用AB包管理资源时必须的定义路径的常量类
	/// </summary>
	public class HotUpdatePathData : Singleton<HotUpdatePathData>, IHotUpdatePath
	{
		public string OriginMainManifest => @Path.Combine(Application.streamingAssetsPath + @"", "StreamingAssets");
		public string LocalMainPath => @Path.Combine(Application.persistentDataPath, "AssetBundles");
		public string WebServeMainPath => @"file:/C:\Users\ReadyGamerOne\Downloads\webserver";
		public string WebServeMainManifest => WebServeMainPath + "\\ManifestFile";
		public string WebServeVersionPath => WebServeMainPath + "\\ServeVersion.html";
		public string WebServeBundlePath => WebServeMainPath + "\\AssetBundles";
		public string WebServeConfigPath => WebServeMainPath + "\\ServeConfig";
		public Func<string, string> GetServeConfigPath => version =>$"{WebServeConfigPath}/{version}.html";
		public Func<string, string, string> GetServeBundlePath => (bundleName,bundleVersion)=>$"{WebServeBundlePath}/{bundleVersion}/{bundleName}";
		public Func<string, string, string> GetLocalBundlePath => (bundleName,bundleVersion)=>$"{LocalMainPath}/{bundleVersion}/{bundleName}";
	}

	public class OriginBundleKey : ReadyGamerOne.MemorySystem.OriginBundleKey
	{
		public const string Audio = @"Audio";
		public const string File = @"File";
		public const string GlobalAssets = @"GlobalAssets";
		public const string Prefab = @"Prefab";
		public const string Texture = @"Texture";
	}

	public class OriginBundleUtil : OriginBundleUtil<OriginBundleUtil>
	{
		private Dictionary<string,string> _keyToName = new Dictionary<string,string>
		{
			{"Self" , @"self"},
			{"Audio" , @"audio"},
			{"File" , @"file"},
			{"GlobalAssets" , @"globalassets"},
			{"Prefab" , @"prefab"},
			{"Texture" , @"texture"},
		};
		
		public override Dictionary<string, string> KeyToName => _keyToName;
		private Dictionary<string,string> _keyToPath = new Dictionary<string,string>
		{
			{"Self" , Path.Combine(Application.streamingAssetsPath + @"", @"self")},
			{"Audio" , Path.Combine(Application.streamingAssetsPath + @"", @"audio")},
			{"File" , Path.Combine(Application.streamingAssetsPath + @"", @"file")},
			{"GlobalAssets" , Path.Combine(Application.streamingAssetsPath + @"", @"globalassets")},
			{"Prefab" , Path.Combine(Application.streamingAssetsPath + @"", @"prefab")},
			{"Texture" , Path.Combine(Application.streamingAssetsPath + @"", @"texture")},
		};
		public override Dictionary<string, string> KeyToPath => _keyToPath;
		private Dictionary<string,string> _nameToPath = new Dictionary<string,string>
		{
			{@"self" , Path.Combine(Application.streamingAssetsPath + @"", @"self")},
			{@"audio" , Path.Combine(Application.streamingAssetsPath + @"", @"audio")},
			{@"file" , Path.Combine(Application.streamingAssetsPath + @"", @"file")},
			{@"globalassets" , Path.Combine(Application.streamingAssetsPath + @"", @"globalassets")},
			{@"prefab" , Path.Combine(Application.streamingAssetsPath + @"", @"prefab")},
			{@"texture" , Path.Combine(Application.streamingAssetsPath + @"", @"texture")},
		};
		public override Dictionary<string, string> NameToPath => _nameToPath;
	}
}
