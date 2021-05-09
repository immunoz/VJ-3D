using UnityEngine;
using UnityEngine.UI;

public class DeliveryScript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public float expireTime;
    public Ingredient[] ingredients;

    void Start()
    {
        slider.maxValue = expireTime;
    }

    private void Update()
    {
        if (expireTime > 0)
        {
            expireTime -= Time.deltaTime;
            setProcessTime(expireTime);
        }
    }

    public void setProcessTime(float normalizedTime)
    {
        slider.value = normalizedTime;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
