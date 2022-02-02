using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableItem : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private PickableItems pickableItems;
    private void OnTriggerEnter(Collider itemCollider)
    {
        if (itemCollider.gameObject.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            item.SetActive(false);
            var datePreparation = itemCollider.gameObject.GetComponent<DatePreparation>();
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

