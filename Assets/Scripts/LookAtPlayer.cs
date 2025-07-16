using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    [SerializeField] private float maxDistance;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero && Vector3.Distance(player.position, transform.position) <= maxDistance) transform.rotation = Quaternion.LookRotation(direction);
    }
}