using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Stages")]
    public StageBuilder[] stages;
    public Transform stageParent;
    private int currentStageIndex = 0;
    private int stageNumber = 1; // keeps increasing (1,2,3,…)
    private int loopCount = 0;   // how many times we looped back to index 0
    private GameObject activeStage;
    private List<GameObject> activeMonsters = new List<GameObject>();

    public int CurrentStageIndex => currentStageIndex;
    public int CurrentStageNumber => stageNumber; 
    public int MonstersRemaining => activeMonsters.Count;
    public System.Action<int, int> OnStageUpdated;
    private StageClearUI stageClearUI;

    void Start()
    {
        stageClearUI = FindObjectOfType<StageClearUI>();
        if (stageClearUI != null)
            stageClearUI.Init(this);

        LoadStage(currentStageIndex);
    }

    public void LoadStage(int index)
    {
        if (index < 0 || index >= stages.Length) return;

        if (activeStage != null)
        {
            Destroy(activeStage);
            foreach (var m in activeMonsters)
                if (m != null) Destroy(m);
            activeMonsters.Clear();
        }

        StageBuilder stageData = stages[index];
        activeStage = Instantiate(stageData.stagePrefab, stageParent);

        Transform stageSpawn = activeStage.transform.Find("PlayerSpawn");
        if (stageSpawn != null)
        {
            PlayerMovement player = GameObject.FindWithTag("Player")?.GetComponent<PlayerMovement>();
            if (player != null)
                player.TeleportToSpawn(stageSpawn);
        }

        // Spawn monsters with scaling
        SpawnZone[] spawnZones = activeStage.GetComponentsInChildren<SpawnZone>();
        if (spawnZones.Length == 0) return;

        for (int i = 0; i < stageData.mosnters.Length; i++)
        {
            GameObject monsterPrefab = stageData.mosnters[i];

            int baseCount = stageData.monsterCount[i];
            int scaledCount = Mathf.Min(baseCount + loopCount, 15);

            for (int j = 0; j < scaledCount; j++)
            {
                SpawnZone zone = spawnZones[j % spawnZones.Length];
                Vector3 spawnPos = GetSpawnPosition(zone);

                GameObject monster = Instantiate(monsterPrefab, spawnPos, Quaternion.identity, activeStage.transform);
                activeMonsters.Add(monster);
            }
        }
        OnStageUpdated?.Invoke(stageNumber, MonstersRemaining);
    }

    private Vector3 GetSpawnPosition(SpawnZone zone)
    {
        Vector3 spawnPos = zone.GetRandomPosition();
        if (Physics.Raycast(spawnPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
            spawnPos.y = hit.point.y;
        return spawnPos;
    }

    public void MonsterDefeated(GameObject monster)
    {
        if (activeMonsters.Contains(monster))
        {
            activeMonsters.Remove(monster);
            OnStageUpdated?.Invoke(stageNumber, MonstersRemaining);
            Destroy(monster);

            if (activeMonsters.Count == 0)
                StageCleared();
        }
    }

    private void StageCleared()
    {
        StageBuilder stageData = stages[currentStageIndex];
        if (stageClearUI != null)
            stageClearUI.ShowRewards(stageData.moneyReward, (int)stageData.expReward);
    }

    public void LoadNextStage()
    {
        stageNumber++;
        currentStageIndex++;

        if (currentStageIndex >= stages.Length)
        {
            currentStageIndex = 0;
            loopCount++;
        }

        if (CharacterManager.Instance.Player != null && CharacterManager.Instance.Player.condition != null)
        {
            CharacterManager.Instance.Player.condition.FullRecover();
        }

        LoadStage(currentStageIndex);
    }
}
