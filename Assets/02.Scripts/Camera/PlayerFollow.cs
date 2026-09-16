using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [Header("플레이어")][SerializeField] private Player _player;
    [Header("보간속도")][SerializeField]private float _interpolationTime = 0.5f;
  
    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 CurrentPosition = transform.position;
        CurrentPosition.z=-10f;
        Vector3 TargetPosition = _player.transform.position;
        TargetPosition.z = -10f;
        
        transform.position = Vector3.Lerp(CurrentPosition, TargetPosition, _interpolationTime);
        
    }
}
