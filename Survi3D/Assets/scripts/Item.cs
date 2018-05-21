using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName ="Inventory/Item")]

public class Item : ScriptableObject {

    #region item properties
    new public string name = "New item";
    public int hp;

    public Sprite icon;
    public bool stackable;

    [SerializeField] private float value;

    public bool craftMaterial;
    public bool equippable;

    #endregion

    #region types of consumables
    [Header("Your Consumables")]
    [Space(10)]
    public bool food;
    public bool water;
    public bool health;
    public bool sleepingBag;
    public bool rock;
    public bool tree;
    #endregion

    //#region Equippable

    //Equippable parts
    public enum EQPart
    {
        Gloves = 0,
        Neck = 1,
        Legs = 2,
        Ring2 = 3,
        OH = 4,
        Chest = 5,
        Ring1 = 6,
        MH = 7,
        Head = 8,
        Feet = 9
    };
    public EQPart eqPart;
    //public bool helm;
    //public bool chestplate;
    //public bool legplate;
    //public bool feet;
    //public bool gloves;

    //public bool weapon;
    //public bool shield;

    //public bool neck;
    //public bool ring;
    //#endregion

    public void interaction(PlayerVitals playerVitals)
    {
        if (food)
        {
            playerVitals.hungerSlider.value += value;
        }
        else if (water)
        {

            playerVitals.thirstSlider.value += value;

        }
        else if (health)
        {
            playerVitals.hpSlider.value += value;

        }
      

    }


}
