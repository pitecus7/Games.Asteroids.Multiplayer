#if MIRROR
using Mirror;
#endif
using UnityEngine;

public abstract class SpaceshipEntity :
#if MIRROR
    NetworkBehaviour
#else
    MonoBehaviour
#endif
{
    public abstract void Init();
}
