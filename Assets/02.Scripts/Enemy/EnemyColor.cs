using System;
using UnityEngine;

public class EnemyColor : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _enemyColor = new(0.25f, 2.25f, 2.5f, 1f);

    [Header("체력별 색상 (최대 체력 -> 빈사)")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _fullHpColor = new(0.25f, 2.25f, 2.5f, 1f);
    [ColorUsage(true, true)]
    [SerializeField] private Color _lowHpColor = new(2.5f, 0.25f, 0.25f, 1f);

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
            _spriteRenderer.color = _enemyColor;
        }
    }
}
