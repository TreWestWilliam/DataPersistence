using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    public Text HighScoreText;
    public int m_HighScore;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        LoadHighscore();
    }

    private void Update()
    {
        if (!m_Started)
        {
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if (m_Points > m_HighScore) 
        {
            SaveHighscore();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void SaveHighscore() 
    {
        if (Names.UsersName.Equals("")) 
        {
            Names.UsersName = "Nanashi";
        }
        HighScore hs = new();
        hs.ScoreName = Names.UsersName;
        hs.ScoreNumber = m_Points;

        string json = JsonUtility.ToJson(hs);

        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }

    public void LoadHighscore() 
    {
        HighScore hs = new();
        string path = Application.persistentDataPath + "/highscore.json";
        if (File.Exists(path))
        {
            string JsonString = File.ReadAllText(path);
            hs = JsonUtility.FromJson<HighScore>(JsonString);
        }
        else 
        {
            hs.ScoreName = "None";
            hs.ScoreNumber = 0;
        }

        HighScoreText.text = $"Best Score: {hs.ScoreNumber} By {hs.ScoreName}";
        m_HighScore = hs.ScoreNumber;
    }
}

[System.Serializable]
struct HighScore 
{
    public string ScoreName;
    public int ScoreNumber;
}