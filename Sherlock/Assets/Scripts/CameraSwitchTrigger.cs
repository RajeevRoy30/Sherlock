using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject mainCamera; 
    public GameObject thirdPersonCamera; 
    public Animator animator; 
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("PlayAnimationE"); 
            SwitchToThirdPersonCamera();
        }

       
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("PlayAnimationF"); 
            SwitchToThirdPersonCamera();
        }

        
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            SwitchToMainCamera();
        }
    }

    void SwitchToThirdPersonCamera()
    {
        mainCamera.SetActive(false);
        thirdPersonCamera.SetActive(true);
    }

    void SwitchToMainCamera()
    {
        thirdPersonCamera.SetActive(false);
        mainCamera.SetActive(true);
    }
}