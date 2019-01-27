using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public UIHeightCounter heightCounter;
    public UIEnergyCounter energyCounter;
    public UIUnitCounter cooksCounter, scavsCounter, warriorsCounter;

    CameraMovement camMovement;

    #region Awake and Updates
    void Awake()
    {
        instance = this;
        camMovement = Camera.main.GetComponent<CameraMovement>();
    }

    void LateUpdate()
    {
        heightCounter.UpdateCurrentValue(Time.deltaTime);
        energyCounter.UpdateCurrentValue(Time.deltaTime);
        cooksCounter.UpdateCurrentValue(Time.deltaTime);
        scavsCounter.UpdateCurrentValue(Time.deltaTime);
        warriorsCounter.UpdateCurrentValue(Time.deltaTime);
    }
    #endregion

    #region Public methods for updating UI
    public void UpdateTotalHeight(float height)
    {
        heightCounter.SetTargetValue(height);
        camMovement.UpdateTopConstraint(height);
    }

    public void UpdateAverageEnergy(float energyPercent) => energyCounter.SetTargetValue(energyPercent);

    public void UpdateCooksNumber(int cooks) => cooksCounter.SetTargetValue(cooks);
    public void UpdateScavsNumber(int scavs) => scavsCounter.SetTargetValue(scavs);
    public void UpdateWarriorsNumber(int warriors) => warriorsCounter.SetTargetValue(warriors);

    public void UpdateUnitsNumbers(int cooks, int scavs, int warriors)
    {
        cooksCounter.SetTargetValue(cooks);
        scavsCounter.SetTargetValue(scavs);
        warriorsCounter.SetTargetValue(warriors);
    }
    #endregion
}