using System;
using ReadyGamerOne.View.AssetUi;
using UnityEngine;
using UnityEngine.Playables;

namespace ReadyGamerOne.TimeLine
{
	[Serializable]
	public class PanelChangeMonobehavior : PlayableBehaviour
	{
		public PanelUiAsset panelasset;
		public bool dontDestoryOnPlaying = true;


		private bool playing = false;
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			if (!playing)
			{
				PanelAssetMgr.PushPanel(panelasset);
				playing = true;
			}
		}
		

		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			Debug.Log("OnBehaviorPause");
			if (playing)
			{
				if(!Application.isPlaying || !dontDestoryOnPlaying)
					PanelAssetMgr.PopPanel();
				playing = false;
			}
		}

		public override void OnGraphStop(Playable playable)
		{
			if (playing)
			{
				if(!Application.isPlaying || !dontDestoryOnPlaying)
					PanelAssetMgr.PopPanel();
				playing = false;
			}
		}
	}

}

