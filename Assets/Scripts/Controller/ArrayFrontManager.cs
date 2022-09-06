using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics.Systems;
using Unity.Transforms;

//阵法管理
public class ArrayFrontManager : MonoBehaviour
{
    public static ArrayFrontManager Instance;
    public Transform parent;

    public SpriteRenderer spriteRenderer;
    //位置列表
    public List<float2> posList;

    [Header("Drawing")]
    public int drawDensity;//密度
    public float disperseMin;//最小分散距离

    //图片位置的长宽
    private int width;
    private int height;

    //模型
    public GameObject swordPrefab;
    public float RotateSpeed;

    public float swardDmaage;

    private EntityManager _manager;
    //高速缓存
    private BlobAssetStore _blobAssetStore;
    private GameObjectConversionSettings _settings;

    private Entity swordEntity;
    public Entity TargetPos;

    Unity.Physics.RaycastHit raycastHit;
    private bool isGo = false;
    EntityArchetype tempAchetype;

    private void Awake()
    {
        Instance = this;
    }

    private void InitPixel()
    {
        width = spriteRenderer.sprite.texture.width;
        height = spriteRenderer.sprite.texture.height;
        //Debug.Log("图片宽度" + width + "图片高度" + height);
        GetPixelPos();
    }
    /// <summary>
    /// 阵法生成
    /// </summary>
    private void InitArrayFront()
    {
        InitPixel();
        raycastHit = new Unity.Physics.RaycastHit();
        _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        if (_blobAssetStore == null)
            _blobAssetStore = new BlobAssetStore();
        _settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
        swordEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(swordPrefab, _settings);

        Debug.Log("出错吗");
        tempAchetype = _manager.CreateArchetype(
             typeof(Translation),
             typeof(LocalToWorld),
             typeof(Rotation),
             typeof(RotateTag),
             typeof(Target),
             typeof(TempEntityTag)
             );

    }

    public void CreatArayFront()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitArrayFront();
            BurstGenerateSword();
        }
    }

    private void OnDestroy()
    {
        if (_blobAssetStore != null)
            _blobAssetStore.Dispose();
    }

    #region TempEntity

    public void SpawnNewSword(float2 pos, Entity prefabEntity)
    {
        Entity newSword = _manager.Instantiate(swordEntity);

        Translation ballTrans = new Translation
        {
            Value = new float3(pos.x, pos.y, -2)
        };

        float3 temp;
        float randomSpeed = UnityEngine.Random.Range(4f, 7f);
        temp = float3.zero;

        Target target = new Target
        {
            isGo = false,
            Tpos = temp,
            randomSpeed = randomSpeed,
            targetTempentity = prefabEntity,
            damage = swardDmaage,
            //targetPos = parent
        };

        _manager.AddComponentData(newSword, ballTrans);
        _manager.AddComponentData(newSword, target);
    }

    /// <summary>
    /// 生成临时Entity
    /// </summary>
    /// <param name="pos">生成坐标</param>
    /// <returns></returns>
    private Entity SpawnTempEntity(float2 pos)
    {
        Entity tempEntity = _manager.CreateEntity(tempAchetype);

        Target target2 = new Target
        {
            isGo = false,
            Tpos = float3.zero,
            damage = swardDmaage,
            //targetPos = parent
        };

        Translation tempTrans = new Translation
        {
            Value = new float3(pos.x, pos.y, - 2)
        };

        //为临时实体添加属性
        _manager.SetComponentData(tempEntity, target2);
        _manager.SetComponentData(tempEntity, tempTrans);

        return tempEntity;

    }
    #endregion

    //生成大量实体
    public void BurstGenerateSword()
    {
        Debug.Log(posList.Count);

        for (int i = 0; i < posList.Count; i++)
        {
            Entity temp = SpawnTempEntity(posList[i]);
            SpawnNewSword(posList[i], temp);
        }
    }


    public void GetPixelPos()
    {
        int halfHeight = height / 2;
        int halfWidth = width / 2;
        float2 tempPos;
        posList.Clear();
        for (int i = 0; i < height; i += drawDensity)
        {
            for (int j = 0; j < width; j += drawDensity)
            {
                //获取生成坐标
                Color32 c = spriteRenderer.sprite.texture.GetPixel(j, i);
                tempPos.y = (j - halfHeight) * disperseMin;
                //如果不透明 加入位置列表
                if (c.a != 0)
                {
                    tempPos.x = (i - halfWidth) * disperseMin;
                    posList.Add(tempPos);
                }
            }
        }

        Debug.Log("点阵数量" + posList.Count);
    }
}
