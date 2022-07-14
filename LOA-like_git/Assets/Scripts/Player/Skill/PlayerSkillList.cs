using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Player Skill List")]
public class PlayerSkillList : SerializedScriptableObject
{
    public List<PlayerSkillType> list = new List<PlayerSkillType>();
}
