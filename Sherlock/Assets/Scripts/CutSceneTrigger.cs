using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector timeline;  
    public GameObject player;  
    public CinemachineVirtualCamera cutsceneCamera; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetActive(false); // Hiding player
            cutsceneCamera.Priority = 15; // Activate the virtual camera
            timeline.Play();
            timeline.stopped += OnCutsceneEnd;
        }
    }

    void OnCutsceneEnd(PlayableDirector pd)
    {
        SceneManager.LoadScene("EndScene");  // Change scene
    }
}
