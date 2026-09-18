using UnityEngine;

public class BulletColor : MonoBehaviour
{
    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int RendererColorId = Shader.PropertyToID("_RendererColor");

    [Header("공격력 최소 색상")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _startColor = new(0.25f, 2.25f, 2.5f, 1f);

    [Header("공격력 최대 색상")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _endColor = new Color(2.5f, 0.5f, 3.0f, 1f);

    private Color _bulletColor;
    public Color GetBulletColor => _bulletColor;
    private SpriteRenderer _spriteRenderer;
    private MaterialPropertyBlock _propertyBlock;
    
    
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _bulletColor = _startColor;
        ApplyColor();
    }
    
    public void DecisionColorToAttackDamage()
    {
        
        float normalizedDamage = Mathf.Clamp01(
            (GameManager.Instance.Player.PlayerStat.AttackDamage - 1f) / 3f);

        _bulletColor = Color.Lerp(
            _startColor,
            _endColor,
            normalizedDamage
        );
        
        ApplyColor();
    }
    
    private void ApplyColor()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (_spriteRenderer != null)
        {
            _propertyBlock ??= new MaterialPropertyBlock();

            _spriteRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(ColorId, _bulletColor);
            _propertyBlock.SetColor(BaseColorId, _bulletColor);
            _propertyBlock.SetColor(RendererColorId, Color.white);
            _spriteRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
