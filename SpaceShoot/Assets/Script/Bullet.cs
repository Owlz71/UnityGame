using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 dir;
    private Vector3 mouse;

    //public float bulletForce=100f;
    public float bulletSpeed = 20f;
    Rigidbody2D rb;
    private void Start()
    {

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //实例化调用
        //获取鼠标位置
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //获取方向
        dir = (mouse - transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        dir = (mouse - transform.position).normalized;
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = dir * bulletSpeed;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Border")){
            Debug.Log("消失");
            Destroy(gameObject);
            //加分

        }
    }

}
