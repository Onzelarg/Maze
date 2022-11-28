using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

public class movement_new : MonoBehaviour
{
    Rigidbody player;
    public float playerSpeed;
    public float jumpHeight;
    Vector3 toMove;
    public Camera mapCam;
    public int topView;
    PlayerInput playerInput;
    InputAction actionMove;
    InputAction actionLook;
    InputAction actionJump;
    InputAction actionMap;
    InputAction actionScroll;
    public bool isGrounded;
    public bool isMoving;
    bool isPaused;
    public float jumpFreq;
    public float rotationSpeed;
    public CinemachineVirtualCamera cinCam;
    public float xSensitivity;
    public float ySensitivity;
    CinemachineFramingTransposer cinFollow;
    public TextMeshProUGUI deviceText;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        actionJump = playerInput.actions["Jump"];
        actionLook = playerInput.actions["Mouse"];
        actionMove = playerInput.actions["Movement"];
        actionMap = playerInput.actions["Map"];
        actionScroll = playerInput.actions["Scroll"];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        isGrounded = true;
        Cursor.visible = false;
        mapCam.enabled = false;
        isPaused = false;
        topView = 50;
        jumpFreq = 0.5f;
        jumpHeight = 5f;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        rotationSpeed = 70f;
        playerSpeed = 20f;
        player.freezeRotation= true;
        isMoving = false;
        cinFollow = cinCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        xSensitivity = cinCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
        ySensitivity = cinCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
    }

    void OnEnable()
    {
        actionMove.performed += ActionMove_performed;
        actionMove.canceled += ActionMove_canceled;
        actionScroll.started += ActionScroll_started;
        actionLook.started += ActionLook_started;
        actionMap.started += ActionMap_started;
        actionJump.started += ActionJump_started;
    }

    void OnDisable()
    {
        actionMove.performed -= ActionMove_performed;
        actionMove.canceled -= ActionMove_canceled;
        actionScroll.started -= ActionScroll_started;
        actionLook.started -= ActionLook_started;
        actionMap.started -= ActionMap_started;
        actionJump.started -= ActionJump_started;
    }

    void Update()
    {
        if (!isPaused || isMoving)
        {
            playerMovement();
        }
        xSensitivity = cinCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
        ySensitivity = cinCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
    }

    void ActionMove_canceled(InputAction.CallbackContext obj)
    {
        isMoving = false;
    }

    void ActionMove_performed(InputAction.CallbackContext obj)
    {
        playerMovement();
        isMoving = true;
        getDevice(obj);
    }

    void ActionScroll_started(InputAction.CallbackContext obj)
    {
        float scroll = actionScroll.ReadValue<float>();
        cinFollow.m_CameraDistance -= scroll * 0.1f;
    }

    private void ActionLook_started(InputAction.CallbackContext obj)
    {
        getDevice(obj);
    }

    void getDevice(InputAction.CallbackContext obj)
    {
        if (toChangeSensitivity(obj))
        {
            changeSensitivity(5f, 3f);
        }
        else
        {
            changeSensitivity(0.5f, 0.3f);
        }   
    }

    bool toChangeSensitivity(InputAction.CallbackContext obj)
    {
        string device = obj.control.device.ToString().Split("/")[1];
        if (device=="Keyboard" || device=="Mouse")
        {
            deviceText.text = "Keyboard / Mouse";
            return false;
        }
        else
        {
            deviceText.text = device;
            return true;
        }       
    }


    void changeSensitivity(float xS,float yS)
    {
        cinCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = xS;//5
        cinCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = yS;//3
    }

    void ActionJump_started(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            player.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
        }      
        getDevice(obj);
        Invoke("jumpEnable", jumpFreq);
    }

    void jumpEnable()
    {
        isGrounded = true;
    }

    Vector3 convertToCamera(Vector3 toRotate)
    {
        Vector3 camForward = cinCam.transform.forward;
        Vector3 camRight = cinCam.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;
        Vector3 newForward = camForward * toRotate.z;
        Vector3 newRight = camRight * toRotate.x;
        Vector3 result = newForward + newRight;
        return result;
    }

    void rotatePlayer()
    {
        Vector3 toLookat = new Vector3(toMove.x, 0, toMove.z);
        Quaternion currentRotation = transform.rotation;

        if (actionMove.triggered)
        {
            Quaternion targetRotation = Quaternion.LookRotation(toLookat);
            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void playerMovement()
    {
        Vector3 move = new Vector3(actionMove.ReadValue<Vector2>().x, 0, actionMove.ReadValue<Vector2>().y);
        toMove = convertToCamera(move);
        player.AddForce(toMove * playerSpeed, ForceMode.Force);
        rotatePlayer();
    }

    private void ActionMap_started(InputAction.CallbackContext obj)
    {
        getDevice(obj);
        if (mapCam.enabled)
        {
            mapCam.enabled = false;
            isPaused = false;
        }
        else
        {
            mapCam.enabled = true;
            isPaused = true;
            Vector3 player_pos = transform.position;
            mapCam.transform.position = new Vector3(player_pos.x, player_pos.y + topView, player_pos.z);
        }
    }
}
