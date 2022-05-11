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

            CinematicNPC C = DialogueManager.Instance.activeConversation.Conversant.GetComponent<CinematicNPC>();
            
            if(C)
            {
                C.SetActiveSpeaker(!SpeakerActive);
            }

            else
            {
                C = DialogueManager.Instance.activeConversation.Conversant.GetComponentInChildren<CinematicNPC>();
                C.SetActiveSpeaker(!SpeakerActive);
            }

            Stop();
        }
    }
}
