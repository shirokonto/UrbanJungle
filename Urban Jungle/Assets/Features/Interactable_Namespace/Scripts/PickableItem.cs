using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableItem : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject player;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            print("Item picked up");
            item.SetActive(false);
            if (item.name.Contains("Pants"))
            {
                print("this is pants");
                
            }
        }
    }
}

