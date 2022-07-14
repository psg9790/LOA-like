using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Player Skill Type")]
public class PlayerSkillType : SerializedScriptableObject
{
    public string skillName;
    public float coolDown;

    public float yellowCost;
    public float yellowEarn;
    public float greenCost;
    public float greenEarn;

}
