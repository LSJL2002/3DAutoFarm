using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpening : MonoBehaviour
{
    public Shop shop; 
    public void OnChestExplodeEvent()
    {
        shop.OnChestExplode();
    }
}
