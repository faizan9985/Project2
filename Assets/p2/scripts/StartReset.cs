using UnityEngine;

public class StartReset : MonoBehaviour
{
    [SerializeField] ObjectManagement _objectManagementScript;
    [SerializeField] private DrawUI _drawUIScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetGames();
    }

    public void ResetGames()
    {
        _drawUIScript.CloseInfo();
        _drawUIScript.SetScreenStart();
        _drawUIScript.CloseWinLoose();
        _objectManagementScript.SetPanels("on");
        _objectManagementScript.UpdateButtons();
        _objectManagementScript.UpdateDetectableObjects();
    }

    public void StartGames(int _gameType) // Bingo = 0
    {
        _objectManagementScript.UpdateButtons();
        _objectManagementScript.UpdateDetectableObjects();
        _drawUIScript.CloseWinLoose();
        _drawUIScript.SetScreenMain();
        _objectManagementScript.SetGameType(_gameType);
        if (_gameType == 0)
        {  
            _drawUIScript.StartTimer(true);
        }
    }
}
