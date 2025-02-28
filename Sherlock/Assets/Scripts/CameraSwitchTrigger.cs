using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class CameraSwitchTrigger : MonoBehaviour
{
    public GameObject mainCamera; // Reference to the main camera
    public CinemachineVirtualCamera virtualCamera; // Reference to the virtual camera
    public PlayableDirector timeline; // Reference to the timeline

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            // Disable the main camera
            mainCamera.SetActive(false);

            // Enable the virtual camera
            virtualCamera.gameObject.SetActive(true);

            // Play the timeline
            if (timeline != null)
            {
                timeline.Play();
            }
        }
    }
}