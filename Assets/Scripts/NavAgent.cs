using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(CapsuleCollider))]
public class NavAgent : MonoBehaviour
{
    [Tooltip("Collection of points the AI will path to. Upon reaching the end, they will return back to the 1st element")]
    [SerializeField] Transform[] Waypoints = new Transform[2];

    [Tooltip("If true, the agent will start at the first waypoint position and move to the second")]
    [SerializeField] bool StartAtFirstWaypoint = false;

    [Tooltip("Acceptable distance from a waypoint to be considered as arriving at the waypoint")]
    float AcceptableRadius = 1.0f;

    NavMeshAgent Agent;
    Animator Anim;
    
    /** Current waypoint index*/
    int WaypointIndex;

    /** True if user supplies bad input. Nav pathing will not occur */
    bool BadWaypointSetup;

    /** Blendspace blend hash*/
    int AnimBlendHash = Animator.StringToHash("Blend"); 

    void Start()
    {    
        int Length = Waypoints.Length;

        if (Length >= 2)
        {
            for(int i = 0; i < Length; i++)
            {
                if(Waypoints[i] == null)
                {
                    BadWaypointSetup = true;
                    break;
                }
            }
        }

        else
        {
            BadWaypointSetup = true;
        }

        if(BadWaypointSetup)
        {
            Debug.LogError("The waypoint collection was set up incorrectly. Ensure you have at least two waypoints and every element is assigned a Transform.");
        }

        else
        {
            Agent = GetComponent<NavMeshAgent>();
            Anim = GetComponent<Animator>();

            if (StartAtFirstWaypoint)
            {
                transform.position = Waypoints[WaypointIndex].position;
                WaypointIndex++;
            }

            Agent.SetDestination(Waypoints[WaypointIndex].position);
        }     
    }

    
    void Update()
    {
        if(!BadWaypointSetup)
        {   
            //If we reached the waypoint, path towards the next one
            if(Agent.remainingDistance < AcceptableRadius)
            {
                WaypointIndex++;

                if (WaypointIndex == Waypoints.Length)
                {
                    WaypointIndex = 0;
                }

                Agent.SetDestination(Waypoints[WaypointIndex].position);
            }

            //Set animation blend space param based on current speed
            float CurrentSpeedSq = Agent.velocity.sqrMagnitude;
            float MaxSpeedSq = Agent.speed * Agent.speed;
            Anim.SetFloat(AnimBlendHash, CurrentSpeedSq / MaxSpeedSq);
        }
    }
}