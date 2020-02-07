using ReadyGamerOne.Script;
using ReadyGamerOne.MemorySystem;
namespace Preview.Script
{
	public partial class PreviewMgr : AbstractGameMgr<PreviewMgr>
	{
		protected override IResourceLoader ResourceLoader => ResourcesResourceLoader.Instance;
		protected override IAssetConstUtil AssetConstUtil => Utility.AssetConstUtil.Instance;
		partial void OnSafeAwake();
		protected override void Awake()
		{
			base.Awake();
			OnSafeAwake();
		}
	}
}
