using Unity.Behavior;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericScriptableObject", menuName = "Scriptable Objects/GenericScriptableObject")]
public class GenericScriptableObject<T> : EventChannel<T>
{

    /// <summary>
    /// send event message through eventChanel
    /// </summary>
    /// <param name="data"> the data to send</param>
    public void Broadcast(T data)
    {
        this.SendEventMessage(data);
    }
}
