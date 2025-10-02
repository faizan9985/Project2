using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DrawUI : MonoBehaviour
{
    //variables
    [SerializeField] ObjectManagement _objectManagementScript;
    [SerializeField] ObjectDetectionP2 _objectDetectionP2Script;
    //[SerializeField] GameObject findMePanel;
    //[SerializeField] GameObject findMePanelButton;
    [SerializeField] private GameObject _PanelButtons;
     [SerializeField] private GameObject _PanelMain;
    [SerializeField] private GameObject _PanelStart;
    [SerializeField] private GameObject _PanelFindMe;
    [SerializeField] private GameObject[] _backButtons;
    private GameObject _targetresponsePanel;
    [SerializeField] private GameObject _PanelWin;
    [SerializeField] private GameObject _PanelWinX;
    [SerializeField] private GameObject _PanelWinO;
    [SerializeField] private GameObject _PanelLoose;
    [SerializeField] private TMP_Text _gameTitleTextbox;
    [SerializeField] private TMP_Text _playerTextbox;
    [SerializeField] private string[] _gameTitle;
    [SerializeField] private string[] _player;
    private bool _OXPlayer;
    private int _gameType;

[Header("Only use these variables if using a timer")]
    [SerializeField] private bool useTimer;
    [SerializeField] private TMP_Text _TimeText;
    [SerializeField] private Image _timerImage;
    //[SerializeField] private int _TimeStart;
    //private int _TimeExisting = 0;
    //private int _TimeNow;
    [SerializeField] private float _MaxTimeBingo = 300f;
    [SerializeField] private float _MaxTimeOX = 30f;
    [SerializeField] private string TimerEndText = "Yikes! You ran out of time!!";
    private float _currentTime = 0f;
    private bool _startTimer = false;

/*
    void Start()
    {

    }
*/
    void Update()
    {
         //timer
        if(useTimer && _startTimer) UpdateTimer(Time.deltaTime);
    }
    public void SetGameInfo(int _gameTypeVal)
    {
        _gameType = _gameTypeVal;
        _gameTitleTextbox.text = _gameTitle[_gameType];
        if (_gameType == 0)
        {
            _playerTextbox.enabled = false;
        }
        else
        {
            _playerTextbox.enabled = true;
            SetOXPlayerInfo(true);
        }
    }
    public void SetOXPlayerInfo(bool PlayerInfo)
    {
        //swapping Players [O - X] and reset the current time
        _currentTime = 0f;
        _OXPlayer = PlayerInfo;

        if (_OXPlayer)
        {
            //O PLayer
            _playerTextbox.text = _player[0];
        }
        else
        {
            //XPlayer
            _playerTextbox.text = _player[1];
        }

    }
    public void OpenInfo(string _name)
    {

        //get the element number of the Detectable Object
        _targetresponsePanel = _objectManagementScript.GetObjectElement(_name);
        //set the panels
        //findMePanel.SetActive(false);
        _PanelFindMe.SetActive(false);
        _targetresponsePanel.SetActive(true);

    }
    public void CloseInfo()
    {
        //findMePanelButton.SetActive(false);
        if (_targetresponsePanel != null)
        {
            _targetresponsePanel.SetActive(false);
        }
    }
    public void WinLoose(bool _winloose)
    {
        _PanelButtons.SetActive(false);
        CloseInfo();
        if (_gameType == 0)
        {
            if (_winloose)
            {
                _PanelWin.SetActive(true);
            }
            else
            {
                _PanelLoose.SetActive(true);
            }
        }
        else
        {
            if (_OXPlayer && _winloose)
            {
                _PanelWinO.SetActive(true);
            }
            else if (!_OXPlayer && _winloose)
            {
                _PanelWinX.SetActive(true);
            }
            else
            {
                _PanelLoose.SetActive(true);
            }
        }
    }
    public void CloseWinLoose()
    {
        _PanelWin.SetActive(false);
        _PanelWinO.SetActive(false);
        _PanelWinX.SetActive(false);
        _PanelLoose.SetActive(false);
    }
    public void SetScreenMain()
    {
        _PanelMain.SetActive(true);
        _PanelStart.SetActive(false);
    }
    public void SetScreenStart()
    {
        _PanelMain.SetActive(false);
        _PanelFindMe.SetActive(false);
        _PanelStart.SetActive(true);
    }

 public void RunBackButton()
    {
        if (_PanelFindMe.activeSelf)
        {

        }
        else
        {
            SetScreenStart();
        }
    }

 public void HeaderSetup(bool backButton)
    {
        if (backButton)
        {
            _backButtons[0].SetActive(true); //grid
            _backButtons[1].SetActive(false); //findme
        }
        else
        {
            _backButtons[0].SetActive(false); //grid
            _backButtons[1].SetActive(true); //findme
        }

    }

 void UpdateTimer(float newTime)
    {
        _currentTime += newTime;
        if (_objectManagementScript._gameType == 0) //bingo
        {
            //amount of fill
            //_currentTime += newTime;
            if (_currentTime < _MaxTimeBingo)
            {
                _timerImage.fillAmount = _currentTime / _MaxTimeBingo;
                int tempval = (int)_currentTime;
                _TimeText.text = tempval.ToString();
            }
            else
            {
                _TimeText.text = TimerEndText;
                WinLoose(false);
            }
        }
        else //tictactoe
        {
            if (_currentTime < _MaxTimeOX)
            {
                _timerImage.fillAmount = _currentTime / _MaxTimeOX;
                int tempval = (int)_currentTime;
                _TimeText.text = tempval.ToString();
            }
            else
            {
                StartTimer(false);
                _TimeText.text = TimerEndText;
                SetOXPlayerInfo(!_OXPlayer);
                _objectManagementScript.SetPanels("on");
                _objectDetectionP2Script.SetProcess();
            }
        }
        
    }

    public void StartTimer(bool StartStop)
    {
        _startTimer = StartStop;
    }
    public void StopTimerFromBackbuttonOX()
    {
        if (_gameType == 1)
        {
            _startTimer = false;
            _currentTime = 0f;
        }
    }




}
