using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerInput input;
    bool disabledMoving = false;

    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        input = new PlayerInput();

        input.Player.Quit.performed += _ => Application.Quit(0);
    }

    public bool GetInteract()
    {
        float interact = input.Player.Interact.ReadValue<float>();
        return interact > 0.5f;
    }

    public float GetHorizontal()
    {
        if (disabledMoving)
            return 0.0f;

        return input.Player.Horizontal.ReadValue<float>();
    }

    public float GetVertical()
    {
        if (disabledMoving)
            return 0.0f;

        return input.Player.Vertical.ReadValue<float>();
    }

    public bool GetInventoryDown()
    {
        return input.Player.Inventory.WasPressedThisFrame();
    }

    public bool GetEquipmentDown()
    {
        return input.Player.Equipment.WasPressedThisFrame();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void AllowMoving(bool state)
    {
        disabledMoving = !state;
    }

}
