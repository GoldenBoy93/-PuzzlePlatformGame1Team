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

    private HashSet<GameObject> collisions = new HashSet<GameObject>();


    private void Update()
    {
        collisions.RemoveWhere(go =>
        {
            if (go == null) return true; // ÆÄ±«µÈ °æ¿ì
            if (!GetComponent<CharacterController>().bounds.Intersects(go.GetComponent<Collider>().bounds))
            {
                Debug.Log("Exit : " + go.name);
                return true;
            }
            return false;
        });
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
