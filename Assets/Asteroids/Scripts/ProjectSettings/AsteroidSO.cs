using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidData", menuName = "ScriptableObjects/Asteroid", order = 0)]
public class AsteroidSO : ProjectObject
{
    public uint dividePartsNumber;
    public uint scoreToAdd;
    public AsteroidSO divideObject;
}
