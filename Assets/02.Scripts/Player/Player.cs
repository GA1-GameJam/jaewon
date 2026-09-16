using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private PlayerStat _playerStat;
    public PlayerStat PlayerStat => _playerStat;
    
    public void Init()
    {
        
    }
}
