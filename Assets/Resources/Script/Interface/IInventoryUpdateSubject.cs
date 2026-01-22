using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryUpdateSubject
{
    void addObserver(IInventoryUpdateObserver observer);

    void removeObserver(IInventoryUpdateObserver observer);

    void notifyObservers(List<IInventoryUpdateObserver> observer);

    void notifyAmmunitionsUpdate(List<IInventoryUpdateObserver> observer, int amount);
    void notifyFearUpdate(List<IInventoryUpdateObserver> observer, float fear);

    void notifyMedicineUpdate(List<IInventoryUpdateObserver> observer, int amount);
}
