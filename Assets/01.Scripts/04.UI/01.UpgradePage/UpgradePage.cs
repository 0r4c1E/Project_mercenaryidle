using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePage : MonoBehaviour
{
    [Title("Character List")]
    public Transform listParents;
    [Title("Character Page")]
    public TextMeshProUGUI _name;
    public TextMeshProUGUI _class;
    public TextMeshProUGUI _tier;
    public Image _standing;
    public Image _classIcon;
}
