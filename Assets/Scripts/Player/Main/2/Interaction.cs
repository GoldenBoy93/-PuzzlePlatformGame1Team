using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public interface IInteractable
{
    public string GetInteractPrompt();
    public void Oninteract();
}

public class Interaction : MonoBehaviour //,IInteractable
{

    private HashSet<GameObject> collisions = new HashSet<GameObject>(); //�ߺ�����

    GameObject promptText;
    TextMeshProUGUI text;



    private void Start()
    {
        promptText = UI_Manager.Instance._promptText;
        text = promptText.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        collisions.RemoveWhere(go =>  //���������
        {
            if (go == null) return true; // �ı��� ���
            if (!GetComponent<CharacterController>().bounds.Intersects(go.GetComponent<Collider>().bounds))
            {
                promptText.SetActive(false);
                return true;
            }
            return false;
        });

        if (Input.GetKeyDown(KeyCode.E)) //�������ݱ�
        {
            foreach (var go in collisions)
            {
                var item = go.GetComponent<Interaction>();
                if (item != null)
                {
                    promptText.SetActive(false);
                    PickupItem(item);
                    break; // �ϳ��� �԰� ����
                }
            }
        }
    }


    public ItemData data;
    
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }
    
    public void Oninteract()
    {
        //CharacterManager.Instance.Player.itemData = data;
        //CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }

    private void PickupItem(Interaction item)
    {
        var playerInventory = UI_Manager.Instance._inventory;
        playerInventory.data = item.data;   // �ӽ÷� Inventory�� �ְ� AddItem ȣ��
        playerInventory.AddItem();
        Destroy(item.gameObject);
    }

    //private void SetPromptText()
    //{
    //    promptText.gameObject.SetActive(true);
    //    promptText.text = curInteractable.GetInteractPrompt();
    //}
    //
    //public void OnInteractInput(InputAction.CallbackContext context)
    //{
    //    if (context.phase == InputActionPhase.Started && curInteractable != null)
    //    {
    //        curInteractable.Oninteract();
    //        curInteractGameObject = null;
    //        curInteractable = null;
    //        promptText.gameObject.SetActive(false);
    //    }
    //}


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!collisions.Contains(hit.gameObject))
        {
            collisions.Add(hit.gameObject);

            var item = hit.gameObject.GetComponent<Interaction>();
            if (item != null)
            {
                promptText.SetActive(true);
                text.text = item.GetInteractPrompt();
            }
        }
    }
}