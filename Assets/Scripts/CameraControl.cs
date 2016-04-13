using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{
    public CharacterControl character;

    public float distance;
    public Vector2 mouseSensitivity;

    private float theta = Mathf.PI * 0.5f, phi;

    // Forward vector on the x-z plane
    public Vector3 forward
    { 
        get 
        {
            Vector3 forward = transform.forward;
            forward.y = 0;
            return forward.normalized;
        }
    }

    void Awake()
    {
        // Make sure camera is set up
        if (character == null)
        {
            character = Object.FindObjectOfType<CharacterControl>();
            if (character != null)
                Debug.LogWarning("character not set in inspector, found one in scene.", this);
            else
                Debug.LogError("No character set and one not found in scene.", this);
        }
    }

	void Update () 
    {
        // Gather mouse inputs
        float rotate = Input.GetAxis("Mouse X") * mouseSensitivity.x * -1; // -1 inverts mouse control - remove if you don't want it
        float panUp = Input.GetAxis("Mouse Y") * mouseSensitivity.y * -1;
        float zoom = Input.mouseScrollDelta.y;

        // Update state variables
        phi += rotate;
        theta += panUp;
        distance += zoom;

        // Clamp variable values
        distance = Mathf.Max(0, distance);
        theta = Mathf.Clamp(theta, 0.2f, Mathf.PI * 0.75f);
        while (phi < 0) phi += Mathf.PI * 2;
        while (phi > Mathf.PI * 2) phi -= Mathf.PI * 2;

        // Build target position
        float x = distance * Mathf.Sin(theta) * Mathf.Cos(phi);
        float y = distance * Mathf.Cos(theta);
        float z = distance * Mathf.Sin(theta) * Mathf.Sin(phi);
        Vector3 position = character.focusPoint + new Vector3(x, y, z);

        // Raycast off scene to place camera at closest point not behind an object
        Ray r = new Ray(character.focusPoint, position - character.focusPoint);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, distance))
        {
            // Back it off a smidgen so it's not too flush with the hit object
            position = r.GetPoint(hit.distance - 0.25f);
        }

        // Apply position and look
        transform.position = position;
        transform.LookAt(character.focusPoint);
	}
}
