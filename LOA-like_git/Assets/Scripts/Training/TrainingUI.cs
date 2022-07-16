
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using TMPro;

public class TrainingUI : MonoBehaviour
{
    public Player player;
    public PlayerSkillManager playerSkillManager;

    [BoxGroup("StatFix")] public GameObject statPopup;
    [BoxGroup("StatFix")] public TextMeshProUGUI[] curStats;
    [BoxGroup("StatFix")] public TMP_InputField[] targetStatsInput;


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
    public void StatFixButton()
    {
        statPopup.SetActive(true);
        RefreshCurStatsText();
    }
    public void AdjustTargetTextButton()
    {
        // 예외처리 해야하는데
        player.playerStats.crit = int.Parse(targetStatsInput[0].text);
        player.playerStats.specialization = int.Parse(targetStatsInput[1].text);
        player.playerStats.swiftness = int.Parse(targetStatsInput[2].text);

        EscapeStatFix();
    }
    public void EscapeStatFix()
    {
        statPopup.SetActive(false);
    }


}
