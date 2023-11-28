using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "SO/Input/InputReader")]
public class InputReader : ScriptableObject, Controls.IPlayerActions 
{
    public event Action<Vector2> MovementEvent;
    public event Action DodgeEvent;
    public event Action AttackEvent;
    public event Action JumpEvent;

    private Controls _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        
        _controls.Player.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        MovementEvent?.Invoke(value);
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        DodgeEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        AttackEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke();
    }
}