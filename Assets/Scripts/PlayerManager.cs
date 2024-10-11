using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerObj;
    
    public List<GameObject> AllEnemyObj = new List<GameObject>();
    public float PropFreezeTime = 3;
    public bool IsPropFreeze;
    private float propFreezeTimer;

    //����ֵ
    public int lifeValue = 3;
    public int playerscore = 0;
    public bool isDead;
    public bool isDefeat;


    //����
    public GameObject born;
    public Text playerScoreText;
    public Text PlayerLifeValueText;
    public GameObject isDefeatUI;
    public bool isGameover = false;
    public GameObject gameover;

    //����

    private static PlayerManager instance;

    public static PlayerManager Instance
    {
        get { return instance; }

        set { instance = value; }
    }

    private void Awake()
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
        if (isGameover)
        {
            return;
        }


        if (isDefeat)
        {
            isDefeatUI.SetActive(true);
            isGameover = true;
            Invoke(" ReturnToTheMainMenu", 3);
            return;
        }


        if (isDead)
        {
            Recover();
        }

        AutoCancelFreeze();

        playerScoreText.text = playerscore.ToString();
        PlayerLifeValueText.text = lifeValue.ToString();
    }

    private void Recover()
    {
        if (lifeValue <= 0)
        {
            //��Ϸʧ�ܣ�����������
            isDefeat = true;
            SceneManager.LoadScene(0);
            Invoke(" ReturnToTheMainMenu", 3);
        }
        else
        {
            lifeValue -= 1;
            GameObject go = Instantiate(born, new Vector3(-2, -8, 0), Quaternion.identity);
            go.GetComponent<Born>().createPlayer = true;
            isDead = false;
        }
    }

    private void ReturnToTheMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnPropFreeze()
    {
        IsPropFreeze = true;
        foreach (var enemy in PlayerManager.Instance.AllEnemyObj)
        {
            enemy.GetComponent<Enemy>().OnPropFreeze();
        }
    }
    
    private void AutoCancelFreeze()
    {
        if (IsPropFreeze)
        {
            if (propFreezeTimer <= 0)
            {
                propFreezeTimer = PropFreezeTime;
                foreach (var enemy in PlayerManager.Instance.AllEnemyObj)
                {
                    enemy.GetComponent<Enemy>().OnCancelFreeze();
                }
                IsPropFreeze = false;
            }
            else
            {
                propFreezeTimer -= Time.deltaTime;
            }
        }
    }

    public void RemoveAllEnemy()
    {
        for (var i = 0; i < AllEnemyObj.Count; i++)
        {
            var enemy = AllEnemyObj[i];
            AllEnemyObj.Remove(enemy);
            Destroy(enemy);
        }
    }
}