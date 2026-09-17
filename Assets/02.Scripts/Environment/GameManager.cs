using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance=>_instance;

    [Header("플레이어 참조")][SerializeField]private Player _player;
    public Player Player => _player;
    [Header("클릭시 소환 vfx")][SerializeField]private GameObject _clickVfx;
    public GameObject ClickVfx => _clickVfx;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
