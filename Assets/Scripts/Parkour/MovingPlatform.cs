using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool waitAtEnds = false;
    [SerializeField] private float waitTime = 1f;

    private Vector3 target;
    private bool waiting = false;
    private Vector3 lastPosition;
    private GameObject player;

    void Start()
    {
        target = pointB.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (waiting) return;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 delta = transform.position - lastPosition;
        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null) controller.ApplyPlatformMotion(delta);
        }
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            if (waitAtEnds)
            {
                waiting = true;
                Invoke(nameof(SwitchTarget), waitTime);
            }
            else SwitchTarget();
        }
        lastPosition = transform.position;
    }

    void SwitchTarget()
    {
        target = (target == pointA.position) ? pointB.position : pointA.position;
        waiting = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) player = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) player = null;
    }
}