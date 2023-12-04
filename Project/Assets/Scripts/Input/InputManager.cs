using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static PlayerControls movement_controls;

    public static void Initialise_Movment(KirbyMovment movementScript)
    {
        movement_controls = new PlayerControls();

        movement_controls.Game.Enable();

        movement_controls.Game.Movement.performed += ctx =>
        {
            movementScript.MoveKirby(ctx.ReadValue<float>());
        };

        movement_controls.Game.Jump.performed += ctx => { movementScript.Jump(); };

        movement_controls.Game.EnterDoor.performed += ctx =>
        {
            movementScript.TryEnterDoor();
        };

        movement_controls.Game.BackToMainMenu.performed += ctx =>
        {
            movementScript.ExitToMainMenu();
        };
    }

    private static PlayerControls copy_controls;

    public static void Initialise_Copy(KirbyCopyAbilities copyScript)
    {
        copy_controls = new PlayerControls();

        copy_controls.Game.Enable();

        copy_controls.Game.Primary.performed += ctx =>
        {
            copyScript.PrimaryAction();
        };

        copy_controls.Game.Primary.canceled += ctx =>
        {
            copyScript.TryEndSuck();
        };

        copy_controls.Game.Crouch.performed += ctx =>
        {
            copyScript.Swallow();
        };

        copy_controls.Game.Secondary.performed += ctx =>
        {
            copyScript.SecondaryAction();
        };

        copy_controls.Game.DropCopyAbility.performed += ctx =>
        {
            copyScript.DropCopyAbility();
        };
    }

    private static void Crouch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }
}
