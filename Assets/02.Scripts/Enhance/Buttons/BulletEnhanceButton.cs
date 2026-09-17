using UnityEngine;

public class BulletEnhanceButton : EnhanceButton
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    protected override void OnClick()
    {
        GameManager.Instance.Player.BulletCountUp(1);
        EnhanceManager.Instance.EndEnhance();
    }
}
