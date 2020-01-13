using System.IO;
using UnityEngine;

namespace ReadyGamerOne.Const
{
    /// <summary>
    /// 关于版本控制的常量类
    /// </summary>
    public class VersionDefine
    {
        /// <summary>
        /// 当前客户端版本的PlayerPref键
        /// </summary>
        public static string PrefKey_LocalVersion => "NewestVersion";

        public static string OriginAssetManifestPath => Path.Combine(Application.streamingAssetsPath, "OriginAssets");
    }
}