using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectEvent : UnityEvent<GameObject> { }
public class ObjectPoolEvent : UnityEvent<string, GameObject> { }

[System.Serializable]
public class IntEvent : UnityEvent<int> { }

[System.Serializable]
public class StringEvent : UnityEvent<string> { }

