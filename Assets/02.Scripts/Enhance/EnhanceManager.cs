using UnityEngine;



public class EnhanceManager : MonoBehaviour
{
    private static EnhanceManager _instance;
    public static EnhanceManager Instance=>_instance;
    
    private EnhancePanelManager enhancePanelManager;
    public EnhancePanelManager EnhancePanelManager => enhancePanelManager;
    [Header("경험치량 표시하는 객체 참조")][SerializeField]
    private EnhanceIndicator _enhanceIndicator;
    
    [Header("경험치 프리팹")][SerializeField]
    private GameObject _expPrefab;
    [Header("레벨업을 위한 경험치 총량")] [SerializeField]
    private float _expAmountForEnhance;
    [Header("레벨업마다 다음 레벨업 총량 늘어나는 비율")] [SerializeField]
    private float _expAmountMultipier;
    private float _curExp;
    
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        enhancePanelManager = GetComponent<EnhancePanelManager>();
    }
    public void TakeExp(float input)
    {
        _curExp += input * GameManager.Instance.Player.PlayerStat.ExpGetMultiplier;
        if (_curExp >= _expAmountForEnhance)
        {
            
            StartEnhance();
        }

        UpdateExpProgress();
    }

    private void UpdateExpProgress()
    {
        float percent = _curExp / _expAmountForEnhance;
        _enhanceIndicator.Refresh(percent);
    }
    private void StartEnhance()
    {
        _curExp-= _expAmountForEnhance;
        _expAmountForEnhance *= _expAmountMultipier;
        
        enhancePanelManager.OnEnhancePanel();
    }

    public void EndEnhance()
    {
        //실행 내역에 따른 능력 적용 

        enhancePanelManager.OffEnhancePanel();
    }

    public void CreateExp(Vector3 spawnPoint)
    {
        PoolManager.Instance.ExpPoolFactory.Get(_expPrefab,spawnPoint,Quaternion.identity);
    }
}
