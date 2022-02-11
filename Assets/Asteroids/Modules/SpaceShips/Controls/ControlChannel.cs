using UnityEngine;

[CreateAssetMenu(fileName = "ControlChannel", menuName = "ScriptableObjects/ControlChannel", order = 2)]
public class ControlChannel : ScriptableObject, IControlable
{
    public KeyCode ThrustingKey = KeyCode.W;
    public KeyCode HorizontalRightKey = KeyCode.D;
    public KeyCode HorizontalLeftKey = KeyCode.A;

    public float GetTurnDirection()
    {
        float turnDirection;

        if (Input.GetKey(HorizontalLeftKey) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        }
        else if (Input.GetKey(HorizontalRightKey) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1.0f;
        }
        else
        {
            turnDirection = 0f;
        }

        return turnDirection;
    }

    public bool IsThrusting()
    {
        return Input.GetKey(ThrustingKey) || Input.GetKey(KeyCode.UpArrow);
    }
}
