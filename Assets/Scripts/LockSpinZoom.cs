using UnityEngine;

public class LockSpinZoom : MonoBehaviour
{
    public float rotateSpeed = 100f;
    public float zoomSpeed = 5f;
    public float minDistance = 1f;
    public float maxDistance = 5f;

    private Transform target;
    private Camera cam;
    private float distance;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("LockSpinZoom은 카메라 오브젝트에 붙어야 합니다!");
            enabled = false;
            return;
        }

        target = transform; // 임시. 나중에 자물쇠 중심을 넣어줄 수도 있음
        distance = Vector3.Distance(cam.transform.position, target.position);
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // 좌클릭 드래그로 회전
        {
            float h = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float v = -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
            cam.transform.RotateAround(target.position, Vector3.up, h);
            cam.transform.RotateAround(target.position, cam.transform.right, v);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, minDistance, maxDistance);
            cam.transform.position = target.position - cam.transform.forward * distance;
        }
    }
}
