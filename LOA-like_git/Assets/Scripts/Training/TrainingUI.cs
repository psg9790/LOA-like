
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class TrainingUI : MonoBehaviour
{
    public Player player;
    public PlayerSkillManager playerSkillManager;

    [BoxGroup("StatFix")] public GameObject statPopup;
    [BoxGroup("StatFix")] public TextMeshProUGUI[] curStats;
    [BoxGroup("StatFix")] public TMP_InputField[] targetStatsInput;
    [BoxGroup("StatFix")] public TextMeshProUGUI[] specificDifference;

    [BoxGroup("Dummy")] public GameObject dummyPrefab;
    [BoxGroup("Dummy")] public GameObject HPbarGO;
    [BoxGroup("Dummy")] public Image HPbarFill;
    [BoxGroup("Dummy")] public TextMeshProUGUI HPbarReal;
    [BoxGroup("Dummy")] public Monster monster;


    private void Update()
    {
        if (monster != null)
        {
            if (!HPbarGO.activeSelf)
                HPbarGO.SetActive(true);

            HPbarFill.fillAmount = monster.curHealth / monster.initHealth;
            HPbarReal.text = monster.curHealth + " / " + monster.initHealth;
        }
        else
        {
            if (HPbarGO.activeSelf)
                HPbarGO.SetActive(false);
        }
    }
    public void SpawnDummyButton()
    {
        if (monster == null)
        {

            GameObject _monsterGo = Instantiate(dummyPrefab, Vector3.zero + Vector3.up * 2.384186e-07f, dummyPrefab.transform.rotation);
            Monster _monster = _monsterGo.GetComponent<Monster>();
            monster = _monster;
        }
        else
        {
            monster.curHealth = monster.initHealth;
        }
    }
    public void RemoveDummyButton()
    {
        Destroy(monster.gameObject);
        monster = null;
    }

    public void RestoreCoolDownButton()
    {
        for (int x = 0; x < playerSkillManager.active.Count; x++)
        {
            playerSkillManager.active[x] = true;
        }
    }
    public void RestoreResourceButton()
    {
        playerSkillManager.curGreenGage = PlayerSkillManager.greenGage;
        playerSkillManager.curYellowGage = PlayerSkillManager.yellowGage;
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    private void RefreshCurStatsText()
    {
        curStats[0].text = player.playerStats.crit.ToString();
        curStats[1].text = player.playerStats.specialization.ToString();
        curStats[2].text = player.playerStats.swiftness.ToString();
    }
    public void StatFixStatsButton()
    {
        statPopup.SetActive(true);
        RefreshCurStatsText();
    }
    public void AdjustTargetStatsButton()
    {
        // ???????? ??????????
        player.playerStats.crit = int.Parse(targetStatsInput[0].text);
        player.playerStats.specialization = int.Parse(targetStatsInput[1].text);
        player.playerStats.swiftness = int.Parse(targetStatsInput[2].text);
        player.RefreshBonus();

        specificDifference[0].text = "+" + player.criticalBonusPercent.ToString() + "%";
        specificDifference[1].text = "+" + player.greenSkillBonusPercent.ToString() + "%";
        specificDifference[2].text = "+" + player.speedBonusPercent.ToString() + "%";
        specificDifference[3].text = "+" + player.coolDownBonusPercent.ToString() + "%";

        RefreshCurStatsText();
        // EscapeStatFix();
    }
    public void EscapeStatFix()
    {
        statPopup.SetActive(false);
    }


}
