using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnhanceButton : MonoBehaviour
{
    [Header("각성가능 해질시 버튼 ")] [SerializeField]
    private EnhanceButton _awakeingButtons;
    public EnhanceButton AwakeingButtons => _awakeingButtons;
    [Header("최대 강화 개수")][SerializeField]
    private int _enhancedLimitedCount=5;
    [SerializeField]protected int _enhancedCount = 1;
    
    
    private Button _button;
    private EnhanceLevelIndicator _enhancedLevelIndicator;
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        _button.onClick.AddListener(ClickCommonBehavior);

        _enhancedLevelIndicator=GetComponent<EnhanceLevelIndicator>();
        _enhancedLevelIndicator.RefreshEnhancedImageByLevel(_enhancedCount, _enhancedLimitedCount);
    }
    

    protected abstract void OnClick();

    
    private void ClickCommonBehavior()
    {
        EnhanceManager.Instance.EndEnhance();
        _enhancedCount++;
        _enhancedLevelIndicator.RefreshEnhancedImageByLevel(_enhancedCount, _enhancedLimitedCount);
        
        if (_enhancedLimitedCount <= _enhancedCount)
        {
            ChanceForAwakening();
        }
    }

    private void ChanceForAwakening()
    {
        EnhanceManager.Instance.EnhancePanelManager.SwitchButtonsToAwakening(this);
    }
    
}
