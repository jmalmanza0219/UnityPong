using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    public TextMeshProUGUI winText;

    void Update()
    {
        //if (GameManager.Singleton == null) return;

        if (GameManager.Instance == null) return;        

        leftText.text = GameManager.Instance.leftScore.Value.ToString();
        rightText.text = GameManager.Instance.rightScore.Value.ToString();

        if (GameManager.Instance.gameOver.Value)
        {
           int w = GameManager.Instance.winner.Value;
           winText.text = (w == 1) ? "LEFT WINS!" :
                          (w == 2) ? "RIGHT WINS!" :
                          "GAME OVER!";
        }
        else
        {
            winText.text = "";
        }
    }
}
