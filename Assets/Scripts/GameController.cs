using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public delegate void DelegatsOfResult();
    public event DelegatsOfResult event_WonOfGame;
    public event DelegatsOfResult event_LoseOfGame;

    [Header("Scripts")]
    [SerializeField] private ShowResults_UI ShowResults_UI;
    [Space(10)]

    [Header("Game Objects")]
    [SerializeField] private GameObject[] wheelsObjects = new GameObject[3];
    [Space(10)]

    [Header("UI")]
    [SerializeField] private Button startButton;

    private IWheel[] IWheels;
    private Wheel[] WheelsScripts;

    private int generalNumberOfWheels;
    private int startNumberOfWheels;

    #region Start
    private void Start()
    {
        generalNumberOfWheels = wheelsObjects.Length;

        IWheels = new IWheel[wheelsObjects.Length];
        WheelsScripts = new Wheel[wheelsObjects.Length];

        for (int i = 0; i < wheelsObjects.Length; i++)
        {
            if (wheelsObjects[i].TryGetComponent(out IWheel iWheel))
                IWheels[i] = iWheel;

            if (wheelsObjects[i].TryGetComponent(out Wheel wheelScript))
                WheelsScripts[i] = wheelScript;
        }

        // subscribe
        Subscribe();
    }

    private void Subscribe()
    {
        for (int i = 0; i < WheelsScripts.Length; i++)
        {
            WheelsScripts[i].event_ChooseIsEnd += AddNumberForEnd;
            WheelsScripts[i].event_ChooseIsEnd += CheckForEndGame;
        }

        event_WonOfGame += ShowResults_UI.ShowWin;
        event_LoseOfGame += ShowResults_UI.ShowLose;
    }
    #endregion

    #region Button

    public void StrartGame()
    {
        startButton.interactable = false;
        startNumberOfWheels = 0;

        StartCoroutine(StartRotate());
    }

    #endregion

    #region CommandForRotate
    private IEnumerator StartRotate()
    {
        for (int i = 0; i < IWheels.Length; i++)
        {
            yield return new WaitForSeconds(0.2f);
            IWheels[i].AddTorqueImpulse();
        }

        StartCoroutine(StopRotate());
    }

    private IEnumerator StopRotate()
    {
        for (int i = 0; i < IWheels.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(1.01f, 1.21f));
            IWheels[i].StopRotate();
        }
    }
    #endregion

    private void AddNumberForEnd() => startNumberOfWheels++;

    #region EndGame
    private void CheckForEndGame()
    {
        if (generalNumberOfWheels == startNumberOfWheels)
        {
            bool isWin = true;

            startButton.interactable = true;

            for (int i = 0; i < WheelsScripts.Length; i++)
            {
                if (WheelsScripts[0].numOfChoosenWheel != WheelsScripts[i].numOfChoosenWheel)
                {
                    isWin = false;
                    break;
                }
            }

            if (isWin)
            {
                event_WonOfGame?.Invoke();
                print("You Win");
            }
            else
            {
                event_LoseOfGame?.Invoke();
                print("End Game");
            }
        }
    }

    #endregion
}
