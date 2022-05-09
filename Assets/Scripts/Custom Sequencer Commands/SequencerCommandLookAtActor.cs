using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    [AddComponentMenu("")] // Hide from menu.
    public class SequencerCommandLookAtActor : SequencerCommand
    {
        public void Awake()
        {
            Transform Current = GameObject.Find(GetParameterAs<string>(0, string.Empty)).transform;
            Transform Target = GameObject.Find(GetParameterAs<string>(1, string.Empty)).transform;

            bool bLerp = GetParameterAsBool(2);

            if (bLerp)
            {
                StartCoroutine(LerpRotation(Current, Current.rotation, Quaternion.LookRotation((Target.position - Current.position).normalized)));
            }

            else
            {
                Current.rotation = Quaternion.LookRotation((Target.position - Current.position).normalized);
            }
        }

        IEnumerator LerpRotation(Transform T, Quaternion StartingRotation, Quaternion LookRotation)
        {
            float Timer = 0.0f;

            while (Timer < 0.5f)
            {
                Timer += Time.deltaTime;
                float Alpha = Timer / 0.5f;
                T.rotation = Quaternion.Slerp(StartingRotation, LookRotation, Alpha);
                yield return null;
            }

            yield return new WaitForSeconds(2.0f);
            Stop();
        }
    }
}