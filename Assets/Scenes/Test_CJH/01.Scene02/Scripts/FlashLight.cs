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

    float minShakeAmount = 5f; // ��鸲 ����
    float maxShakeAmount = 30f;  // Blend=1�� �� ��鸲 ũ��
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
        // Rigidbody �ʱ�ȭ
        joint.velocity = Vector3.zero;
        joint.angularVelocity = Vector3.zero;

        // Transform �ʱ�ȭ
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

            // ���Ʒ� ��鸲�� ���̱�
            float x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * (shakeAmount * 0.2f);
            // �¿� ��鸲 ����
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