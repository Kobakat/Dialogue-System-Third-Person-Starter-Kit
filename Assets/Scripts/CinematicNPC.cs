using UnityEngine;
using UnityEngine.Animations;
using Cinemachine;

public class CinematicNPC : MonoBehaviour
{
    public Transform PlayerConversationTransform;
    public string Conversation;

    public CinematicNPC[] OtherNPCS;

    [SerializeField]
    CinemachineVirtualCameraBase SpeakerCamera;

    [SerializeField]
    CinemachineVirtualCameraBase ListenerCamera;

    [SerializeField]
    Animator Anim;

    [SerializeField]
    RuntimeAnimatorController DialogueController;

    RuntimeAnimatorController NonDialogueController;

    const int ActivePriority = 100;
    const int InactivePriority = 0;

    void Start()
    {
        if(Anim)
        {
            NonDialogueController = Anim.runtimeAnimatorController;
        }
    }

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

    public void SetController(bool bStartConversation)
    {
        if(Anim && DialogueController)
        {
            Anim.runtimeAnimatorController = bStartConversation ? DialogueController : NonDialogueController;
        }
    }
}
