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
    [SerializeField] private Camera cam;
    private InventoryManager inventoryManager;
    private float _angle;
    private float _verticalVelocity;
    private CharacterController _cc;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cc = GetComponent<CharacterController>();
        inventoryManager = FindObjectOfType<InventoryManager>();
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

        if (dialogLayout.activeSelf)
        {
            moveDirection.x = 0f;
            moveDirection.z = 0f;
        }
        moveDirection.y = _verticalVelocity;

        _cc.Move(moveDirection * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LibEnter"))
        {
            if (inventoryManager.HasItem("Ключ")) SceneManager.LoadScene(3);
        }
        if (other.CompareTag("LibExit")) SceneManager.LoadScene(2);
    }
}