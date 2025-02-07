using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public List<BottomAreaBtn> bottomBtns = new List<BottomAreaBtn>();
    public List<MenuPage> menuPages = new List<MenuPage>();

    public List<TextMeshProUGUI> moneys = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> dias = new List<TextMeshProUGUI>();

    private void Start()
    {
        SetPlayerMoney();
        SetPlayerDia();
        SetBottomBtns();
    }

    public void SetPlayerMoney()
    {
        for(int i = 0; i < moneys.Count; i++)
        {
            moneys[i].text = CurrencyFormatter.FormatCurrency(DataManager.inst.player.money);
        }
    }
    public void SetPlayerDia()
    {
        for(int i = 0; i < dias.Count; i++)
        {
            dias[i].text = CurrencyFormatter.FormatCurrency(DataManager.inst.player.dia);
        }
    }


    public void SetBottomBtns()
    {
        for(int i = 0; i < bottomBtns.Count; i++)
        {
            int index = i;
            bottomBtns[index].myBtn.onClick.RemoveAllListeners();
            bottomBtns[index].myBtn.onClick.AddListener(() =>
            {
                if (menuPages[index] != null) menuPages[index].gameObject.SetActive(true);
                GameManager.inst.backBtnController.AddStack(() =>
                {
                    if (menuPages[index] != null) menuPages[index].gameObject.SetActive(false);
                });
            });
        }
    }
}
