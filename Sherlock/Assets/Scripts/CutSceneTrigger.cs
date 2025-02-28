using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera cinematicCamera;
    public GameObject player;
    public PlayableDirector cutscene; // Assign Timeline in Inspector
    private bool cutscenePlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !cutscenePlayed)
        {
            StartCutscene();
            cutscenePlayed = true;
        }
    }

    private void StartCutscene()
    {
        // Increase the Virtual Camera priority to override the Player Camera
        cinematicCamera.Priority = 20;

        // Disable player movement during the cutscene
        if (player) player.GetComponent<CharacterController>().enabled = false;

        // Play the Timeline cutscene
        cutscene.Play();

        // Return to normal camera after cutscene ends
        StartCoroutine(EndCutscene());
    }

    private IEnumerator EndCutscene()
    {
        yield return new WaitForSeconds((float)cutscene.duration);

        // Lower the Virtual Camera priority so the player camera takes control again
        cinematicCamera.Priority = 5;

        // Re-enable player movement
        if (player) player.GetComponent<CharacterController>().enabled = true;
    }
}
