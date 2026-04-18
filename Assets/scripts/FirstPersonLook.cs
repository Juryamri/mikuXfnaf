using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    public Transform pov;
    public float mouseSensitivity = 150f;

    float xRotation = 0f;
    bool canLook = false;

    void OnEnable()
    {
        StartCoroutine(EnableLookNextFrame());
    }

    System.Collections.IEnumerator EnableLookNextFrame()
    {
        canLook = false;
        yield return null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canLook = true;
    }

    void Update()
    {
        if (!canLook) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        pov.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}