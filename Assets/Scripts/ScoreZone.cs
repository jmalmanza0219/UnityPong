using UnityEngine;
using Unity.Netcode;

public class ScoreZone : MonoBehaviour
{
    public enum ZoneSide { Left, Right }
    public ZoneSide zoneSide;

    private void OnTriggerEnter2D(Collider2D other)
{
    if (!NetworkManager.Singleton.IsServer) return;
    if (!other.CompareTag("Ball")) return;
    if (GameManager.Instance == null) return;
    if (GameManager.Instance.gameOver.Value) return;

    // left side trigger -> right scores
    if (transform.position.x < 0f)
        GameManager.Instance.AddRightScore();
    else
        GameManager.Instance.AddLeftScore();
}

}
