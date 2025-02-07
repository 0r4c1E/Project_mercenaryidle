using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "CharacterData", menuName = "StudioParu/Skill")]
public class Skill : ScriptableObject, IIdentifiable
{
    [Title("Main Data")]
    public string id;
    public string ID => id;

    public Type.SkillType type;
    public bool personalSkill;
    public string name;
}
