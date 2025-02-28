using UnityEngine;

public class ChaseTrigger : MonoBehaviour
{
    public MonsterAI monsterAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterAI.StartChase();  // Monster starts chasing
        }
    }
}
