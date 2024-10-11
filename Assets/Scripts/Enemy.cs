using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //����ֵ
    public float moveSpeed = 3;
    private Vector3 bullectEulerAngles;
    private float v = -1;
    private float h;

    //����
    private SpriteRenderer sr;
    public Sprite[] tankSprite;
    public GameObject bullectPrefab;
    public GameObject explosionPrefab;

    //��ʱ��
    private float timeVal;
    private float timeValChangeDirection = 4;

    private bool isFreeze;

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.AllEnemyObj.Add(gameObject);
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.isGameover)
        {
            return;
        }
        //������ʱ����
        if (timeVal >= 0.4f)
        {
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (PlayerManager.Instance.isGameover)
        {
            return;
        }
        if (isFreeze)
        {
            return;
        }

        Move();
    }

    //̹�˵Ĺ�������
    private void Attack()
    {
        if (isFreeze)
        {
            return;
        }

        Instantiate(bullectPrefab, transform.position,
            Quaternion.Euler(transform.eulerAngles + bullectEulerAngles));
        timeVal = 0;
    }


    //̹�˵��ƶ�����

    private void Move()
    {
        if (timeValChangeDirection >= 4)
        {
            int num = Random.Range(0, 8);
            if (num > 5)
            {
                v = -1;
                h = 0;
            }
            else if (num == 0)
            {
                v = 1;
                h = 0;
            }
            else if (num > 0 && num <= 2)
            {
                h = -1;
                v = 0;
            }
            else if (num > 2 && num <= 4)
            {
                h = 1;
                v = 0;
            }

            timeValChangeDirection = 0;
        }
        else
        {
            timeValChangeDirection += Time.fixedDeltaTime;
        }


        if (v < 0)
        {
            sr.sprite = tankSprite[2];
            bullectEulerAngles = new Vector3(0, 0, -180);
        }
        else if (v > 0)
        {
            sr.sprite = tankSprite[0];
            bullectEulerAngles = new Vector3(0, 0, 0);
        }

        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);

        if (v != 0)
        {
            return;
        }


        if (h < 0)
        {
            sr.sprite = tankSprite[3];
            bullectEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            sr.sprite = tankSprite[1];
            bullectEulerAngles = new Vector3(0, 0, -90);
        }

        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);
    }

    //̹�˵���������
    private void Die()
    {
        PlayerManager.Instance.playerscore++;
        //������ը��Ч
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        //����
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            timeValChangeDirection = 4;
        }
    }

    public void OnPropFreeze()
    {
        isFreeze = true;
    }

    public void OnCancelFreeze()
    {
        isFreeze = false;
    }
}