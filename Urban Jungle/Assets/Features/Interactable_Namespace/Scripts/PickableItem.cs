using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableItem : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private PickableItems pickableItems;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            item.SetActive(false);
            var datePreparation = collider.gameObject.GetComponent<DatePreparation>();
            if (pickableItems == PickableItems.NewHairstyle)
            {
                datePreparation.SwapHairStyle();
            }
            else
            {
                datePreparation.PickUpItem(pickableItems, true);
            }
        }
    }
}

