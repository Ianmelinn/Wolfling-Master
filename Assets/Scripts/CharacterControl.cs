using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CharacterControl : MonoBehaviour 
{
    public CameraControl controlCamera;
    public float movementSpeed;
    public Vector3 focusPoint { get { return transform.position; } }

    private Rigidbody rigidbody;

	void Awake ()
    {
        // Make sure camera is set up
        if (controlCamera == null)
        {
            controlCamera = Object.FindObjectOfType<CameraControl>();
            if (controlCamera != null)
                Debug.LogWarning("controlCamera not set in inspector, found one in scene.", this);
            else
                Debug.LogError("No controlCamera set and one not found in scene.", this);
        }

        rigidbody = GetComponent<Rigidbody>();

        // Hides cursor and locks it - makes mouse controls feel better
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	
	void FixedUpdate () 
    {
        // Gather input
        float forward = Input.GetKey(KeyCode.W) ? 1 : 0;
        forward += Input.GetKey(KeyCode.S) ? -1 : 0;

        float strafe = Input.GetKey(KeyCode.D) ? 1 : 0;
        strafe += Input.GetKey(KeyCode.A) ? -1 : 0;

        // Build movement direction (based off camera's forward vector)
        Vector3 movement = new Vector3(strafe, 0, forward).normalized;
        movement *= movementSpeed * Time.fixedDeltaTime;
        movement = Quaternion.FromToRotation(Vector3.forward, controlCamera.forward) * movement;

        // Move the rigidbody
        rigidbody.MovePosition(transform.position + movement);

        // Let other scripts know about the movement
        BroadcastMessage("CharacterMoved", movement, SendMessageOptions.DontRequireReceiver);
	}
}
