using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayViewCore : MonoBehaviour
{
    public abstract void StartMatch(object Data);
    public abstract void FinishMatch(object Data = null);
}
