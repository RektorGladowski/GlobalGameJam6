using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    void Awake() => instance = this;

    #region Public for updating UI
    public void UpdateTotalHeight (float height)
    {

    }

    public void UpdateAverageEnergy (float energyPercent)
    {

    }

    public void UpdateCooksNumber (int cooks)
    {

    }

    public void UpdateScavsNumber (int scavs)
    {

    }

    public void UpdateWarriorsNumber (int warriors)
    {

    }

    public void UpdateUnitsNumbers (int cooks, int scavs, int warriors)
    {

    }
    #endregion
}
