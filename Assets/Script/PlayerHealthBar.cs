using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image fillImage;

    public void SetHealth(float current, float max)
    {
        float fill = Mathf.Clamp01(current / max);
        fillImage.fillAmount = fill;
    }
}
