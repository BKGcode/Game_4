using UnityEngine;
using Databox;

public class DatabaseManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private DataboxObject database; // Assign your preconfigured DataboxObject in the Inspector

    public static DatabaseManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern to maintain a single DatabaseManager instance across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load the database configured in the editor
        if (database != null)
        {
            database.LoadDatabase();
            Debug.Log("<color=green>✅ Database loaded!</color>");
        }
        else
        {
            Debug.LogError("❌ Database object not assigned!");
        }
    }

    public void SaveDatabase()
    {
        if (database != null)
        {
            database.SaveDatabase();
            Debug.Log("<color=cyan>💾 Database saved!</color>");
        }
        else
        {
            Debug.LogError("❌ Database object not assigned!");
        }
    }

    private void OnApplicationQuit()
    {
        SaveDatabase();
    }
}
