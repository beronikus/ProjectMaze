using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set; }

    private PlayerActionL playerAction;

    private void Awake()
    {
        Instance = this;
        playerAction = new PlayerActionL();
        playerAction.Enable();
        
    }

    private void OnDisable()
    {
        playerAction.Disable();
        
    }

    

    public Vector2 GetMovementInput()
    {
        return playerAction.Player.Movement.ReadValue<Vector2>();
    }

    public bool GetDriftInput()
    {
        return playerAction.Player.Drift.IsPressed();
    }
    
    
}
