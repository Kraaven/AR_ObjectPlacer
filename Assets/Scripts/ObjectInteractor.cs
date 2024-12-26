using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectInteractor : MonoBehaviour
{
    [SerializeField] private InteractionManager _interactionManager; // InteractionManager reference (can be used for extended logic)
    public static FurnitureObject CurrentObject;
    [SerializeField] private Camera _mainCamera;
    public InputActionReference ActionReference;

    private void Start()
    {
        ActionReference.action.Enable();
        ActionReference.action.started += OnTap;
    }
    
    private void OnDestroy()
    {
        ActionReference.action.started -= OnTap;
    }

    [Header("Inputs")]
    [SerializeField] private TMP_InputField MatField;
    [SerializeField] private TMP_InputField ColorField;
    [SerializeField] private TMP_InputField TextureField;
    //[SerializeField] private TMP confirmationbutton;
    
    public void OnTap(InputAction.CallbackContext context)
    {
        Debug.Log("Tap Checked");
            Vector2 screenPosition = Pointer.current.position.ReadValue(); // Get the tap/click position on the screen
            Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.collider.name);
                if(hit.collider.TryGetComponent<FurnitureObject>(out FurnitureObject obj))
                {
                    _interactionManager.CustomisationMenu.SetActive(true);
                    CurrentObject = obj; 
                }
                
            }
        
    }

    public void RequestMaterialChange()
    {
        if (ColorField.text == "" && MatField.text == "" && TextureField.text == "") return;
        CurrentObject.SetCushionMaterial(MatField.text,ColorField.text, TextureField.text);
        _interactionManager.CustomisationMenu.SetActive(false);
    }
    
    
}