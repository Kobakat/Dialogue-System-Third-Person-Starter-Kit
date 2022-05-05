using UnityEngine;
using Cinemachine;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    [AddComponentMenu("")] // Hide from menu.
    public class SequenceCinematic : SequencerCommand
    {
        public void Awake()
        {
            bool SpeakerActive = GetParameterAsBool(0);
            listener.GetComponent<CinematicNPC>().SetActiveSpeaker(!SpeakerActive);
            speaker.GetComponent<CinematicNPC>().SetActiveSpeaker(SpeakerActive);
            Stop();
        }
    }
}
