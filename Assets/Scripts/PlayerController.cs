using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Ball ball;
    public Button throwButton;
    public Text scoreText;
    private int _score;

    public void AddScore()
    {
        _score++;

        SetScore(_score);
    }

    private void Start()
    {
        SetScore(_score);
    }

    private void OnEnable()
    {
        throwButton.onClick.AddListener(OnThrowClick);
    }

    private void OnDisable()
    {
        throwButton.onClick.RemoveListener(OnThrowClick);
    }

    private void OnThrowClick()
    {
        var force = new Vector2(4, 12);

        ball.ApplyForce(force);
    }

    private void SetScore(int score)
    {
        scoreText.text = $"<b>Score: {score}</b>";
    }
}
