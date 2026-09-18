using UnityEngine;

public class SkillColor : MonoBehaviour
{
    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int RendererColorId = Shader.PropertyToID("_RendererColor");

    [Header("최소 레벨 색상")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _startColor = new(0.25f, 2.25f, 2.5f, 1f);

    [Header("최대 레벨 색상")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _endColor = new(2.5f, 0.25f, 0.25f, 1f);
    private SpriteRenderer _spriteRenderer;
    private MaterialPropertyBlock _propertyBlock;
    
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SetColor(1);
    }
    public void SetColor(int level)
    {
        float normalizedLevel = Mathf.InverseLerp(1f, 5f, level);
        Color bladeColor = Color.Lerp(_startColor, _endColor, normalizedLevel);

        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (_spriteRenderer == null)
        {
            return;
        }

        _propertyBlock ??= new MaterialPropertyBlock();
        _spriteRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(ColorId, bladeColor);
        _propertyBlock.SetColor(BaseColorId, bladeColor);
        _propertyBlock.SetColor(RendererColorId, Color.white);
        _spriteRenderer.SetPropertyBlock(_propertyBlock);
    }
}
