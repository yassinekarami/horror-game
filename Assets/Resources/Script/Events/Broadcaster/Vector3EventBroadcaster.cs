using UnityEngine;

public class Vector3EventBroadcaster : GenericEventBroadcaster<Vector3>
{
    public new void Broadcast(Vector3 data)
    {
        base.Broadcast(data);
    }
}
