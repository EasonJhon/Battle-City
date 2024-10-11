using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //����ֵ
    public int Grade = 0;
    public float moveSpeed = 3;
    public float FireAttack = 10;
    private Vector3 bullectEulerAngles;
    private float timeVal;
    private float defendTimeval = 3;

    private bool isDefended = true;
    public bool isMoving = true;

    //����
    private SpriteRenderer sr;
    public Sprite[] tankSpriteGrade1;
    public Sprite[] tankSpriteGrade2;
    public Sprite[] tankSpriteGrade3;
    public Sprite[] tankSprite;
    public GameObject bullectPrefab;
    public GameObject explosionPrefab;
    public GameObject defendEffectPrefab;
    public AudioSource moveAudio;
    public AudioClip[] tankAudio;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tankSprite = tankSpriteGrade1;
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.tag == "bonuslife")
        {
            MapCreation.Instance.RemoveProp(other.gameObject);
            PlayerManager.Instance.lifeValue++;
        }
        else if (other.tag == "bonussheild")
        {
            MapCreation.Instance.RemoveProp(other.gameObject);
            PlayerManager.Instance.OnPropFreeze();
        }
        else if (other.tag == "fort")
        {
            MapCreation.Instance.RemoveProp(other.gameObject);
            MapCreation.Instance.HomeWallToBarriar();
        }
        else if (other.tag == "bomb")
        {
            MapCreation.Instance.RemoveProp(other.gameObject);
            PlayerManager.Instance.RemoveAllEnemy();
        }
        else if (other.tag == "Star")
        {
            MapCreation.Instance.RemoveProp(other.gameObject);
            Grade++;
            if (Grade >= 2)
            {
                Grade = 2;
            }
            
            FireAttack = Grade switch
            {
                0 => 12,
                1 => 14,
                2 => 16,
                _ => FireAttack
            };
            tankSprite = Grade switch
            {
                0 => tankSpriteGrade1,
                1 => tankSpriteGrade2,
                2 => tankSpriteGrade3,
                _ => tankSprite
            };
        }
        else if (other.tag == "helmet")
        {
            MapCreation.Instance.RemoveProp(other.gameObject);
            isDefended = true;
            defendTimeval = 3;
        }
    }

    void Update()
    {
        //�Ƿ����޵�״̬
        if (isDefended)
        {
            defendEffectPrefab.SetActive(true);
            defendTimeval -= Time.deltaTime;
            if (defendTimeval < 0)
            {
                isDefended = false;
                defendEffectPrefab.SetActive(false);
            }
        }


        //������CD
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

        Move();


        //������VD
        if (timeVal >= 0.4f)
        {
            Attack();
        }
        else
        {
            timeVal += Time.fixedDeltaTime;
        }
    }

    //̹�˵Ĺ�������
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(bullectPrefab, transform.position,
                Quaternion.Euler(transform.eulerAngles + bullectEulerAngles));
            bullet.GetComponent<Bullect>().SetBulletSpeed(FireAttack);
            timeVal = 0;
        }
    }


    //̹�˵��ƶ�����

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);

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

        if (Mathf.Abs(v) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }

        if (v != 0)
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);

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

        if (Mathf.Abs(v) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.clip = tankAudio[0];

            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
    }

    //̹�˵���������
    private void Die()
    {
        if (isDefended)
        {
            return;
        }

        PlayerManager.Instance.isDead = true;
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}