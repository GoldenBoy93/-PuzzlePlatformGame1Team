using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public Light flashlight;
    public Animator animator;
    float intensityMin = 1f;
    float intensityMax = 1.5f;
    float flickerSpeed = 0.3f;

    float minShakeAmount = 5f; // 흔들림 각도
    float maxShakeAmount = 30f;  // Blend=1일 때 흔들림 크기
    float shakeSpeed = 3f;
    Quaternion baseRotation;
    public Rigidbody joint;
    Vector3 joint_Pos;
    Quaternion joint_Rot;

    private void Awake()
    {
        joint_Pos = joint.transform.localPosition;
        joint_Rot = joint.transform.localRotation;
    }
    private void OnEnable()
    {
        // Rigidbody 초기화
        joint.velocity = Vector3.zero;
        joint.angularVelocity = Vector3.zero;

        // Transform 초기화
        joint.transform.localPosition = joint_Pos;
        joint.transform.localRotation = joint_Rot;
    }
    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light>();

        baseRotation = transform.localRotation;
        StartCoroutine(Flicker());
    }

    void LateUpdate()
    {
        if (animator != null)
        {
            float blend = animator.GetFloat("Blend");
            float shakeAmount = Mathf.Lerp(minShakeAmount, maxShakeAmount, blend);

            // 위아래 흔들림은 줄이기
            float x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * (shakeAmount * 0.2f);
            // 좌우 흔들림 위주
            float y = (Mathf.PerlinNoise(Time.time * shakeSpeed, 1f) - 0.5f) * shakeAmount;
            float blendOffsetX = Mathf.Lerp(0f, -40f, blend);

            transform.localRotation = baseRotation * Quaternion.Euler(x + blendOffsetX, y, 0);
        }
    }
    System.Collections.IEnumerator Flicker()
    {
        while (true)
        {
            flashlight.intensity = Random.Range(intensityMin, intensityMax);
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}