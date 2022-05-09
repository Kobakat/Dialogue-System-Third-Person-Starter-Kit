using System.Collections;
using UnityEngine;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    public class SequencerCommandAnimBool : SequencerCommand
    {
        public void Awake()
        {
            int Hash = Animator.StringToHash(GetParameterAs<string>(0, ""));
            GameObject NPC = GameObject.Find(GetParameterAs<string>(1, ""));
            NPC.GetComponentInParent<Animator>().SetBool(Hash, GetParameterAsBool(2));

            float waitTime = GetParameterAsFloat(3, 2.5f);
            StartCoroutine(WaitForAnim(waitTime));
        }

        IEnumerator WaitForAnim(float Duration)
        {
            yield return new WaitForSeconds(Duration);
            Stop();
        }
    }
}
