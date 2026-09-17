using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    private bool _isScaleUpStart;
    [Header("속도 증가율 ")][SerializeField]private float _scaleUpRate=0.5f;

    private void Update()
    {
        if (_isScaleUpStart)
        {
            ScaleUp();
        }
    }

    public void SizeUpStart()
    {
        _isScaleUpStart = true;
    }

    private void ScaleUp()
    {
        transform.localScale += Vector3.one * _scaleUpRate * Time.deltaTime;
    }

    public void SizeUpStop()
    {
        _isScaleUpStart = false;
        _scaleUpRate = 0f;
    }

    private void OnDisable()
    {
        SizeUpStop();
    }
}
