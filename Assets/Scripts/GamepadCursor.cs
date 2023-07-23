using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class GamepadCursor : MonoBehaviour
{
    
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    public RectTransform cursorTransform;
    [SerializeField]
    private RectTransform canvasRectTransform;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private float cursorSpeed = 1000f;
    [SerializeField]
    private Image cursorPNG;
    [SerializeField]
    private float padding = 35f;

    private bool previousMouseState;
    private Mouse virtualMouse;
    private Mouse currentMouse;
    private Camera mainCamera;

    public string previousControlScheme ="";
    public const string gamepadScheme = "Gamepad";
    public const string mouseScheme = "Keyboard&Mouse";
    
    public GameObject gameManager;

    private void OnEnable(){
        mainCamera = Camera.main;
        currentMouse = Mouse.current;

        
        if(virtualMouse == null){
            virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
        }
        else if(!virtualMouse.added){
            InputSystem.AddDevice(virtualMouse);
        }
        

        InputUser.PerformPairingWithDevice(virtualMouse,playerInput.user);

        if(cursorTransform != null){
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }


        InputSystem.onAfterUpdate += UpdateMotion;
        playerInput.onControlsChanged += onControlsChanged;
        
    }

    private void OnDisable()
    {
        if(virtualMouse != null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        playerInput.onControlsChanged -= onControlsChanged;
    }

    private void UpdateMotion()
    {
        if(virtualMouse == null || Gamepad.current == null){
            return;
        }
        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.unscaledDeltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
        if(previousMouseState != aButtonIsPressed){
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }

        AnchorCursor(newPosition);
    }

    private void AnchorCursor(Vector2 position){
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, canvas.renderMode
        == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }

    private void onControlsChanged(PlayerInput input){
        if(playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme && gameManager.GetComponent<PauseMenu>().activity){
            cursorTransform.gameObject.SetActive(false);
            Cursor.visible = true;
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
        }
        else if(playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme && gameManager.GetComponent<PauseMenu>().activity){
            cursorTransform.gameObject.SetActive(true);
            Cursor.visible = false;
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchorCursor(currentMouse.position.ReadValue());
            previousControlScheme = gamepadScheme;
        }else if (!gameManager.GetComponent<PauseMenu>().activity){
            Cursor.visible = false;
            cursorTransform.gameObject.SetActive(false);
        }
    }
}
