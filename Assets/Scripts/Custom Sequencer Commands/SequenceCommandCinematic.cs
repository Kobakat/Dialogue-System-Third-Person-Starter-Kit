using UnityEngine;
using Cinemachine;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    [AddComponentMenu("")] // Hide from menu.
    public class SequencerCommandCinematic : SequencerCommand
    {
        public void Awake()
        {           
            bool SpeakerActive = GetParameterAsBool(0);
            DialogueManager.Instance.activeConversation.Conversant.GetComponent<CinematicNPC>().SetActiveSpeaker(!SpeakerActive);
            Stop();          
        }
    }
}
