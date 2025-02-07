using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "CharacterData", menuName = "StudioParu/Character")]
public class Character : ScriptableObject, IIdentifiable
{
    [Title("Main Data")]
    public string id;
    public string ID => id;
    public Type.Tier _tier;
    public string _name;
    [TextArea] public string _description;
    public Sprite standing;
    [Title("Battle Type")]
    public ClassType _class;
    public Weapon _weapon;
    public Type.MainStatus _mainStatus;
    public Element _element;

    public CharacterController prefab;
}
