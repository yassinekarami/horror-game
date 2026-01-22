using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour, IInventoryUpdateObserver
{
    public Text medicineText;
    public Text fearText;
    public Text ammunitionText;
    public Text batteryText;

    private void Start()
    {
        medicineText.text = $"medicine : val".Replace("val", Inventory.GetInventory().medicine.ToString());
        ammunitionText.text = $"ammunition : val".Replace("val", Inventory.GetInventory().ammunition.ToString());
    }


    public void onAmmunitionUpdate(int newAmmunition)
    {
        ammunitionText.text = $"ammunition : val".Replace("val", Inventory.GetInventory().ammunition.ToString());
    }

    public void onFearUpdate(float newFear)
    {
        fearText.text = $"fear : val".Replace("val", Inventory.GetInventory().fear.ToString());
    }

    public void onTorchBatteryUpdate(float newBattery)
    {
        batteryText.text = $"battery : val".Replace("val", Inventory.GetInventory().torchBattery.ToString());
    }

    public void onMedicineUpdate(int newMedicine)
    {
        medicineText.text = $"medicine : val".Replace("val", Inventory.GetInventory().medicine.ToString());
    }

    public void onInventoryUpdate()
    {
        throw new System.NotImplementedException();
    }
}
