using UnityEngine;
using UnityEngine.Rendering;

public class TorchControls : MonoBehaviour
{
    Light torchLight;
    public InventoryScriptableObject inventory;
    public PanelScript panelScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is create
    void Start()
    {
        torchLight = GetComponentInChildren<Light>();
        inventory.addObserver(panelScript);
    }

    // Update is called once per frame
    void Update()
    {
        if (torchLight.enabled && inventory.torchBattery > 0)
        {
            inventory.updateTorchBatteryAndNotifyObservers(Time.deltaTime);
        }
        if (inventory.torchBattery <= 0)
        {
            torchLight.enabled = false;
        }
    }

    /// <summary>
    /// enable or disable the torch light
    /// </summary>
    public void ToggleTorch()
    {
        if (torchLight != null)
        {
            torchLight.enabled = !torchLight.enabled;
        }
    }
}
