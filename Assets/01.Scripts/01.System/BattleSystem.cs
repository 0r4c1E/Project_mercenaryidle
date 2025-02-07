using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem inst;

    public SkillEffectImage skillEffect;

    private void Awake()
    {
        if (inst == null) inst = this;
    }
}
