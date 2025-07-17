using UnityEngine;
using System.Collections.Generic;

public class PlayerGhostRecorder : MonoBehaviour
{
    [Header("Settings")]
    public float recordInterval = 0.1f;
    public float maxRecordTime = 5f;

    private Queue<GhostFrame> recordedFrames = new Queue<GhostFrame>();
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            timer = 0f;
            recordedFrames.Enqueue(new GhostFrame(transform.position, transform.rotation));
            while (recordedFrames.Count > maxRecordTime / recordInterval) recordedFrames.Dequeue();
        }
    }

    public Queue<GhostFrame> GetRecordedFrames()
    {
        return new Queue<GhostFrame>(recordedFrames);
    }

    public struct GhostFrame
    {
        public Vector3 position;
        public Quaternion rotation;

        public GhostFrame(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }
}