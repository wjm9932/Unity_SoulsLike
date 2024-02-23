using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string jumpButtonName = "Jump";
    public string moveHorizontalAxisName = "Horizontal";
    public string moveVerticalAxisName = "Vertical";

    public Vector2 moveInput { get; private set; }
    public bool jump { get; private set; }
    public bool isSprinting { get; private set; }

    private void Update()
    {
        moveInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));
        
        if (moveInput.sqrMagnitude > 1) moveInput = moveInput.normalized;

        jump = Input.GetButton(jumpButtonName);
        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }
}