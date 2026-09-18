using UnityEngine;

public class EnemyColor : MonoBehaviour
{
    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int RendererColorId = Shader.PropertyToID("_RendererColor");

    private SpriteRenderer _spriteRenderer;
    private MaterialPropertyBlock _propertyBlock;
    private Color _enemyColor = new(0.25f, 2.25f, 2.5f, 1f);
    public Color GetEnemyColor => _enemyColor;
    [Header("체력별 색상 (최대 체력 -> 빈사)")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _fullHpColor = new(2.5f, 2.0f, 0.1f, 1f);
    [ColorUsage(true, true)]
    [SerializeField] private Color _lowHpColor =  new(2.5f, 0.1f, 0.1f, 1f);
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void DecisionColorByHp(float maxHp, float hp)
    {
        if (maxHp <= 0f) return;

        float hpRatio = Mathf.Clamp01(hp / maxHp);
        _enemyColor = Color.Lerp(_lowHpColor, _fullHpColor, hpRatio);

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
            _propertyBlock.SetColor(ColorId, _enemyColor);
            _propertyBlock.SetColor(BaseColorId, _enemyColor);
            _propertyBlock.SetColor(RendererColorId, Color.white);
            _spriteRenderer.SetPropertyBlock(_propertyBlock);
        }
    }

   
}
