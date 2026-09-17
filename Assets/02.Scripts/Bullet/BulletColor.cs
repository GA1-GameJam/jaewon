using UnityEngine;

public class BulletColor : MonoBehaviour
{
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
            (GameManager.Instance.Player.PlayerStat.AttackDamage - 1f) / 9f);

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
            _propertyBlock.SetColor("_Color", _bulletColor);
            _propertyBlock.SetColor("_RendererColor", Color.white);
            _spriteRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
