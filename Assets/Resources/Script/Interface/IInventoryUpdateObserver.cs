using UnityEngine;

public interface IInventoryUpdateObserver
{
    void onFearUpdate(float newFear);

    void onMedicineUpdate(int newMedicine);

    void onAmmunitionUpdate(int newAmmunition);

    void onTorchBatteryUpdate(float newBattery);
}
