using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;
using System;

public class movement_new : MonoBehaviour
{
    Rigidbody player;
    public float playerSpeed;
    public float jumpHeight;
    Vector3 toMove;
    public int topView;
    PlayerInput playerInput;
    InputAction actionMove;
    InputAction actionLook;
    InputAction actionJump;
    InputAction actionMap;
    InputAction actionScroll;
    InputAction actionAttack;
    public bool isGrounded;
    bool isPaused;
    public float jumpFreq;
    public float rotationSpeed;
    public CinemachineVirtualCamera cinCam;
    public float xSensitivity;
    public float ySensitivity;
    CinemachineFramingTransposer cinFollow;
    public TextMeshProUGUI deviceText;
    public Animator animator;
    float animationSpeed;
    int moveXAnimation;
    int moveZAnimation;
    int jumpAnimation;
    int attackAnimation;
    public Map map;
    public Camera mapCam;
    public Canvas canvas;
    public GameObject weapon;

    public float rayLength;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        actionJump = playerInput.actions["Jump"];
        actionLook = playerInput.actions["Mouse"];
        actionMove = playerInput.actions["Movement"];
        actionMap = playerInput.actions["Map"];
        actionScroll = playerInput.actions["Scroll"];
        actionAttack = playerInput.actions["Attack"];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        isGrounded = true;
        Cursor.visible = false;
        isPaused = false;
        topView = 50;
        jumpFreq = 0.5f;
        jumpHeight = 5f;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        rotationSpeed = 70f;
        playerSpeed = 20f;
        player.freezeRotation= true;
        cinFollow = cinCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        xSensitivity = cinCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
        ySensitivity = cinCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
        animationSpeed = 0.15f;
        moveXAnimation = Animator.StringToHash("MoveX");
        moveZAnimation = Animator.StringToHash("MoveZ");
        jumpAnimation= Animator.StringToHash("Jump");
        attackAnimation= Animator.StringToHash("Attack");
        mapCam.enabled = false;
        mapCam.depth = -10;
        rayLength = 0.5f;
    }

    void OnEnable()
    {
        actionMove.performed += ActionMove_performed;
        actionScroll.started += ActionScroll_started;
        actionLook.started += ActionLook_started;
        actionMap.started += ActionMap_started;
        actionJump.started += ActionJump_started;
        actionAttack.started += ActionAttack_started;
        
    }

    void OnDisable()
    {
        actionMove.performed -= ActionMove_performed;
        actionScroll.started -= ActionScroll_started;
        actionLook.started -= ActionLook_started;
        actionMap.started -= ActionMap_started;
        actionJump.started -= ActionJump_started;
        actionAttack.started -= ActionAttack_started;
    }

    void Update()
    {
        groundCheck();
        if (!isPaused && isGrounded)
        {
            playerMovement();
        }
        xSensitivity = cinCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
        ySensitivity = cinCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
    }

    void ActionMove_performed(InputAction.CallbackContext obj)
    {
        playerMovement();
        getDevice(obj);
    }

    void ActionScroll_started(InputAction.CallbackContext obj)
    {
        float scroll = actionScroll.ReadValue<float>();
        cinFollow.m_CameraDistance -= scroll * 0.1f;
    }

    void ActionLook_started(InputAction.CallbackContext obj)
    {
        getDevice(obj);
    }

    void ActionAttack_started(InputAction.CallbackContext obj)
    {
        
        animator.CrossFade(attackAnimation, animationSpeed);
        getDevice(obj);  
    }

    void disableCollision()
    {
        if (weapon.TryGetComponent<MeshCollider>(out MeshCollider mesh))
        {
            mesh.enabled = false;
        }
        if (weapon.TryGetComponent<BoxCollider>(out BoxCollider box))
        {
            box.enabled = false;
        }
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
            animator.CrossFade(jumpAnimation, animationSpeed);
            isGrounded = false;
        }      
        getDevice(obj);
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
        animator.SetFloat(moveXAnimation, move.x);
        animator.SetFloat(moveZAnimation, move.z);
        rotatePlayer();  
    }

    void ActionMap_started(InputAction.CallbackContext obj)
    {
        if (mapCam.enabled)
        {
            canvas.enabled = true;
            isPaused = false;
            mapCam.enabled = false;
            mapCam.depth = -10;
        }
        else
        {
            canvas.enabled = false;
            isPaused = true;
            mapCam.enabled = true;
            mapCam.depth = 10;
        }
        
        getDevice(obj);
    }
     
    public void groundCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.down), out hit,rayLength))
        {
            isGrounded = true;
            if (hit.collider.name.Split("Tile").Length>1)
            {
                changeTileColor(Convert.ToInt32(hit.collider.name.Split("Tile")[1]));
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    void changeTileColor(int index)
    {
        MazeGenerator.floors[0].cells[index].changeMaterial(Resources.Load("Room") as Material);
        Tiles cell = MazeGenerator.floors[0].cells[index];
        float scale = MazeGenerator.floors[0].tile_size;
        map.updateTile(index);
    }

}
