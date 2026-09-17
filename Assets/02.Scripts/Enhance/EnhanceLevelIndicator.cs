using UnityEngine;
using UnityEngine.UI;

public class EnhanceLevelIndicator : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color _baseColor = new(0.25f, 2.25f, 2.5f, 1f);
    [ColorUsage(true, true)]
    [SerializeField] private Color _maxLevelColor = new(2.5f, 0.25f, 0.25f, 1f);

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
