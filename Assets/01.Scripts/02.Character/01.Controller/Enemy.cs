using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "EnemyData", menuName = "StudioParu/Enemy")]
public class Enemy : ScriptableObject, IIdentifiable
{
    [Title("Main Data")]
    public string id;
    public string ID => id;
    public Type.Tier _tier;
    public string _name;
    [Title("Battle Type")]
    public ClassType _class;
    public Weapon _weapon;
    public Type.MainStatus _mainStatus;
    public Element _element;

    public CharacterController prefab;
}