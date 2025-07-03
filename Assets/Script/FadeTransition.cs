using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    public Image fadePanel;
    
    public void FadeToBlack()
    {
        fadePanel.CrossFadeAlpha(1, 1f, false);
    }
    
    public void FadeToClear()
    {
        fadePanel.CrossFadeAlpha(0, 1f, false);
    }
}
