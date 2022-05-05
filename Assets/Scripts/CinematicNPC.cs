using UnityEngine;
using Cinemachine;

public class CinematicNPC : MonoBehaviour
{
    public Transform PlayerConversationTransform;

    [SerializeField]
    CinemachineVirtualCameraBase SpeakerCamera;

    [SerializeField]
    CinemachineVirtualCameraBase ListenerCamera;

    const int ActivePriority = 100;
    const int InactivePriority = 0;

    public void SetActiveSpeaker(bool value)
    {
        if(value)
        {
            SpeakerCamera.Priority = ActivePriority;
            ListenerCamera.Priority = InactivePriority;
        }

        else
        {
            SpeakerCamera.Priority = InactivePriority;
            ListenerCamera.Priority = ActivePriority;
        }
    }
}
