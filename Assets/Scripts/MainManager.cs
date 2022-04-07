using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;

    public int LineCount = 6;

    public Rigidbody Ball;

    [SerializeField]
    public string username;

    public Text ScoreText;

    public GameObject GameOverText;

    public Text highName;

    private bool m_Started = false;

    private int m_Points;

    private static int bestScore;

    private static string bestName;

    private bool m_GameOver = false;

    private void Awake()
    {
        Load();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("main")
        )
        {
            checkBest();
            const float step = 0.6f;
            int perLine = Mathf.FloorToInt(4.0f / step);

            int[] pointCountArray = new [] { 1, 1, 2, 2, 5, 5 };
            for (int i = 0; i < LineCount; ++i)
            {
                for (int x = 0; x < perLine; ++x)
                {
                    Vector3 position =
                        new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                    var brick =
                        Instantiate(BrickPrefab, position, Quaternion.identity);
                    brick.PointValue = pointCountArray[i];
                    brick.onDestroyed.AddListener (AddPoint);
                }
            }
            username = MasterDataHandler.Instance.playerName;
            ///Debug.Log (username);
            bestScore = MasterDataHandler.Instance.bestScore;
            Load();
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("main")
        )
        {
            ///Load();
            if (!m_Started)
            {
                ///Load();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Started = true;
                    float randomDirection = Random.Range(-1.0f, 1.0f);
                    Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                    forceDir.Normalize();

                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                }
            }
            else if (m_GameOver)
            {
                checkBest();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager
                        .LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                ///Load();
                ///Save(username,bestScore);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        checkBest();
        GameOverText.SetActive(true);
    }

    public void checkBest()
    {
        highName.text = bestName + ": " + bestScore;
        ///Debug.Log (username);
        if (m_Points > bestScore)
        {
            ///Debug.Log("checked");
            bestScore = m_Points;
            bestName = username;
            highName.text = bestName + ": " + bestScore;
            Save (bestName, bestScore);
        }
    }

    public void setBest()
    {
        highName.text = username + ": " + bestScore;
    }

    public void Save(string username, int bestScore)
    {
        SaveData data = new SaveData();

        data.topPlayer = bestName;
        data.topScore = bestScore;

        string json = JsonUtility.ToJson(data);
        File
            .WriteAllText(Application.persistentDataPath + "/savefile.json",
            json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestName = data.topPlayer;
            bestScore = data.topScore;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int topScore;

        public string topPlayer;
    }
}
