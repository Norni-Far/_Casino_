using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeState : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private Animator animator;

    public void DeactiveObject()
    {
        gamePanel.SetActive(false);
    }

    public void SetBool(string boolName)
    {
        animator.SetBool(boolName, false);
    }


}
