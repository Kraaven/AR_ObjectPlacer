using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectInteractor : MonoBehaviour
{
    [SerializeField] private InteractionManager _interactionManager; // InteractionManager reference (can be used for extended logic)
    public static FurnitureObject CurrentObject;
    private Camera _mainCamera;
    public InputActionReference ActionReference;

    private void Start()
    {
        ActionReference.action.started += OnTap;
    }

    [SerializeField] private TMP_InputField inputField;
    //[SerializeField] private TMP confirmationbutton;
    
    public void OnTap(InputAction.CallbackContext context)
    {
        if (context.performed) // Check if the action was performed
        {
            Vector2 screenPosition = Pointer.current.position.ReadValue(); // Get the tap/click position on the screen
            Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.TryGetComponent<FurnitureObject>(out FurnitureObject obj))
            {
                _interactionManager.CustomisationMenu.SetActive(true);
                CurrentObject = obj;
            }
        }
    }

    public void RequestMaterialChange()
    {
        Debug.Log(inputField.text);
        _interactionManager.CustomisationMenu.SetActive(false);
    }
    
    
}