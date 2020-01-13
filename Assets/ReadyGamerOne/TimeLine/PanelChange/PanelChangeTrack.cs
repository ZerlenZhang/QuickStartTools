using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ReadyGamerOne.TimeLine
{
    [TrackColor(0.5f,0.875f,0.134f)]
    [TrackClipType(typeof(PanelChangeClip))]
    public class PanelChangeTrack:TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<PanelChangeMixer>.Create(graph, inputCount);
        }
    }
}