using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    CanvasGroup Canvas;

    [SerializeField]
    Transform ExitTempleTransform;

    [SerializeField]
    AudioSource Music;

    static LevelManager Self;

    static bool bLoaded;

    void Awake()
    {        
        Self = this;

        StartCoroutine(Self.Fade("", true));

        if (bLoaded && ExitTempleTransform)
        {
            Transform PlayerT = GameObject.Find("Player").transform;
            PlayerT.position = ExitTempleTransform.position;
            PlayerT.rotation = ExitTempleTransform.rotation;
        }

        Cursor.visible = false;
        bLoaded = true;
    }

    public static void LoadLevel(string Path)
    {
        Self.StartCoroutine(Self.Fade(Path, false));
    }

    IEnumerator Fade(string Path, bool bFadeIn)
    {
        float Timer = 0.0f;
        float StartVolume = Music.volume;

        while(Timer < 2f)
        {
            Timer += Time.deltaTime;
            float Alpha = Timer / 2.0f;

            Canvas.alpha = bFadeIn ? Mathf.Lerp(1f, 0f, Alpha) : Mathf.Lerp(0f, 1f, Alpha);
            Music.volume = bFadeIn ? Mathf.Lerp(0f, StartVolume, Alpha) : Mathf.Lerp(StartVolume, 0f, Alpha);
            yield return null;
        }
  
        if(!bFadeIn)
        {
            SceneManager.LoadScene(Path);
        }
    }
}
