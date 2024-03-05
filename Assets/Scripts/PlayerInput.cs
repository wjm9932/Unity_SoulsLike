using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private string jumpButtonName = "Jump";
    private string moveHorizontalAxisName = "Horizontal";
    private string moveVerticalAxisName = "Vertical";
    private string attackButtonName = "Fire1";

    public Vector2 moveInput { get; private set; }
    public bool isJumping { get; private set; }
    public bool isSprinting { get; private set; }
    public bool isBack { get; private set; }
    public bool isAttack { get; private set; }

    public bool isDodging { get; private set; }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        moveInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));
        
        if (moveInput.sqrMagnitude > 1) moveInput = moveInput.normalized;

        isJumping = Input.GetButton(jumpButtonName);
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        isBack = Input.GetKeyDown(KeyCode.S);
        isDodging = Input.GetKeyDown(KeyCode.F);
        isAttack = Input.GetButtonDown(attackButtonName);
    }
}