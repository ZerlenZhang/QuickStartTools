namespace Preview.Utility
{
	/// <summary>
	/// 这个类提供了Resources下文件名和路径字典访问方式，同名资源可能引起bug
	/// </summary>
	public class ConstUtil
	{
		private static readonly System.Collections.Generic.Dictionary<string,string> nameToPath 
			= new System.Collections.Generic.Dictionary<string,string>{
					{ @"New Tile" , @"New Tile" },
				};
		public static string GetPath(string resName)
		{
			if (!nameToPath.ContainsKey(resName))
				throw new System.Exception("没有这个资源名：" + resName);
			return nameToPath[resName];
		}
	}
}
