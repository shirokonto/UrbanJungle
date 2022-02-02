using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features.StreetParallax_Namespace
{
    public class CarBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform spawn;
        [SerializeField] private Transform target;
        [SerializeField] private float speed;
        [SerializeField] private List<GameObject> carPool;

        [SerializeField][Range(1f, 100f)] private float minRespawnTime;
        [SerializeField][Range(1f, 100f)] private float maxRespawnTime;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private void Instantiate()
        {
            GameObject car = Instantiate(carPool[Random.Range(0, carPool.Count)], transform);
            car.transform.position = spawn.position;
            car.transform.LookAt(target);

            float movementTime = Vector3.Distance(spawn.position, target.position) / speed;
            LeanTween.move(car, target, movementTime).setOnComplete(() =>
            {
                Destroy(car);
            });
        }

        private IEnumerator Spawn()
        {
            Instantiate();
            
            yield return new WaitForSeconds(Random.Range(minRespawnTime, maxRespawnTime));

            StartCoroutine(Spawn());
        }
    }
}
