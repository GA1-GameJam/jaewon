using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    private bool _isScaleUpStart;
    [Header("속도 증가율 ")][SerializeField]private float _scaleUpRate=0.5f;
    [Header("시작 스케일 ")][SerializeField]private Vector3 _startScale=new Vector3(0.2f,0.2f,0.2f);

    private void Update()
    {
        if (_isScaleUpStart)
        {
            ScaleUp();
        }
    }

    public void SizeUpStart()
    {
        transform.localScale = _startScale;
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
