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

    CinematicNPC ActiveNPC;

    public void StartCinematicDialogue(CinematicNPC NPC)
    {
        if(!DialogueManager.Instance.IsConversationActive)
        {
            ActiveNPC = NPC;
            StartCoroutine(Fade(1.0f, NPC, true, NPC.Conversation));
        }   
    }

    void EndConversation(Transform T)
    {    
        StartCoroutine(Fade(1.0f, ActiveNPC, false));       
    }

    IEnumerator Fade(float FullFadeDuration, CinematicNPC NPC, bool StartedConversation, string Conversation = null)
    {
        float Timer = 0.0f;
        
        //Fade Out
        while (Timer < FullFadeDuration * 0.5f)
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
            //For some reason, root motion needs a couple seconds to be disabled before any position changes are valid
            Player.GetComponent<Animator>().applyRootMotion = false;
            yield return new WaitForSeconds(.05f);

            NPC.SetController(true);
            foreach(CinematicNPC N in NPC.OtherNPCS)
            {
                N.SetController(true);
            }
            
            //Move player into position
            Player.transform.position = NPC.PlayerConversationTransform.position;
            Player.transform.rotation = NPC.PlayerConversationTransform.rotation;

            //Put root motion back on
            yield return new WaitForSeconds(.05f);
            Player.GetComponent<Animator>().applyRootMotion = true;

            //Cache camera values
            PreviousCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
            PreviousCameraPriority = PreviousCamera.Priority;
            PreviousCamera.Priority = 0;

            //Start cinematic
            DialogueManager.StartConversation(Conversation);
            NPC.SetActiveSpeaker(true);        
        }

        //Conversation ended, return to normal
        else
        {
            NPC.SetController(false);
            foreach (CinematicNPC N in NPC.OtherNPCS)
            {
                N.SetController(false);
            }

            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Priority = 0;
            PreviousCamera.Priority = PreviousCameraPriority;
        }

        yield return new WaitForSeconds(0.05f);
        
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
        DialogueManager.Instance.conversationEnded += EndConversation;
    }

    void OnDisable()
    {
        if (DialogueManager.Instance)
        {
            DialogueManager.Instance.conversationEnded -= EndConversation;
        }     
    }
}
