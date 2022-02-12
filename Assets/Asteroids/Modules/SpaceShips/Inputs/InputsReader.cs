using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputsChannel", menuName = "ScriptableObjects/InputsChannel", order = 2)]
public class InputsReader : ScriptableObject, GameInput.IPlayerActions
{
    public Action<float> TurnDirection;

    [SerializeField] private float turnDirection;

    [SerializeField] private bool thrusting;

    [SerializeField] private bool shoot;

    private GameInput _gameInput;

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.Enable();
            _gameInput.Player.SetCallbacks(this);
        }
    }

    private void OnDisable()
    {
        _gameInput.Player.Disable();
    }

    public float GetTurnDirection()
    {
        return turnDirection;
    }


    public bool IsThrusting()
    {
        return thrusting;
    }

    public bool IsShooting()
    {
        return shoot;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        turnDirection = context.ReadValue<float>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        shoot = Convert.ToBoolean(context.ReadValue<float>());
    }

    public void OnThrusting(InputAction.CallbackContext context)
    {
        thrusting = Convert.ToBoolean(context.ReadValue<float>());
    }
}
