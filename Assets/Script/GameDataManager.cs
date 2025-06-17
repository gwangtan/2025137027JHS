using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerData
{

    public List<string> collectedItems = new List<string>();
    public int stage = 1;
}
    // Start is called before the first frame update

public class GameDatamanager : MonoBehaviour
{
    public static GameDatamanager Instance;

    public PlayerData playerData;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}