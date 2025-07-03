using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
   [SerializeField] private float speed = 5f;
   [SerializeField] private float jumpForce = 5f;
   [SerializeField] private float sensitivityX = 10f;
   [SerializeField] private float sensitivityY = 10f;
   [SerializeField] private float cameraLimit = 60f;
   [SerializeField] private Camera cam;
   private float _angle;
   private CharacterController _cc;

   private void Start()
   {
      Cursor.lockState = CursorLockMode.Locked;
      _cc = GetComponent<CharacterController>();
   }

   private void Update()
   {
      float horizontal = Input.GetAxis("Horizontal");
      float vertical = Input.GetAxis("Vertical");
      Vector3 forward = transform.forward * vertical;
      Vector3 right = transform.right * horizontal;

      if (Time.timeScale == 1)
      {
          _angle += Input.GetAxis("Mouse Y") * -sensitivityY * Time.deltaTime;
          _angle = Mathf.Clamp(_angle, -cameraLimit, cameraLimit);
          cam.transform.eulerAngles = new Vector3(_angle, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
          transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivityX, 0));
      }

      _cc.SimpleMove(Vector3.Normalize(forward + right) * speed);
   }
}
