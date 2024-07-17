using UnityEngine;
using FirstPersonMobileTools.DynamicFirstPerson;

public class AnimationController : MonoBehaviour
{
    public MovementController movementController;
    public CharacterController characterController;
    public Animator animator;
    [HideInInspector]
    public bool IsFiring { get; set; }

    void Update()
    {
        if(movementController.Input_Jump)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        if(movementController.Input_Sprint)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if(IsFiring)
        {
            animator.SetBool("isFiring", true);
        }
        else
        {
            animator.SetBool("isFiring", false);
        }

        if(new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude > 0.0f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
