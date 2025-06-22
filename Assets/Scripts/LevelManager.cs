using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform levelParent;
    public GameObject[] levelPrefabs;

    private int currentLevelIndex = 0;
    private GameObject currentLevelInstance;

    void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int index)
    {
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        currentLevelInstance = Instantiate(levelPrefabs[index], levelParent);

        Player_Control p = currentLevelInstance.GetComponentInChildren<Player_Control>();
        if (p != null)
        {
            p.SetLevelManager(this);
        }
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex < levelPrefabs.Length)
        {
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }
}
