using System.Collections.Generic;
using UnityEngine;

public class RotateBlade : SkillParent
{
    [Header("블레이드 프리팹")]
    [SerializeField] private GameObject _bladePrefab;

    [Header("플레이어와 블레이드 사이 거리")]
    [SerializeField] private float _bladeDistance = 1.5f;

    private List<GameObject> _blades;
    private Player _player;

    private void Start()
    {
        _blades = new List<GameObject>();
        _player = GameManager.Instance.Player;
        MakeNewBlade();
    }

    // Update is called once per frame
    void Update()
    {
        SyncToPlayer();
    }

    private void SyncToPlayer()
    {
        transform.position = _player.transform.position;
    }

    
    protected override void ExcuteLevelUp()
    {
        MakeNewBlade();
        if (_level == 5)
        {
            Awakening();
        }
    }

    protected override void Awakening()
    {
        GetComponent<ObjectRotator>().ControlRotateSpeed(500);
    }

    private void MakeNewBlade()
    {
        if (_bladePrefab == null)
        {
            Debug.LogWarning($"{nameof(RotateBlade)}: 블레이드 프리팹이 할당되지 않았습니다.");
            return;
        }

        Vector3 bladePosition = transform.position + Vector3.up * _bladeDistance;
        GameObject blade=Instantiate(_bladePrefab, bladePosition, transform.rotation, transform);
        _blades.Add(blade);
        SetBladesColor();
    }

 
    private void SetBladesColor()
    {
        foreach (var blade in _blades)
        {
            if (blade == null)
            {
                continue;
            }

            Blade bladeComponent = blade.GetComponent<Blade>();
            bladeComponent?.SetBladeColor(_level);
        }
    }

    private Vector3 GetBladeOffset(int level)
    {
        switch (level)
        {
            case 1:
                return Vector3.up * _bladeDistance;
            case 2:
                return Vector3.down * _bladeDistance;
            case 3:
                return Vector3.left * _bladeDistance;
            case 4:
                return Vector3.right * _bladeDistance;
            case 5:
                
            default:
                Debug.LogWarning($"{nameof(RotateBlade)}: {level}레벨의 블레이드 위치가 정의되지 않았습니다.");
                return Vector3.zero;
        }
    }
}
