using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BarParu;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    public BackBtnController backBtnController;

    public PartyController party;
    public CharacterController player;
    public Transform hitParents;

    private void Awake()
    {
        if (inst == null) inst = this; // ΩÃ±€≈Ê

    }
    private void Start()
    {

        ParuPool.inst.ObjectPool();
    }
}
