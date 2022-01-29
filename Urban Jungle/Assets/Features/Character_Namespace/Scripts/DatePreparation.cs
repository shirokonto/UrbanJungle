using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = Object.FindObjectOfType<ScoreManager>();
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
            scoreManager.AddPoints(collectable.type.ToString());
            collectable.item.SetActive(equip);
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
