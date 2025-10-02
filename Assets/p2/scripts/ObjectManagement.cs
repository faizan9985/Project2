using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UI;
using TMPro;

//using System.Drawing.Design;

public class ObjectManagement : MonoBehaviour
{
    [SerializeField] private ObjectDetectionP2 _objectDectectionScript;
    [SerializeField] private UtilitiesP2 _utilitiesP2Script;
    [SerializeField] private DrawUI _drawUIScript;

    [SerializeField]
    [Tooltip("Detectable Objects, their Category Name")]
    public ObjectStruct[] _detectableObjects;

    [SerializeField] int _objectChoice;
    // UI Panels to manage
    [SerializeField] GameObject _itemButtonPanel;
    [SerializeField] GameObject _findMePanel;
    [SerializeField] Image _FindMeImage;
    [SerializeField] Transform _objectButtonHolder;
    public Transform[] _objectButtons;
    public int _gameType = 0; //0 = bingo, 1 = TicTacToe
    [SerializeField] private Sprite _Transparent;
    [SerializeField] private Sprite _Ink;
    [SerializeField] private Sprite _O;
    [SerializeField] private Sprite _X;
    private GameObject tempGO;
    private int _ChildCount = 0;
    private bool _OXPlayer = true; //true = O, false = X

/*
    void Start()
    {

    }
*/
    public void UpdateButtons()
    {

        // Shuffle the array
        ArrayShuffler.Shuffle(_detectableObjects);

        //Debug.Log("ChildCount " + _objectButtonHolder.childCount.ToSafeString());
        _ChildCount = _objectButtonHolder.childCount;
        _objectButtons = new Transform[_ChildCount];
        for (int i = 0; i < _ChildCount; i++)
        {
            _objectButtons[i] = _objectButtonHolder.GetChild(i);
        }
        for (int i = 0; i < _objectButtons.Length; i++)
        {
            _objectButtons[i].gameObject.GetComponent<Button>().interactable = true;
            _objectButtons[i].gameObject.GetComponent<Image>().sprite = _detectableObjects[i].CategoryImage;
            _objectButtons[i].gameObject.GetComponentInChildren<TMP_Text>().text = _detectableObjects[i].CategoryName;
            Image tempImage = Find2ndImage(i);
            tempImage.sprite = _Transparent;
        }
    }
    public void UpdateDetectableObjects()
    {
        for (int i = 0; i < _detectableObjects.Length; i++)
        {
            _detectableObjects[i].OX = "n";
        }
    }

    public void GetSelection(int choice)
    {
        List<string> _tempList = new List<string>();
        _objectChoice = choice;
        _tempList.Add(_detectableObjects[choice].CategoryName);

        _objectDectectionScript.SetList(_tempList);

        _FindMeImage.sprite = _detectableObjects[choice].CategoryImage;

        SetPanels("off");
        //set the header back button
        _drawUIScript.HeaderSetup(false);
        //turn on the timer for OX game
        if (_gameType == 1) _drawUIScript.StartTimer(true);
    }
    public void SetPanels(string todo)
    {
        switch (todo)
        {
            case "on":
                _itemButtonPanel.SetActive(true);
                _findMePanel.SetActive(false);
                //clear the objects list
                List<string> _tempList = new List<string>();
                _tempList.Add("null");
                _objectDectectionScript.SetList(_tempList);
                _drawUIScript.HeaderSetup(true);
                break;
            case "off":
                _itemButtonPanel.SetActive(false);
                _findMePanel.SetActive(true);
                break;
            default:
                break;
        }

    }
    public GameObject GetObjectElement(string ObjectCategory)
    {
        for (int i = 0; i < _detectableObjects.Length; i++)
        {
            if (_detectableObjects[i].CategoryName == ObjectCategory)
            {
                tempGO = _detectableObjects[i].reactionPanel;
                break;
            }
        }
        return tempGO;
    }

    public void SetButtonInteraction()
    {
        if (_gameType == 0)
        {
            bool isComplete = true;
            _objectButtons[_objectChoice].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = false;
            Image tempImage = Find2ndImage(_objectChoice);
            tempImage.sprite = _Ink;
            for (int i = 0; i < _objectButtons.Length; i++)
            {
                if (_objectButtons[i].gameObject.GetComponent<UnityEngine.UI.Button>().interactable == true)
                {
                    isComplete = false;
                    break;
                }
            }
            if (isComplete)
            {
                _itemButtonPanel.SetActive(false);
                _drawUIScript.WinLoose(true);
            }
        }
        else
        {
            _objectButtons[_objectChoice].gameObject.GetComponent<UnityEngine.UI.Button>().interactable = false;
            Image tempImage = Find2ndImage(_objectChoice);
            if (_OXPlayer) //true = X
            {
                tempImage.sprite = _O;
                _detectableObjects[_objectChoice].OX = "O";
            }
            else
            {
                tempImage.sprite = _X;
                _detectableObjects[_objectChoice].OX = "X";
            }
            //test for win
            //player to test
            string p = "";
            //set O X test value
            if (_OXPlayer) p = "O";
            else p = "X";
            TestOX(p);
            //update the player selection
            _OXPlayer = !_OXPlayer;
            _drawUIScript.SetOXPlayerInfo(_OXPlayer);
            //stop the timer
            _drawUIScript.StartTimer(false);
        }

    }
    public void SetGameType(int val)
    {
        _gameType = val;
        _drawUIScript.SetGameInfo(_gameType);
    }
    private Image Find2ndImage(int val)
    {
        Image[] tempImages = _objectButtons[val].gameObject.GetComponentsInChildren<Image>();
        Image tempImage = tempImages[1];

        return tempImage;
    }
    
    public void TestOX(string p)
    {
        Debug.Log("0. " + p.ToString());
        bool process = true;
        //cells to test
        int a = -1;
        int b = -1;
        int c = -1;

        //test for match being a draw
        int TotalTicks = 0;
        for (int i = 0; i < _objectButtons.Length; i++)
        {
            Debug.Log("1. " + TotalTicks.ToString());
            if (_detectableObjects[i].OX != "n")
            {
                TotalTicks = 1 + TotalTicks;
                Debug.Log("2. " + TotalTicks.ToString());
            }
        }
        if (TotalTicks > 7)
        {
        //end game in draw
            _drawUIScript.WinLoose(false);
            return;
        }
        
        //test a player's move for a win
        for (int i = 0; i < 8; i++)
        {
            process = true;
            switch (i)
            {
                case 0:
                    a = 0;
                    b = 1;
                    c = 2;
                    break;
                case 1:
                    a = 3;
                    b = 4;
                    c = 5;
                    break;
                case 2:
                    a = 6;
                    b = 7;
                    c = 8;
                    break;
                case 3:
                    a = 0;
                    b = 3;
                    c = 6;
                    break;
                case 4:
                    a = 1;
                    b = 4;
                    c = 7;
                    break;
                case 5:
                    a = 2;
                    b = 5;
                    c = 8;
                    break;
                case 6:
                    a = 0;
                    b = 4;
                    c = 8;
                    break;
                case 7:
                    a = 2;
                    b = 4;
                    c = 6;
                    break;
                default:
                    process = false;
                    break;
            }
            if (process)
            {
                if (_detectableObjects[a].OX == p && _detectableObjects[b].OX == p && _detectableObjects[c].OX == p)
                {
                    _drawUIScript.WinLoose(true);
                    return;
                }
            }
        }

    }

        
    }
