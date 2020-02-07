using ReadyGamerOne.Script;
using Preview.Const;
using ReadyGamerOne.MemorySystem;
namespace Preview.Script
{
	public partial class PreviewMgr : AbstractGameMgr<PreviewMgr>
	{
		protected override IResourceLoader ResourceLoader => AssetBundleResourceLoader.Instance;
		protected override IHotUpdatePath PathData => HotUpdatePathData.Instance;
		protected override IOriginAssetBundleUtil OriginBundleData => OriginBundleUtil.Instance;
		partial void OnSafeAwake();
		protected override void Awake()
		{
			base.Awake();
			OnSafeAwake();
		}
	}
}
