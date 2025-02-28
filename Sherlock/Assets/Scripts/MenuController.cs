using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadStoryScene()
    {
        SceneManager.LoadScene("Story"); 
    }
}
