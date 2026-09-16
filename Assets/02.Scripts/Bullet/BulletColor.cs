using UnityEngine;

public class BulletColor : MonoBehaviour
{
    
    private Color _bulletColor;
    public Color GetBulletColor=> _bulletColor;
    private SpriteRenderer _spriteRenderer;
    
    
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ApplyColor();
    }
    
    public void DecisionColorToAttackDamage()
    {
        
        float normalizedDamage =
            (GameManager.Instance.Player.PlayerStat.AttackDamage - 1f) / 9f;

        _bulletColor = Color.Lerp(
            Color.cyan,
            Color.red,
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
