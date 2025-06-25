using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image fillImage;
    public Vector3 offset = new Vector3(0, 1.5f, 0); // naik di atas musuh
    private Transform target;

    public void Initialize(Transform targetEnemy)
    {
        target = targetEnemy;
    }

    public void SetHealth(float current, float max)
    {
        float fill = Mathf.Clamp01(current / max);
        fillImage.fillAmount = fill;
    }

    void LateUpdate()
    {
        if (target)
            transform.position = target.position + offset;
    }
}
