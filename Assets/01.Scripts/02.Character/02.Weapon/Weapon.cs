using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "CharacterData", menuName = "StudioParu/Equip/Weapon")]
public class Weapon : ScriptableObject, IIdentifiable
{
    [Title("Main Data")]
    public string id;
    public string ID => id;

    [Title("Attack Type (0 : Normal, 0.5 : Range, 1 : Magic")]
    public float attackType;

    [Title("Status")]
    public float range;
    public float speed;
    public int hitCount;
}
