using ReadyGamerOne.Script;
using ReadyGamerOne.MemorySystem;
namespace Test.Script
{
	public partial class TestMgr : AbstractGameMgr<TestMgr>
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
