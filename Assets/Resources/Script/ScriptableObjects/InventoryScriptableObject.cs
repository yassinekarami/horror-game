using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryScriptableObject", menuName = "Scriptable Objects/InventoryScriptableObject")]
public class InventoryScriptableObject : ScriptableObject, IInventoryUpdateSubject
{

    List<IInventoryUpdateObserver> observers = new List<IInventoryUpdateObserver>();

    public int ammunition;
    public int medicine;
    public float fear;
    public float torchBattery;

    private void OnEnable()
    {
        ammunition = 10;
        medicine = 2;
        fear = 0f;
        torchBattery = 100f;
    }

    /// <summary>
    /// decrease ammunitions value and notify the observers
    /// </summary>
    public void updateAmmunitionsByValueAndNotifyObservers(int value)
    {
        this.ammunition = this.ammunition + value;
        this.notifyAmmunitionsUpdate(observers, this.ammunition);
    }

    /// <summary>
    /// decrease medicine value and notify the observers
    /// </summary>
    public void updateMedicineByValueAndNotifyObservers(int value)
    {
        this.medicine = this.medicine + value;
        this.notifyMedicineUpdate(observers, this.medicine);
    }

    /// <summary>
    /// update fear and notify the observers
    /// </summary>
    /// <param name="fear"></param>
    public void updateFearAndNotifyObservers(float fear)
    {
        this.fear = fear;
        this.notifyFearUpdate(observers, this.fear);
    }

    public void updateTorchBatteryAndNotifyObservers(float value)
    {
        this.torchBattery = this.torchBattery - value;
        this.notifyTorchBatteryUpdate(observers, this.torchBattery);
    }

    /// <summary>
    /// decrease the fear value and  notify the observers
    /// </summary>
    /// <param name="value"></param>
    public void updateFearByValueAndNotifyObservers(float value)
    {
        this.fear = this.fear + value;
        this.notifyFearUpdate(observers, this.fear);
    }
    /// <summary>
    /// Registers an observer to receive inventory update notifications.
    /// </summary>
    /// <param name="observer">The observer to add.</param>
    public void addObserver(IInventoryUpdateObserver observer)
    {
        this.observers.Add(observer);
    }

    /// <summary>
    /// Removes the specified inventory update observer from the list of observers.
    /// </summary>
    /// <param name="observer">The observer to remove.</param>
    public void removeObserver(IInventoryUpdateObserver observer)
    {
        this.observers.Remove(observer);
    }

    /// <summary>
    /// Notifies all registered inventory update observers.
    /// </summary>
    /// <param name="observer">A list of observers to be notified.</param>
    /// <exception cref="System.NotImplementedException">Thrown when the method is not implemented.</exception>
    public void notifyObservers(List<IInventoryUpdateObserver> observer)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Notifies all observers of an ammunition amount update.
    /// </summary>
    /// <param name="observer">The list of observers to notify.</param>
    /// <param name="amount">The updated amount of ammunition.</param>
    public void notifyAmmunitionsUpdate(List<IInventoryUpdateObserver> observer, int amount)
    {
        foreach (var item in observer)
        {
            item.onAmmunitionUpdate(amount);
        }
    }

    /// <summary>
    /// Notifies all registered inventory update observers of a fear value change.
    /// </summary>
    /// <param name="observer">The list of observers to notify.</param>
    /// <param name="fear">The updated fear value to send to observers.</param>
    public void notifyFearUpdate(List<IInventoryUpdateObserver> observer, float fear)
    {
        foreach (var item in observer)
        {
            item.onFearUpdate(fear);
        }
    }

    public void notifyTorchBatteryUpdate(List<IInventoryUpdateObserver> observer, float battery)
    {
        foreach (var item in observer)
        {
            item.onTorchBatteryUpdate(battery);
        }
    }
    /// <summary>
    /// Notifies all registered inventory update observers about a change in medicine amount.
    /// </summary>
    /// <param name="observer">A list of observers to be notified of the medicine update.</param>
    /// <param name="amount">The updated amount of medicine to notify observers about.</param>
    public void notifyMedicineUpdate(List<IInventoryUpdateObserver> observer, int amount)
    {
        foreach (var item in observer)
        {
            item.onMedicineUpdate(amount);
        }
    }
}
