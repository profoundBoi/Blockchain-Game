using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}