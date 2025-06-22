using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Control : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void GameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void HomeScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
