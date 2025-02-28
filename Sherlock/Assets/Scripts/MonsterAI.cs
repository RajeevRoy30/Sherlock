using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables; // Import Timeline

public class MonsterAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    public PlayableDirector chaseCutscene; // Reference to Timeline Cutscene

    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isChasing)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
        }
    }

    public void StartChase()
    {
        if (!isChasing) // Prevent re-triggering
        {
            isChasing = true;
            agent.speed = 6f;

            // 🎬 Play Cutscene when the chase starts
            if (chaseCutscene != null)
            {
                chaseCutscene.Play();
            }
        }
    }
}
