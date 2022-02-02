using System.Collections.Generic;
using UnityEngine;

public class StreetParalax : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private List<GameObject> carPool;

    private void Start()
    {
        GameObject car = Instantiate(carPool[Random.Range(0, carPool.Count)], transform);
        car.transform.position = spawn.position;
    }
}
