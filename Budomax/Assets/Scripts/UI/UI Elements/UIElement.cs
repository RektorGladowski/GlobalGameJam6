using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIElement
{
    public Text textHolder;
    public string textAddition;
    public float valueLerpSpeed;
    protected float currentValue = 0f;
    protected float targetValue = 0f;

 
    public virtual void UpdateCurrentValue(float deltaTime)
    {
        if (currentValue != targetValue)
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, valueLerpSpeed * deltaTime);
            UpdateTextValue();
        }
    }

    public virtual void SetTargetValue(float target) => targetValue = target;
    public virtual void UpdateTextValue() => textHolder.text = ((int)currentValue).ToString() + textAddition;
}