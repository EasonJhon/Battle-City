using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreation : MonoBehaviour
{
    public static MapCreation Instance;
    //����װ�γ�ʼ����ͼ�������������
    //0.�ϼ� 1.ǽ 2.�ϰ� 3.����Ч�� 4.���� 5.�� 6.����ǽ
    public GameObject[] item;
    //�Ѿ��ж���λ�õ��б�
    private List<Vector3> itemPositionList = new List<Vector3>();
    
    private List<Vector3> homeWallPositionList = new List<Vector3>();
    private List<GameObject> homeWallObj = new List<GameObject>();
    private Dictionary<GameObject, Vector3> homeWallPosDic = new Dictionary<GameObject, Vector3>();

    private List<Vector3> propItemPossitionList = new List<Vector3>();
    private List<GameObject> propObj = new List<GameObject>();
    private Dictionary<GameObject, Vector3> propItemPosDic = new Dictionary<GameObject, Vector3>();

    private List<Vector3> enemyBornPositionList = new List<Vector3>();
    private List<GameObject> enemyBornObj = new List<GameObject>();
    private Dictionary<GameObject, Vector3> enemyBornPosDic = new Dictionary<GameObject, Vector3>();
    
    private float bonustime = 3; //30

    private bool toBarriarWall;
    private float wallToNormalTime = 5;
    private float wallToNormalTimer;
    private void Awake()
    {
        InitMap();
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void InitMap()
    {
        //ʵ�����ϼ�
        CtrateItem(item[0], new Vector3(0, -8, 0), Quaternion.identity);
        //ʵ�����ϼ�
        CreateAllHomeWall(item[1]);
        //ʵ������ǽ
        for (int i = -11; i < 12; i++)
        {
            CtrateItem(item[6], new Vector3(i, 9, 0), Quaternion.identity);
        }
        for (int i = -11; i < 12; i++)
        {
            CtrateItem(item[6], new Vector3(i, -9, 0), Quaternion.identity);
        }
        for (int i = -8; i < 9; i++)
        {
            CtrateItem(item[6], new Vector3(-11, i, 0), Quaternion.identity);
        }
        for (int i = -8; i < 9; i++)
        {
            CtrateItem(item[6], new Vector3(11, i, 0), Quaternion.identity);
        }
        //ʵ�������
        var go = Instantiate(item[3], new Vector3(-2, -8, 0), Quaternion.identity);
        go.GetComponent<Born>().createPlayer = true;
        PlayerManager.Instance.PlayerObj = go;

        //��������
        CreateEnemy(item[7], new Vector3(-10, 8, 0), Quaternion.identity);
        CreateEnemy(item[7], new Vector3(0, 8, 0), Quaternion.identity);
        CreateEnemy(item[7], new Vector3(10, 8, 0), Quaternion.identity);

        InvokeRepeating("CreateEnemy", 4, 5);


        //ʵ������ͼ
        for (int i = 0; i < 20; i++)
        {
            CtrateItem(item[1], CreateRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < 20; i++)
        {
            CtrateItem(item[2], CreateRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < 20; i++)
        {
            CtrateItem(item[4], CreateRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < 20; i++)
        {
            CtrateItem(item[5], CreateRandomPosition(), Quaternion.identity);
        }
    }

    private void CtrateItem(GameObject ctrateGameObject, Vector3 createPosition, Quaternion createRotation)
    {
        GameObject itemGo = Instantiate(ctrateGameObject, createPosition, createRotation);
        itemGo.transform.SetParent(gameObject.transform);
        itemPositionList.Add(createPosition);
    }

    #region homeWall
    private void CreateHomeWall(GameObject ctrateGameObject, Vector3 createPosition, Quaternion createRotation)
    {
        var itemGo = Instantiate(ctrateGameObject, createPosition, createRotation);
        itemGo.transform.SetParent(gameObject.transform);
        itemPositionList.Add(createPosition);
        homeWallPositionList.Add(createPosition);
        homeWallObj.Add(itemGo);
        homeWallPosDic.Add(itemGo,createPosition);
    }

    private void CreateAllHomeWall(GameObject item)
    {
        CreateHomeWall(item, new Vector3(-1, -8, 0), Quaternion.identity);
        CreateHomeWall(item, new Vector3(1, -8, 0), Quaternion.identity);
        for (var i = -1; i < 2; i++)
        {
            CreateHomeWall(item, new Vector3(i, -7, 0), Quaternion.identity);
        }
    }
    
    private void RemoveAllHomeWall()
    {
        for (var i = homeWallObj.Count - 1; i >= 0; i--)
        {
            var wall = homeWallObj[i];
            var pos = homeWallPosDic[wall];
            itemPositionList.Remove(pos);
            homeWallPositionList.Remove(pos);
            homeWallObj.Remove(wall);
            homeWallPosDic.Remove(wall);
            Destroy(wall);
        }
    }
    #endregion

    #region prop
    private void CreateProp(GameObject ctrateGameObject, Vector3 createPosition, Quaternion createRotation)
    {
        var itemGo = Instantiate(ctrateGameObject, createPosition, createRotation);
        itemGo.transform.SetParent(gameObject.transform);
        itemPositionList.Add(createPosition);
        propItemPossitionList.Add(createPosition);
        propObj.Add(itemGo);
        propItemPosDic.Add(itemGo,createPosition);
    }

    public void RemoveProp(GameObject itemGo)
    {
        var pos = propItemPosDic[itemGo];
        itemPositionList.Remove(pos);
        propItemPossitionList.Remove(pos);
        propObj.Remove(itemGo);
        propItemPosDic.Remove(itemGo);
        Destroy(itemGo);
    }
    #endregion

    #region enemy
    private void CreateEnemy(GameObject ctrateGameObject, Vector3 createPosition, Quaternion createRotation)
    {
        var itemGo = Instantiate(ctrateGameObject, createPosition, createRotation);
        itemGo.transform.SetParent(gameObject.transform);
        itemPositionList.Add(createPosition);
        enemyBornPositionList.Add(createPosition);
        enemyBornObj.Add(itemGo);
        enemyBornPosDic.Add(itemGo,createPosition);
    }

    #endregion
    
    //�������λ�õķ���
    private Vector3 CreateRandomPosition()
    {
        //������x=-10,10�����У�y=-8,8�����е�λ��
        while (true)
        {
            Vector3 createPosition = new Vector3(Random.Range(-9, 10), Random.Range(-7, 8), 0);
            if (!HasThePosition(createPosition))
            {
                return createPosition;
            }
        }
    }
    //�����ж�λ���б����Ƿ������λ��
    private bool HasThePosition(Vector3 createPos)
    {
        for (int i = 0; i < itemPositionList.Count; i++)
        {
            if (createPos == itemPositionList[i])
            {
                return true;
            }
        }
        return false;
    }
    //�������˵ķ���
    private void CreateEnemy()
    {
        if (PlayerManager.Instance.isGameover)
        {
            return;
        }
        int num = Random.Range(0, 3);
        Vector3 EnemyPos = new Vector3();
        if (num == 0)
        {
            EnemyPos = new Vector3(-10, 8, 0);
        }
        else if (num == 1)
        {
            EnemyPos = new Vector3(0, 8, 0);
        }
        else
        {
            EnemyPos = new Vector3(10, 8, 0);
        }

        CreateEnemy(item[7], EnemyPos, Quaternion.identity);
    }

    public void HomeWallToBarriar()
    {
        RemoveAllHomeWall();
        CreateAllHomeWall(item[2]);
        toBarriarWall = true;
    }

    private void HomeWallToNormal()
    {
        RemoveAllHomeWall();
        CreateAllHomeWall(item[1]);
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.isGameover)
        {
            return;
        }
        if (bonustime <= 0)
        {
            bonustime = 3;
            var num = Random.Range(0, 6);
            if (num == 0)
            {
                CreateProp(item[8], CreateRandomPosition(),Quaternion.identity);
            }
            else if (num == 1)
            {
                CreateProp(item[9], CreateRandomPosition(),Quaternion.identity);
            }
            else if (num == 2)
            {
                CreateProp(item[10], CreateRandomPosition(),Quaternion.identity);
            }
            else if (num == 3)
            {
                CreateProp(item[11], CreateRandomPosition(),Quaternion.identity);
            }
            else if (num == 4)
            {
                CreateProp(item[12], CreateRandomPosition(),Quaternion.identity);
            }
            else if (num == 5)
            {
                CreateProp(item[13], CreateRandomPosition(),Quaternion.identity);
            }
        }
        else
        {
            bonustime -= Time.deltaTime;
        }
        
        AutoToNormalWall();
    }

    private void AutoToNormalWall()
    {
        if (toBarriarWall)
        {
            if (wallToNormalTimer <= 0)
            {
                wallToNormalTimer = wallToNormalTime;
                HomeWallToNormal();
                toBarriarWall = false;
            }
            else
            {
                wallToNormalTimer -= Time.deltaTime;
            }
        }
    }
}