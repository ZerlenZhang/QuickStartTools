using System;
using ReadyGamerOne.View.AssetUi;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ReadyGamerOne.TimeLine
{
    [Serializable]
    public class PanelChangeClip:PlayableAsset,ITimelineClipAsset
    {
        public PanelUiAsset panelasset;
        public bool dontDestoryOnPlaying = true;
        private PanelChangeMonobehavior template=new PanelChangeMonobehavior();
        private PanelChangeMonobehavior pcm;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PanelChangeMonobehavior>.Create(graph, template);
            pcm = playable.GetBehaviour();
            pcm.dontDestoryOnPlaying = dontDestoryOnPlaying;
            pcm.panelasset = panelasset;
            return playable;
        }

        public ClipCaps clipCaps => ClipCaps.Blending;
    }
}