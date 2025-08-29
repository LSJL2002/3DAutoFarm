using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public GameObject stageSelectUI;
    public void OpenStageSelect()
    {
        stageSelectUI.SetActive(true);
    }
}
