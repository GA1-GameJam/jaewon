using UnityEngine;

public class AttackSpeedEnhanceButton : EnhanceButton
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("증가율")] [SerializeField] private float _plusMultipier=0.1f;
   

    protected override void OnClick()
    {
        GameManager.Instance.Player.AttackSpeedUp(_enhancedCount*_plusMultipier);
        EnhanceManager.Instance.EndEnhance();
    }
}
