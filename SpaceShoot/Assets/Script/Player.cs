using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //����Ƿɻ�
    [SerializeField]
    private float Force = 5f;
    private float ShooterDistance = 0.5f; //�ӵ�����λ��ƫ����
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


        //��R���¼��س���
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }


        //��ȡ���λ��
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //��ȡ����
        dir = (mouse - transform.position).normalized;

        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //2D�ɻ�����ͼ����ת�����λ��
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        //����
        rb.AddForce(dir * Force);

        _cooldown -= Time.deltaTime;
        if (_cooldown <= 0)
        {
            //�����ӵ�
            Instantiate(bulletPrefab, transform.position+transform.up* ShooterDistance, transform.rotation);
            _cooldown = 1f / fireRate;

        }


    }
}


