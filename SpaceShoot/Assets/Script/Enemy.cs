using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    private Vector2 dir;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 随机生成一个移动方向（朝向屏幕中心的随机点）

        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 randomTarget = Camera.main.ScreenToWorldPoint(center + new Vector3(
            Random.Range(-100, 100),  // x 方向随机偏移像素
            Random.Range(-100, 100),
            0
        ));
        randomTarget.z = 0;
        dir = (randomTarget - transform.position).normalized;

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = dir * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            GameManager.Instance.AddScore(1); // 增加分数
        }
    }
}
