using ReadyGamerOne.Script;
using ReadyGamerOne.View.AssetUi;

namespace ScrptableTest.Scripts
{ 
    public class GameMgr : AbstractGameMgr<GameMgr>
    {
        public PanelUiAsset PanelUiAsset;


//        public ConstStringChooser ConstStringChooser;
//
//        public ResourcesPathChooser ResourcesPathChooser;
//        public TransformPathChooser TransformPathChooser;
//        public AnimationNameChooser AnimationNameChooser;


        protected void Start()
        {
            PanelAssetMgr.PushPanel(PanelUiAsset);
        }
    }
}