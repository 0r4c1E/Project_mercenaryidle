using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillEffectImage : MonoBehaviour
{
    public Image ilust;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI skillName;

    public void SetEffect(Character character, Skill skill)
    {
        ilust.sprite = character.standing;
        characterName.text = character.name;
        skillName.text = skill.name;
    }
}
