using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour, IResetable
{
    [SerializeField] Text resultText;

    static public bool isGameOverScreenActive;


    public void ShowWinResult()
    {
        resultText.text = "You win!";
        SetActive(true);
    }

    public void ShowLoseResult()
    {
        resultText.text = "You lose!";
        SetActive(true);
    }

    public void Reset()
    {
        SetActive(false);
    }

    void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        isGameOverScreenActive = isActive;
    }
}
