using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //玩家是飞机
    [SerializeField]
    private float Force = 5f;
    private float ShooterDistance = 0.5f; //子弹生成位置偏移量
    #region Component
    private Rigidbody2D rb;
    public GameObject bulletPrefab;
    #endregion

    private Vector3 mouse;
    private Vector2 dir;
    float angle;

    #region Bullet
    [SerializeField]
    private float fireRate = 30f;
    private float _cooldown;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {


        //按R重新加载场景
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }


        //获取鼠标位置
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //获取方向
        dir = (mouse - transform.position).normalized;

        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //2D飞机的贴图朝向转向鼠标位置
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        //受力
        rb.AddForce(dir * Force);

        _cooldown -= Time.deltaTime;
        if (_cooldown <= 0)
        {
            //生成子弹
            Instantiate(bulletPrefab, transform.position+transform.up* ShooterDistance, transform.rotation);
            _cooldown = 1f / fireRate;

        }


    }
}


