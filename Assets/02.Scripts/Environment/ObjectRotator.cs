using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [Header("회전속도")][SerializeField]private float _rotateSpeed=50f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(new Vector3(0f, 0f, _rotateSpeed * Time.deltaTime));
    }
}
