using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;
using Cinemachine;

public class CinematicSystem : MonoBehaviour
{
    [SerializeField]
    CanvasGroup CanvasAlpha;

    [SerializeField]
    Transform Player;

    ICinemachineCamera PreviousCamera;
    int PreviousCameraPriority;

    void StartConversation(Transform T)
    {
        CinematicNPC NPC = DialogueManager.Instance.activeConversation.Conversant.GetComponent<CinematicNPC>();
        StartCoroutine(Fade(2.0f, NPC, true));
    }

    void EndConversation(Transform T)
    {
        StartCoroutine(Fade(2.0f, null, false));
    }

    IEnumerator Fade(float FullFadeDuration, CinematicNPC NPC, bool StartedConversation)
    {
        float Timer = 0.0f;

        //Fade Out
        while(Timer < FullFadeDuration * 0.5f)
        {
            Timer += Time.deltaTime;

            float Alpha = Timer / (FullFadeDuration * 0.5f);
            CanvasAlpha.alpha = Mathf.Lerp(0.0f, 1.0f, Alpha);

            yield return null;
        }

        //Now that we've faded out, place the actors in the appropritate place and cut to the camera in question
        if(StartedConversation)
        {
            Player.transform.SetPositionAndRotation(NPC.PlayerConversationTransform.position, NPC.PlayerConversationTransform.rotation);
            PreviousCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
            PreviousCameraPriority = PreviousCamera.Priority;
            PreviousCamera.Priority = 0;
            NPC.SetActiveSpeaker(true);
        }

        else
        {
            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Priority = 0;
            PreviousCamera.Priority = PreviousCameraPriority;
        }

        //Fade in
        Timer = 0.0f;
        while (Timer < FullFadeDuration * 0.5f)
        {
            Timer += Time.deltaTime;

            float Alpha = Timer / (FullFadeDuration * 0.5f);
            CanvasAlpha.alpha = Mathf.Lerp(1.0f, 0.0f, Alpha);

            yield return null;
        }
    }

    void OnEnable()
    {
        DialogueManager.Instance.conversationStarted += StartConversation;
        DialogueManager.Instance.conversationStarted += EndConversation;
    }

    void OnDisable()
    {
        if (DialogueManager.Instance)
        {
            DialogueManager.Instance.conversationStarted -= StartConversation;
            DialogueManager.Instance.conversationStarted -= EndConversation;
        }     
    }
}
