using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerCondition condition;
    private void Awake()
    {
        CharacterManager.Instance.Player = this;

        if (condition == null)
            condition = GetComponent<PlayerCondition>();
    }
}
