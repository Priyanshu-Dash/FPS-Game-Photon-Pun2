using UnityEngine;
using SUPERCharacter;

public class SuperAnimator : MonoBehaviour
{
    private SUPERCharacterAIO superCharacter;
    public Animator animator;
    
    [HideInInspector]
    public bool IsFiring { get; set; }

    void Start()
    {
        superCharacter = GetComponent<SUPERCharacterAIO>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(superCharacter.Jumped)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        if(superCharacter.isSprinting)
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

        if(!superCharacter.isIdle)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
