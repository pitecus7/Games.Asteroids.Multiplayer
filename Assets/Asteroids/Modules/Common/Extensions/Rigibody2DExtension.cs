using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rigibody2DExtension
{
    public static void RemoveVelocity(this Rigidbody2D rigibody)
    {
        rigibody.velocity = Vector2.zero;
    }
}
