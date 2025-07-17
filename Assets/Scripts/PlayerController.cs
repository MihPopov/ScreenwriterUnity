using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float walkSpeedMultiplier = 0.5f;
    [SerializeField] private float runSpeedMultiplier = 1.5f;
    [SerializeField] private float sensitivityX = 10f;
    [SerializeField] private float sensitivityY = 10f;
    [SerializeField] private float cameraLimit = 60f;
    [SerializeField] private float deltaY = 1f;
    [SerializeField] private GameObject dialogLayout;
    [SerializeField] private GameObject screamer;
    [SerializeField] private Camera cam;
    private InventoryManager inventoryManager;
    private CountdownTimer countdownTimer;
    private float _angle;
    private float _verticalVelocity;
    private Vector3 siriusPosition;
    private CharacterController _cc;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cc = GetComponent<CharacterController>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        countdownTimer = FindObjectOfType<CountdownTimer>();
    }

    private void Update()
    {
        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift)) currentSpeed *= walkSpeedMultiplier;
        else if (Input.GetKey(KeyCode.LeftControl)) currentSpeed *= runSpeedMultiplier;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;
        Vector3 moveDirection = Vector3.Normalize(forward + right) * currentSpeed;

        if (Input.GetKeyDown(KeyCode.LeftShift)) cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - deltaY, cam.transform.position.z);
        else if (Input.GetKeyUp(KeyCode.LeftShift)) cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + deltaY, cam.transform.position.z);

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            _angle += Input.GetAxis("Mouse Y") * -sensitivityY * Time.deltaTime;
            _angle = Mathf.Clamp(_angle, -cameraLimit, cameraLimit);
            cam.transform.eulerAngles = new Vector3(_angle, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivityX, 0));
        }

        if (_cc.isGrounded)
        {
            _verticalVelocity = -1f;
            if (Input.GetKeyDown(KeyCode.Space)) _verticalVelocity = jumpForce;
        }
        else _verticalVelocity -= gravity * Time.deltaTime;

        if (!dialogLayout.activeSelf && !screamer.activeSelf)
        {
            moveDirection.y = _verticalVelocity;
            _cc.Move(moveDirection * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LibEnter"))
        {
            siriusPosition = other.transform.position + new Vector3(0f, 0f, 3f);
            if (inventoryManager.HasItem("Ключ") && (countdownTimer.timeRemaining <= 0 || PlayerPrefs.GetInt("GameMode", 0) == 0))
            {
                Teleport(new Vector3(-503f, -499f, -499f));
                other.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
        }
        if (other.CompareTag("LibExit")) Teleport(siriusPosition);
    }


    void Teleport(Vector3 position)
    {
        if (_cc != null) _cc.enabled = false;
        gameObject.transform.position = position;
        if (_cc != null) _cc.enabled = true;
    }
}