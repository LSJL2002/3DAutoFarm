using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [Header("Spawn Area")]
    public float radius = 2f;
    public Vector3 GetRandomPosition()
    {
        Vector2 circle = Random.insideUnitCircle * radius;
        return transform.position + new Vector3(circle.x, 0, circle.y);
    }
}
