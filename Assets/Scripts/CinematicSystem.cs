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
        Player.GetComponent<Animator>().applyRootMotion = false;
        StartCoroutine(Fade(1.0f, NPC, true));
    }

    void EndConversation(Transform T)
    {
        StartCoroutine(Fade(1.0f, null, false));
        Player.GetComponent<Animator>().applyRootMotion = true;
        Player.GetComponent<CharacterController>().enabled = true;
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

        CanvasAlpha.alpha = 1.0f;

        //Conversation started
        if (StartedConversation)
        {
            //Move player into position
            Player.transform.position = NPC.PlayerConversationTransform.position;
            Player.transform.rotation = NPC.PlayerConversationTransform.rotation;

            Debug.DrawRay(NPC.PlayerConversationTransform.position, Vector3.up, Color.red, 10f);

            //Cache camera values
            PreviousCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
            PreviousCameraPriority = PreviousCamera.Priority;
            PreviousCamera.Priority = 0;

            //Start cinematic
            NPC.SetActiveSpeaker(true);
        }

        //Conversation ended, return to normal
        else
        {
            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Priority = 0;
            PreviousCamera.Priority = PreviousCameraPriority;
        }

        yield return new WaitForSeconds(0.1f);

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
