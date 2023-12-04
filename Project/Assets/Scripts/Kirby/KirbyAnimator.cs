using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KirbyAnimator : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    SpriteRenderer spriteRenderer;

    KirbyCopyAbilities copyAbilityManager;
    KirbyMovment MovementManager;
    Rigidbody2D movementController;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        copyAbilityManager = GameObject.FindObjectOfType<KirbyCopyAbilities>();
        MovementManager = GameObject.FindObjectOfType<KirbyMovment>();
        movementController = MovementManager.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.flipX = !MovementManager.getIsLookingRight;

        animator.SetFloat("speed", MovementManager.xMovment * MovementManager.xMovment);
        animator.SetBool("isGrouned", MovementManager.isGrounded);
        animator.SetBool("isFloating", MovementManager.isFloating);
        animator.SetBool("isInhaling", copyAbilityManager.GetIsInhaling());
        animator.SetBool("isSomethingCurrentlyInhaled", copyAbilityManager.HasKirbyCurrentlyInhaledSomething());
        animator.SetFloat("yVelocity", MovementManager.controller.velocity.y);

        if (copyAbilityManager.CurrentCopyAbility == CopyAbilitiy.None)
        {
            animator.SetInteger("copyAbility", 0);
        }
        else if (copyAbilityManager.CurrentCopyAbility == CopyAbilitiy.Beam)
        {
            animator.SetInteger("copyAbility", 1);
        }
        else if (copyAbilityManager.CurrentCopyAbility == CopyAbilitiy.Bomb)
        {
            animator.SetInteger("copyAbility", 2);
        }
        else if (copyAbilityManager.CurrentCopyAbility == CopyAbilitiy.Cutter)
        {
            animator.SetInteger("copyAbility", 3);
        }
    }
    public void FloatInputed()
    {
        animator.SetTrigger("InputedFloat");
    }
}
