using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PickableItems
{
    Pants,
    Shirt,
    Shoes,
    OldHairstyle,
    NewHairstyle,
    MoneyBelt
}
public class DatePreparation : MonoBehaviour
{
    [SerializeField] private List<Collectables> collectablesList;
    [SerializeField] private AudioSource pickUpSound;
    [SerializeField] private FootStepBehaviour _footStepBehaviour;

    private void Start()
    {
        foreach (var collectable in collectablesList)
        {
            if (collectable.type != PickableItems.OldHairstyle)
            {
                collectable.item.SetActive(false);
            }
        }
    }

    public void PickUpItem(PickableItems items, bool equip)
    {
        foreach (var collectable in collectablesList.Where(collectable => collectable.type == items))
        {
            pickUpSound.Play();
            collectable.item.SetActive(equip);
            if (collectable.type == PickableItems.Shoes)
            {
                _footStepBehaviour.PutOnShoes();
            }
        }
    }

    public void SwapHairStyle()
    {
        pickUpSound.Play();
        PickUpItem(PickableItems.OldHairstyle, false);
        PickUpItem(PickableItems.NewHairstyle, true);
    }
    
    [System.Serializable]
    public class Collectables
    {
        public GameObject item;
        public PickableItems type;
    }
}
