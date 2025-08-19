using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockItemInspect : MonoBehaviour
{
    [Header("References")]
    public GameObject inspectPanel;        // UI �г� (RawImage ����)
    public Camera inspectCam;              // InspectCam
    public Transform stage;                // InspectCam �տ� �� �� �ڽ� (������ �ø��� ��ħ��)
    public MonoBehaviour playerController; // �̵�/���� ��ũ��Ʈ (��Ȱ����)
    public Behaviour freeLookCam;          // CinemachineFreeLook �� (��Ȱ����)

    [Header("Stage Placement")]
    public float distance = 1.0f;

    GameObject currentItem;

    void Awake()
    {
        if (inspectPanel) inspectPanel.SetActive(false);
        if (!stage && inspectCam)
        {
            var go = new GameObject("InspectStage");
            go.transform.SetParent(inspectCam.transform, false);
            go.transform.localPosition = new Vector3(0, 0, distance);
            stage = go.transform;
        }
    }

    void Update()
    {
        if (inspectPanel && inspectPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    public void Open(GameObject itemPrefab)
    {
        if (!inspectPanel || !inspectCam || !stage || currentItem) return;

        // �÷��̾� ����/ī�޶� ��Ȱ��
        if (playerController) playerController.enabled = false;
        if (freeLookCam) freeLookCam.enabled = false;

        // Ŀ�� ���̱�
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ������ ���� (Inspect ���̾�� ��� ����)
        currentItem = Instantiate(itemPrefab, stage);
        SetLayerRecursively(currentItem, LayerMask.NameToLayer("Inspect"));

        // ȸ��/�� ��Ʈ�� �߰�(������ �ڵ�����)
        if (!currentItem.GetComponentInChildren<LockSpinZoom>())
            currentItem.AddComponent<LockSpinZoom>();

        inspectPanel.SetActive(true);
    }

    public void Close()
    {
        if (!inspectPanel) return;

        inspectPanel.SetActive(false);

        if (currentItem) Destroy(currentItem);
        currentItem = null;

        // �÷��̾�/ī�޶� ����
        if (playerController) playerController.enabled = true;
        if (freeLookCam) freeLookCam.enabled = true;

        // Ŀ�� ��� ����(������Ʈ�� �°� ����)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform t in go.transform)
            SetLayerRecursively(t.gameObject, layer);
    }
}
