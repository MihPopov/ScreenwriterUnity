using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoverOnPlatform : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 externalMovement = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (externalMovement != Vector3.zero)
        {
            controller.Move(externalMovement);
            externalMovement = Vector3.zero;
        }
    }

    public void AddExternalMovement(Vector3 delta)
    {
        externalMovement += delta;
    }
}