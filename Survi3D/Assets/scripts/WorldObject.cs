using UnityEngine;


public class WorldObject : MonoBehaviour
{
    public Item item;
    [SerializeField] private SleepController sleepMan;

    private void Start()
    {
        name = item.name;

        sleepMan = GameObject.FindGameObjectWithTag("SleepController").GetComponent<SleepController>();
    }

    //Interaction with non-pickup objects TODO
  

public void Interaction()
    {
        if (item.rock || item.tree)
        {
            if (item.hp > 0)
            {
                item.hp -= 10;
                print(item.hp);
            }

            if (item.hp <= 0)
            {
                item.hp = 100;
                itemDie();
            }
        }
        else if (item.sleepingBag)
        {
            sleepMan.EnableSleepUI();
        }
        else if (Inventory.instance.AddItem(this.item))
        {
           Destroy(gameObject);
        }
    }


    public void itemDie()
    {
        
        if (item.tree)
        {
            transform.rotation = Quaternion.Euler(60, 0, 0);
            Instantiate(Resources.Load("logs"), transform.position, transform.rotation);
        }
        else if (item.rock)
        {
            Instantiate(Resources.Load("stones"), this.transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
