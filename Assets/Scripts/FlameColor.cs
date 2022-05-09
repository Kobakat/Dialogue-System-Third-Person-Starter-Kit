using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;

public class FlameColor : MonoBehaviour
{
    [SerializeField]
    GameObject MaxAndAlvin;

    void Awake()
    {
        UpdateFlameColor();       
    }

    public void UpdateFlameColor()
    {
        string color = DialogueLua.GetVariable("Flame").AsString;

        if (color == "Red")
        {
            ParticleSystem.MainModule settings = GetComponent<ParticleSystem>().main; // stupid
            settings.startColor = new ParticleSystem.MinMaxGradient(new Color(255, 0, 0));
        }

        else if(color == "Green")
        {
            ParticleSystem.MainModule settings = GetComponent<ParticleSystem>().main; // stupid
            settings.startColor = new ParticleSystem.MinMaxGradient(new Color(0,255,0));
        }

        if (MaxAndAlvin && DialogueLua.GetVariable("Flame").AsString != "None")
        {
            StartCoroutine(WaitForFade());
        }
    }

    private void OnEnable()
    {
        DialogueManager.Instance.conversationEnded += ListenForConversation;
    }

    private void OnDisable()
    {
        if(DialogueManager.Instance)
        {
            DialogueManager.Instance.conversationEnded -= ListenForConversation;
        }
    }

    void ListenForConversation(Transform T)
    {
        UpdateFlameColor();
    }

    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(.55f);
        MaxAndAlvin.SetActive(false);        
    }
}
