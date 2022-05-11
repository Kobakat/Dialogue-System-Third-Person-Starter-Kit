using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TempleEvent : MonoBehaviour
{
    [SerializeField]
    GameObject Guards;

    [SerializeField]
    GameObject Veronica;

    [SerializeField]
    Light Spotlight;

    [SerializeField]
    MeshRenderer Vase;

    void Awake()
    {
        if (DialogueLua.GetVariable("Investigate").AsBool)
        {
            Break();
        }      
    }

    void Break()
    {     
        Guards.SetActive(true);

        Destroy(Veronica);
        Vase.enabled = false;
        Spotlight.enabled = false;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Break();
            enabled = false;        
        }
    }
#endif
}
