using System.Collections.Generic;
using System.Linq;
using DataStructures.Variables;
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
    [SerializeField] private IntVariable itemCounter;
    [SerializeField] private FootStepBehaviour footStepBehaviour;

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

    public void PickUpItem(PickableItems items, bool equip, bool addPoints)
    {
        foreach (var collectable in collectablesList.Where(collectable => collectable.type == items))
        {
            pickUpSound.Play();
            if(addPoints){itemCounter.Add(1);}
            collectable.item.SetActive(equip);
            if (collectable.type == PickableItems.Shoes)
            {
                footStepBehaviour.PutOnShoes();
            }
        }
    }

    public void SwapHairStyle()
    {
        pickUpSound.Play();
        PickUpItem(PickableItems.OldHairstyle, false, true);
        PickUpItem(PickableItems.NewHairstyle, true, false);
    }
    
    [System.Serializable]
    public class Collectables
    {
        public GameObject item;
        public PickableItems type;
    }
}
