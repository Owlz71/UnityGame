using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.GPUDriven;

public class GameManager :MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject enemyPrefab;
    private float time = 2f; // 每隔2秒生成一个敌人
    private float timer;
    public int Score = 0;
    public void AddScore(int value)
    {
        Score += value;

    }




    void CreateEnemy()
    {

        Debug.Log("进入CreateEnemy方法");

        // 随机选择一个边界生成敌人
        Vector2 spawnPosition = RandomSpawnPosition(1f);

        // 随机生成朝向
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));


        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, spawnPosition, randomRotation);

        }

    }
    Vector2 RandomSpawnPosition(float extraDistance = 1f) // extraDistance 表示在边界外多远生成
    {
        float minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        float maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

        int side = Random.Range(0, 4); // 0=上, 1=下, 2=左, 3=右
        Vector2 spawnPos = Vector2.zero;

        switch (side)
        {
            case 0: // 上
                spawnPos = new Vector2(Random.Range(minX, maxX), maxY + extraDistance);
                break;
            case 1: // 下
                spawnPos = new Vector2(Random.Range(minX, maxX), minY - extraDistance);
                break;
            case 2: // 左
                spawnPos = new Vector2(minX - extraDistance, Random.Range(minY, maxY));
                break;
            case 3: // 右
                spawnPos = new Vector2(maxX + extraDistance, Random.Range(minY, maxY));
                break;
        }
        return spawnPos;
    }

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; // 减少计时器
        if (timer<=0)
        {
            CreateEnemy();
            timer=time; // 重置计时器
        }
    }
}
