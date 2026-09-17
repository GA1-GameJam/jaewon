using UnityEngine;

public class BulletBurstSkillButton : EnhanceButton
{
  

    protected override void OnClick()
    {
        EnhanceManager.Instance.EndEnhance();
    }
}
