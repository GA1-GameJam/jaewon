using UnityEngine;

public class BulletColor : MonoBehaviour
{
    
    private Color _bulletColor = new(0.25f, 2.25f, 2.5f, 1f);
    public Color GetBulletColor=> _bulletColor;
    private SpriteRenderer _spriteRenderer;
    
    
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ApplyColor();
    }
    
    public void DecisionColorToAttackDamage()
    {
        
        float normalizedDamage = Mathf.Clamp01(
            (GameManager.Instance.Player.PlayerStat.AttackDamage - 1f) / 9f);

        _bulletColor = Color.Lerp(
            new Color(0.25f, 2.25f, 2.5f, 1f),
            new Color(2.5f, 0.25f, 0.25f, 1f),
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
            _spriteRenderer.color = _bulletColor;
        }
    }
}
