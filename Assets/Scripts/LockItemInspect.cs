using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockItemInspect : MonoBehaviour
{
    [Header("References")]
    public GameObject inspectPanel;        // UI 패널 (RawImage 포함)
    public Camera inspectCam;              // InspectCam
    public Transform stage;                // InspectCam 앞에 둘 빈 자식 (아이템 올리는 받침대)
    public MonoBehaviour playerController; // 이동/조작 스크립트 (비활성용)
    public Behaviour freeLookCam;          // CinemachineFreeLook 등 (비활성용)

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

        // 플레이어 조작/카메라 비활성
        if (playerController) playerController.enabled = false;
        if (freeLookCam) freeLookCam.enabled = false;

        // 커서 보이기
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 아이템 생성 (Inspect 레이어로 재귀 설정)
        currentItem = Instantiate(itemPrefab, stage);
        SetLayerRecursively(currentItem, LayerMask.NameToLayer("Inspect"));

        // 회전/줌 컨트롤 추가(없으면 자동으로)
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

        // 플레이어/카메라 복구
        if (playerController) playerController.enabled = true;
        if (freeLookCam) freeLookCam.enabled = true;

        // 커서 잠금 복귀(프로젝트에 맞게 조절)
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
