using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PInventory : MonoBehaviour
{
    [System.Serializable]
    public struct item
    {
        
        public string name;
        public GameObject model;
        public GameObject Spawnable;
        public Sprite icon;
        public int id;
        public bool resource; // item is resource used in crafting
        public bool consumable; // item can be consumed
        public float hunger;
        public float thirst;
        public bool equippable; //weather or not this item is equippable
        public float attack; // damage per second
        public float defense; // blocked damage per second
    };

    [System.Serializable]
    public struct Hand{
        public Transform hand;
        public item handSlot;
        public bool isEmpty;
        public GameObject obj;
    }

    public item[] items;
    //[SerializeField]
    //public Hand handR;
    //[SerializeField]
    //public Hand handL;

    [SerializeField]
    public Hand hand;

    public int StorageSize = 10;
    public List <item> storage = new List <item>();
    [SerializeField]
    Health health;

    // Start is called before the first frame update
    void Start()
    {
        //handL.isEmpty = false;
        //handR.isEmpty = true;
        //handL.handSlot = items[1];
        //hand.isEmpty = false;
        //hand.handSlot = items[5];
        Update_Hands();
    }

    public bool UseResource(int id)
    {
        if (CountResource(id) > 0)
        {
            storage.Remove(storage[findItem(id)]);
            return true;
        }
        else
        {
            return false;
        }
    }

    int findItem (int id) // returns storage index
    {
        for (int i = 0; i < storage.Count; i++)
        {
            if (storage[i].id == id)
            {
                return i;
            }
        }
        return -1;
    }

    public int CountResource(int id) // returns number of resources of id in inventory
    {
        int count = 0;
        for (int i = 0; i < storage.Count; i++)
        {
            if (storage[i].id == id)
            {
                count++;
            }
        }
        return count;
    
    }

    public int AddToStorage(int id) //add item to storage
    {
        if (storage.Count < StorageSize)
        {
            storage.Add(items[id]);
            return 1;
        }
        else
        {
            Debug.Log("Storage Full");
            return -1;
        }
    }

    public void Equip(int ind) // equip the item at index of inventory
    {
        if (storage[ind].equippable) //check if can be equipped
        {
            /*
            if (handL.isEmpty) // Check if Left hand is empty
            {
                handL.handSlot = items[storage[ind].id];
                handL.isEmpty = false;
                storage.Remove(storage[ind]); 
            }
            else if (handR.isEmpty) //check if Right hand is empty
            {
                handR.handSlot = items[storage[ind].id];
                handR.isEmpty = false;
                storage.Remove(storage[ind]);// remove item from inventory
            }
            */
            Unequip(0); // unequip first
            if (hand.isEmpty)
            {
                hand.handSlot = items[storage[ind].id];
                hand.isEmpty = false;
                storage.Remove(storage[ind]);
            }

        }
        Update_Hands();
    }

    public void Unequip(int h) //take item from hand and put into inventory (h=0:HandL; h=1:HandR)
    {
        /*
        if (h == 0)
        {
            if (!handL.isEmpty)// ensure hand is not empty
            {
                if (AddToStorage(handL.handSlot.id) == 1) //ensure was able to add to inventory
                {
                    handL.isEmpty = true;
                    handL.handSlot = items[0]; //item[0] is the empty item
                }
            }
        }else if(h == 1)
        {
            if (!handR.isEmpty)// ensure hand is not empty
            {
                if (AddToStorage(handR.handSlot.id) == 1) //ensure was able to add to inventory
                {
                    handR.isEmpty = true;
                    handR.handSlot = items[0]; //item[0] is the empty item
                }
            }
        }
        */

        if (!hand.isEmpty)// ensure hand is not empty
        {
            if (AddToStorage(hand.handSlot.id) == 1) //ensure was able to add to inventory
            {
                hand.isEmpty = true;
                hand.handSlot = items[0]; //item[0] is the empty item
                Destroy(hand.obj);
            }
        }

        Update_Hands();
    }

    public void Consume(int ind){
        if (storage[ind].consumable){ // make sure item is consumable
            
            float Hgr = storage[ind].hunger;
            float Thrst = storage[ind].thirst;
            health.hunger += Hgr;
            health.hydration += Thrst;
            if (health.hunger > 100){
                health.hunger = 100;
            }
            if (health.hydration > 100){
                health.hydration = 100;
            }
            health.update_Txt();
            storage.Remove(storage[ind]);// remove item from inventory
        }
    }

    public void Drop(int ind)
    {
        Instantiate(storage[ind].Spawnable, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        storage.Remove(storage[ind]);// remove item from inventory
    }

    void Update_Hands(){
        /*
        if(!handR.isEmpty && !handR.obj){ //if hand is not empty and no item is in hand, spawn item in hand
            handR.obj = Instantiate(handR.handSlot.Spawnable);
            handR.obj.transform.position = handR.hand.transform.position;
            handR.obj.transform.parent = handR.hand.transform;
            handR.obj.transform.localRotation = Quaternion.Euler(0, 180, 0);
            handR.obj.transform.localScale = new Vector3(10, 50, 10);



        }
        else {
            Destroy(handR.obj);
        }
        if(!handL.isEmpty && !handL.obj)
        { //if hand is not empty and no item is in hand, spawn item in hand
            handL.obj = Instantiate(handL.handSlot.Spawnable);
            handL.obj.transform.position = handL.hand.transform.position;
            handL.obj.transform.parent = handL.hand.transform;
            handL.obj.transform.localRotation = Quaternion.Euler(0, 180, 0);
            handL.obj.transform.localScale = new Vector3(10, 50, 10);
        }
        else
        {
            Destroy(handL.obj);
        }
        */

        if (!hand.isEmpty)
        { //if hand is not empty and no item is in hand, spawn item in hand
            hand.obj = Instantiate(hand.handSlot.Spawnable);
            hand.obj.GetComponent<Rigidbody>().isKinematic = true; // set the object's rigidbody to kinematic
            hand.obj.transform.position = hand.hand.transform.position;
            hand.obj.transform.parent = hand.hand.transform;
            hand.obj.tag = "Equip"; //give spawn item the 'Equip' tag (used for detecting when hitting stuff)
            hand.obj.transform.localRotation = hand.handSlot.Spawnable.transform.rotation;
            //hand.obj.transform.localRotation = Quaternion.Euler(90, 0, 0);
            //hand.obj.transform.localScale = new Vector3(10, 50, 10);
        }
        else
        {
            Destroy(hand.obj);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
