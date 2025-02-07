using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu(fileName = "CharacterData", menuName = "StudioParu/Equip/Class")]
public class ClassType : ScriptableObject, IIdentifiable
{
    [Title("Main Data")]
    public string id;
    public string ID => id;

    [Title("Resources")]
    public Sprite icon;
}
