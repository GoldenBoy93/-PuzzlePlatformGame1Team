using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IInteractable
{
    public string GetInteractPrompt();
    public void Oninteract();
}

public class Interaction : MonoBehaviour //,IInteractable
{

    private HashSet<GameObject> collisions = new HashSet<GameObject>(); //중복방지



    private void Update()
    {
        collisions.RemoveWhere(go =>  //충돌체크 
        {
            if (go == null) return true; // 파괴된 경우
            if (!GetComponent<CharacterController>().bounds.Intersects(go.GetComponent<Collider>().bounds))
            {
                Debug.Log("Exit : " + go.name);
                return true;
            }
            return false;
        });

        // 아이템 줍기 입력
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (var go in collisions)
            {
                var item = go.GetComponent<Interaction>();
                if (item != null)
                {
                    PickupItem(item);
                    break; // 하나만 먹고 종료
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
        playerInventory.data = item.data;   // 임시로 Inventory에 넣고 AddItem 호출
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
            Debug.Log("Enter : " + hit.gameObject.name);
            collisions.Add(hit.gameObject);
        }
    }
}