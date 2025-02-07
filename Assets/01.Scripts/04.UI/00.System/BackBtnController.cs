using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackBtnController : MonoBehaviour
{
    private Stack<System.Action> uiStack = new Stack<System.Action>();

    public void AddStack(System.Action action)
    {
        uiStack.Push(action);
    }
    public void CloseUI()
    {
        System.Action action = uiStack.Pop();
        action();
    }
    public void ResetAll()
    {
        uiStack.Clear();
    }
}
