using UnityEngine;

public class GameobjectEventBroadcaster : GenericEventBroadcaster<GameObject>
{
    public new void Broadcast(GameObject data)
    {
        base.Broadcast(data);
    }
}
