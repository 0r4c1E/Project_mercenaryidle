using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitBtn : MonoBehaviour
{
    public Button myBtn;

    private void Start()
    {
        myBtn.onClick.RemoveAllListeners();
        myBtn.onClick.AddListener(() =>
        {
            GameManager.inst.backBtnController.CloseUI();
        });
    }
}
