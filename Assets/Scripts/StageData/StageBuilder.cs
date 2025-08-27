using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageBuilder : ScriptableObject
{
    [Header("StagePrefab")]
    public GameObject stagePrefab; 
    [Header("Monster")]
    public GameObject[] mosnters;
    public int[] monsterCount;

    [Header("Rewards")]
    public int moneyReward;
    public float expReward;

    [Header("Other Info")]
    public string stageName;
}
