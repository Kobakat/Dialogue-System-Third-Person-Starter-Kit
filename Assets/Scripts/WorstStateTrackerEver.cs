using UnityEngine;
using PixelCrushers.DialogueSystem; 

public class WorstStateTrackerEver : MonoBehaviour
{
    //i am lazy
 
    private void Update()
    {
        if (QuestLog.GetQuestState("Gwen's Dilemma") == QuestState.Success)
        {
            GetComponent<CinematicNPC>().Conversation = "LeClerc Investigation";
            enabled = false;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            QuestLog.SetQuestState("Gwen's Dilemma", QuestState.Success);
        }
    }
}
