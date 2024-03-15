using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private string jumpButtonName = "Jump";
    private string moveHorizontalAxisName = "Horizontal";
    private string moveVerticalAxisName = "Vertical";
    private string attackButtonName = "Fire1";

    public Vector2 moveInput { get; private set; }
    public Vector2 rotationInput { get; private set; }
    public bool isJumping { get; private set; }
    public bool isSprinting { get; private set; }
    public bool isBack { get; private set; }
    public bool isAttack { get; private set; }

    public bool isDodging { get; private set; }

    private float inputX;
    private float inputY;
    private void Start()
    {
        Cursor.visible = false;
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

        moveInput = new Vector2(inputX, inputY);
        rotationInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));

        if (moveInput.sqrMagnitude > 1) moveInput = moveInput.normalized;

        isJumping = Input.GetButton(jumpButtonName);
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        isBack = Input.GetKeyDown(KeyCode.S);
        isDodging = Input.GetKeyDown(KeyCode.Space);
        isAttack = Input.GetButtonDown(attackButtonName);
    }
}