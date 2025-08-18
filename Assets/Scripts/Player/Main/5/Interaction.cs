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

    private void Update()
    {
        //collider °Ë»ç
    }
    //public ItemData data;
    //
    //public string GetInteractPrompt()
    //{
    //    string str = $"{data.Name}\n{data.description}";
    //    return str;
    //}
    //
    //public void Oninteract()
    //{
    //    CharacterManager.Instance.Player.itemData = data;
    //    CharacterManager.Instance.Player.addItem?.Invoke();
    //    Destroy(gameObject);
    //}



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

}
