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
    [SerializeField] private List<Collectables> _collectablesList;

    private void Start()
    {
        foreach (var collectable in _collectablesList)
        {
            if (collectable.type != PickableItems.OldHairstyle)
            {
                collectable.item.SetActive(false);
            }
        }
    }

    public void PickUpItem(PickableItems items, bool equip)
    {
        foreach (var collectable in _collectablesList.Where(collectable => collectable.type == items))
        {
            collectable.item.SetActive(equip);
        }
    }

    public void SwapHairStyle()
    {
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
