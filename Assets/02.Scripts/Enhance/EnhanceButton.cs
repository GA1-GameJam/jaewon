using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnhanceButton : MonoBehaviour
{
 
    [Header("최대 강화 개수")][SerializeField]
    private int _enhancedLimitedCount=5;
    [SerializeField]protected int _enhancedCount = 1;
    [Header("해당 스킬")]
    [SerializeField]private SkillParent _bindingSkill;
    private GameObject _spawnedSkill;
    
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
        ExcuteSkillEnhance();
        
        if (_enhancedLimitedCount < _enhancedCount)
        {
            ChanceForAwakening();
        }
    }

    private void ExcuteSkillEnhance()
    {
        if (_bindingSkill != null) // 바인딩된 스킬이 있을때
        {
            if (_spawnedSkill == null) //객체가 없다면
            {
                _spawnedSkill=Instantiate(_bindingSkill.gameObject);
            }
            else//있으면 레벨업
            {
                _spawnedSkill.GetComponent<SkillParent>().LevelUp();
            }
        }
    }
    
    private void ChanceForAwakening()
    {
        EnhanceManager.Instance.EnhancePanelManager.DeleteButtonsToAwakening(this);
    }
    
}
