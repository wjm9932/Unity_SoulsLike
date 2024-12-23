using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public delegate bool ClickEvent();

    private string moveHorizontalAxisName = "Horizontal";
    private string moveVerticalAxisName = "Vertical";
    private string attackButtonName = "Fire1";

    public Vector2 mouseInput { get; private set; }
    public Vector2 moveInput { get; private set; }
    public Vector2 dodgeInput { get; private set; }
    public Vector2 rotationInput { get; private set; }
    public bool isSprinting { get; private set; }
    public bool isAttack { get; private set; }
    public bool isLockOn { get; private set; }
    public bool isDodging { get; private set; }
    public bool isUsingQuickSlot { get; private set; }
    public bool isInteracting { get; private set; }
    public bool isPressingInventoryKey { get; private set; }
    public bool isPressingQuestLogKey { get; private set; }
    public bool canGetMouseInput { get; set; }
    public bool isChargingStart { get; private set; }
    public bool isChargingDone { get; private set; }
    public bool isPressingEscape { get; private set; }

    private float inputX;
    private float inputY;
    private float mouseDeltaTime;
    private float duration;
    private void Start()
    {
        canGetMouseInput = true;
        Application.targetFrameRate = 144;
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            inputX = Input.GetAxis(moveHorizontalAxisName);
        }
        else
        {
            inputX = 0f;
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            inputY = Input.GetAxis(moveVerticalAxisName);
        }
        else
        {
            inputY = 0f;
        }

        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        moveInput = new Vector2(inputX, inputY);
        dodgeInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rotationInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));

        if (moveInput.sqrMagnitude > 1) moveInput = moveInput.normalized;

        isSprinting = Input.GetKey(KeyCode.LeftShift);
        isDodging = Input.GetKeyDown(KeyCode.Space);
        isAttack = Input.GetButtonDown(attackButtonName);
        isLockOn = Input.GetKeyDown(KeyCode.F);
        isUsingQuickSlot = Input.GetKeyDown(KeyCode.Q);
        isInteracting = Input.GetKeyDown(KeyCode.E);

        isPressingEscape = Input.GetKeyDown(KeyCode.Escape);

        isPressingInventoryKey = Input.GetKeyDown(KeyCode.Tab);
        isPressingQuestLogKey = Input.GetKeyDown(KeyCode.O);

        isChargingStart = Input.GetButtonDown("Fire2");
        isChargingDone = Input.GetButtonUp("Fire2");
    }

    public bool IsClickItemInInventory(ClickEvent clickEvent)
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDeltaTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            duration = Time.time - mouseDeltaTime;

            if (duration <= 0.2f)
            {
                if (clickEvent != null)
                {
                    return clickEvent.Invoke();
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }
}