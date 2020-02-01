using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI[] InvSlots_txt;
    int index = 0;
    [SerializeField]
    GameObject InvUI_Obj;
    [SerializeField]
    PInventory inv;

    bool isShowing;

    int selection = -1;

    // Start is called before the first frame update
    void Start()
    {
        InvUI_Obj.SetActive(false);
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < inv.StorageSize; i++)
        {
            if (i < inv.storage.Count)
            {
                InvSlots_txt[i].color = Color.green;
                InvSlots_txt[i].text = inv.storage[i].name;
            }
            else
            {
                InvSlots_txt[i].text = "Empty";
                InvSlots_txt[i].color = Color.grey;
            }
        }
        //InvSlots_txt[inv.StorageSize].text = inv.handR.handSlot.name;
        //InvSlots_txt[inv.StorageSize+1].text = inv.handL.handSlot.name;
        InvSlots_txt[inv.StorageSize].text = inv.hand.handSlot.name;
    }

    public void Select(int index)
    {
        selection = index;
        for (int i = 0; i < InvSlots_txt.Length; i++)
        {
            InvSlots_txt[i].transform.parent.gameObject.GetComponent<Image>().color = Color.white;
        }
        InvSlots_txt[index].transform.parent.gameObject.GetComponent<Image>().color = Color.grey;
    }

    public void Equip()
    {
        
        if (selection == inv.StorageSize) // Unequip HandR
        {
            inv.Unequip(1);
        }else if (selection == inv.StorageSize + 1) // Unequip HandL
        {
            inv.Unequip(0);
        } else if (selection != -1) //regular inventory slot
        {
            inv.Equip(selection);
        }


        UpdateUI(); //Update UI before leaving
    }

    public void Consume(){
        inv.Consume(selection);
        UpdateUI(); //Update UI before leaving
    }

    public void Drop()
    {
        inv.Drop(selection);
        UpdateUI(); //Update UI before leaving
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (isShowing)
            {
                InvUI_Obj.SetActive(false);
                Cursor.visible = false;
                Screen.lockCursor = true;
                isShowing = false;
            }
            else
            {
                UpdateUI();
                InvUI_Obj.SetActive(true);
                Cursor.visible = true;
                Screen.lockCursor = false;
                isShowing = true;
            }
        }
    }
}
