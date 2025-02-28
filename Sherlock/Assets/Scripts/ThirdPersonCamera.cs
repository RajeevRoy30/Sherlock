using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2, -5);
    public float smoothSpeed = 5f;

    private bool isThirdPerson = false;

    void LateUpdate()
    {
        if (!isThirdPerson) return;
        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }

    public void ActivateThirdPerson() => isThirdPerson = true;
    public void DeactivateThirdPerson() => isThirdPerson = false;
}
