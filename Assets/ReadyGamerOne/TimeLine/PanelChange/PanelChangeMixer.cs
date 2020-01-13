using System;
using ReadyGamerOne.View.AssetUi;
using UnityEngine;
using UnityEngine.Playables;

namespace ReadyGamerOne.TimeLine
{
    [Serializable]
    public class PanelChangeMixer:PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            for (var i = 0; i < playable.GetInputCount(); i++)
            {
                var scriptPlayable = (ScriptPlayable<PanelChangeMonobehavior>) playable.GetInput(i);
                var playableMonoBehavior = scriptPlayable.GetBehaviour();

                var panel = playableMonoBehavior.panelasset;
                if (panel == null)
                    throw new Exception("panelAsset 为空");
                var trans = BaseUiAsset.GetTransform(panel);
                if (trans == null)
                {
                    throw new Exception("trans 为空");
                }

                trans.GetComponent<CanvasGroup>().alpha =
                    playable.GetInputWeight(i);
            }
        }
    }
}