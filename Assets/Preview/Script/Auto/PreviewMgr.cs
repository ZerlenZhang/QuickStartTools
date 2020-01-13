using ReadyGamerOne.Script;
using ReadyGamerOne.MemorySystem;
using Preview.Const;
namespace Preview.Script
{
	public partial class PreviewMgr : AbstractGameMgr<PreviewMgr>
	{
		private IHotUpdatePath _pathData;
		protected override IHotUpdatePath PathData => HotUpdatePathData.Instance;
		protected override IOriginPathData OriginBundleData => OriginBundleConst.Instance;
		partial void OnSafeAwake();
		protected override void Awake()
		{
			base.Awake();
			OnSafeAwake();
		}
	}
}
