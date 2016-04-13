using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void CharacterMoved(Vector3 deltaPosition)
    {
        // Update animation
        if (deltaPosition.sqrMagnitude > 0)
            animator.SetBool("Moving", true);
        else
            animator.SetBool("Moving", false);

        // Update rotation to look along movement
        if (deltaPosition.sqrMagnitude > 0)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(deltaPosition), 0.1f);
    }
}
