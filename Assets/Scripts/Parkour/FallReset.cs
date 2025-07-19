using UnityEngine;

public class FallReset : MonoBehaviour
{
    [SerializeField] private float fallThreshold = -20f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject part2;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform checkPoint;

    private CharacterController cc;

    void Start()
    {
        cc = FindObjectOfType<CharacterController>();
    }

    void Update()
    {
        if (player.transform.position.y < fallThreshold)
        {
            if (cc != null) cc.enabled = false;
            if (part2.activeSelf) player.transform.position = checkPoint.position;
            else player.transform.position = respawnPoint.position;
            if (cc != null) cc.enabled = true;
        }
    }
}