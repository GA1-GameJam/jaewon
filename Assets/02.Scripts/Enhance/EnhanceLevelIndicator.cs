using UnityEngine;
using UnityEngine.UI;

public class EnhanceLevelIndicator : MonoBehaviour
{
    [SerializeField] private Color _baseColor = new(0.53f, 0.81f, 0.92f, 1f);
    [SerializeField] private Color _maxLevelColor = Color.red;

    private Image _enhancedImage;
    private Material _enhancedMaterial;

    void Start()
    {
        CacheMaterial();
    }

    private void CacheMaterial()
    {
        if (_enhancedImage == null)
        {
            _enhancedImage = GetComponent<Image>();
        }

        if (_enhancedImage == null || _enhancedMaterial != null)
        {
            return;
        }

        if (_enhancedImage.material == null)
        {
            Debug.LogWarning($"{nameof(EnhanceLevelIndicator)} requires an Image material.");
            return;
        }

        _enhancedMaterial = new Material(_enhancedImage.material);
        _enhancedImage.material = _enhancedMaterial;
    }

    public void RefreshEnhancedImageByLevel(int level, int maxLevel)
    {
        CacheMaterial();

        if (_enhancedMaterial == null)
        {
            return;
        }

        float normalizedLevel = maxLevel <= 1
            ? 1f
            : Mathf.InverseLerp(1f, maxLevel, level);

        _enhancedMaterial.color = Color.Lerp(_baseColor, _maxLevelColor, normalizedLevel);
    }

    private void OnDestroy()
    {
        if (_enhancedMaterial != null)
        {
            Destroy(_enhancedMaterial);
        }
    }
}
