using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private bool inventoryEnabled;

    #region UI
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private Transform slots;
    [SerializeField] private Transform eqSlots;
    [SerializeField] private Slot tmp;
    [SerializeField] private GameObject EQ;

    Slot[] slot;
    Slot[] eqSlot;
    GameObject[] eq;
    #endregion

    public Slot active;

    private bool hold = false;
    private Slot slotMove;

    public Transform spawnPoint;

    [SerializeField] private DisableManager disableMan;
    [SerializeField] private PlayerVitals playerVitals;

    #region create singleton inventory
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
            instance = this;
    }
    #endregion

    private void Start()
    {
        disableMan = GameObject.FindGameObjectWithTag("DisableController").GetComponent<DisableManager>();

        //Inventory Slots
        slot = slots.GetComponentsInChildren<Slot>();

        //Equippment slots in inventory
        eqSlot = eqSlots.GetComponentsInChildren<Slot>();

        //Where to put models of equippment
        eq = GameObject.FindGameObjectsWithTag("EQ");
    }

    private void Update()
    {
        #region show inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;


            if (inventoryEnabled)
            {
                inventoryUI.SetActive(true);
                   disableMan.DisablePlayer();
            }
            else
            {
                inventoryUI.SetActive(false);
                disableMan.EnablePlayer();
            }
        }
        #endregion

        if (inventoryEnabled)
        {
            #region drop item
            if (Input.GetKeyDown(KeyCode.Q))	
            {
                if (active != null && active.item != null)
                {
                    if (active.item.equippable)
                    {
                        Unequip(eq[(int)active.item.eqPart]);
                        ((GameObject)Instantiate(Resources.Load(active.item.name), spawnPoint.position, spawnPoint.rotation)).AddComponent<Rigidbody>();
                    }
                    else
                        Instantiate(Resources.Load(active.item.name), spawnPoint.position, spawnPoint.rotation);
                    RemoveItem(active);
                }
            }

            if (Input.GetMouseButtonDown(0) && !hold)		
            {
                if (active != null && active.item != null)
                {
                    slotMove = active;
                    hold = true;
                }
            }
            if (Input.GetMouseButtonUp(0) && hold)			
            {
                if (active != null)
                {
                    if (active.tag == "Equip")       
                    {
                        EquipItem(slotMove);
                        hold = false;
                    }
                    else
                    {
                        ChangeSlot(slotMove, active);		
                        hold = false;
                    }
                }
                else
                {
                    //if destination is not slot 
                    ChangeSlot(slotMove, active);
                    hold = false;
                   
                }
                slotMove = null;
            }
            #endregion

            if (Input.GetMouseButtonDown(1)) // Use item
            {
                if (active != null && active.item != null && !active.item.equippable)
                    UseItem(active);
                else if (active != null && active.item != null && active.item.equippable)
                    EquipItem(active);
            }
        }
    }
    public bool AddItem(Item item)
    {
        if (item.stackable)
        {
            for (int ii = 0; ii < slot.Length; ii++)
            {
                if (item == slot[ii].item)
                {
                    slot[ii].addItem(1);
                    return true;
                }
            }
        }

        for (int ii=0; ii< slot.Length; ii++)
        {
            if (slot[ii].item ==null)
            {
                slot[ii].addItem(item);
                return true;
            }
        }
    return false;
    }
    public void RemoveItem(Slot slot)
    {
        slot.delItem();
    }
    //equip or consume
    public void UseItem(Slot slot)
    {
        if (!slot.item.craftMaterial)
        {
            slot.item.interaction(playerVitals);
            slot.delItem();
        }
    }
    public void EquipItem(Slot slot)
    {
        int number = (int)slot.item.eqPart;

        if (eqSlot[number].item != null)
            Unequip(eq[number]);
        ChangeSlot(slot, eqSlot[number]);
        ((GameObject)Instantiate(Resources.Load(eqSlot[number].item.name), eq[number].transform.position, eq[number].transform.rotation)).transform.SetParent(eq[number].transform);
    }
    void Unequip(GameObject obj)
    {
        for (int ii = obj.transform.childCount - 1; ii >= 0; --ii)
        {
            GameObject.Destroy(obj.transform.GetChild(ii).gameObject);
        }
        obj.transform.DetachChildren();
    }
    public void ChangeSlot(Slot sl1, Slot sl2)
    {
            if (sl2 != null)                    //chosen slot 2 in eq           
            {
                tmp.stack = sl1.stack;
                tmp.addItem(sl1.item);

            if (sl1.tag == "Equip" && sl1 != sl2)
            {
                Unequip(eq[(int)sl1.item.eqPart]);
            }
            sl1.eraseSlot();

            if (sl2.item != null)           //chosen slot 2 is NOT empty
            {
                sl1.stack = sl2.stack;
                sl1.addItem(sl2.item);
            }
            sl2.eraseSlot();
            sl2.stack = tmp.stack;
            sl2.addItem(tmp.item);
            tmp.eraseSlot();
            }
            else                                //chosen Slot 2 NOT in eq = drop item
            {
            for (int ii = 0; ii < sl1.stack; ii++)
                if (sl1.item.equippable)
                {
                    Unequip(eq[(int)sl1.item.eqPart]);
                    ((GameObject)Instantiate(Resources.Load(sl1.item.name), spawnPoint.position, spawnPoint.rotation)).AddComponent<Rigidbody>();
                }
                else
                    Instantiate(Resources.Load(slotMove.item.name), spawnPoint.position, spawnPoint.rotation);
                sl1.eraseSlot();
            }
        }
}
