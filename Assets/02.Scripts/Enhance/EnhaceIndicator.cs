using UnityEngine;
using UnityEngine.UI;

public class EnhanceIndicator : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    public void Refresh(float input)
    {
        if (_fillImage != null)
        {
            _fillImage.fillAmount = Mathf.Clamp01(input);
        }
    }
}

