using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class FPSGameManager : MonoBehaviour
{
    public static FPSGameManager instance;
    public GameObject enemyprefab;
    public GameObject bulleytprefab;
    public GameObject boomParticle;

    private EntityManager _manager;
    //预加载
    private BlobAssetStore _blobAssetStore;
    private GameObjectConversionSettings _settings;

    public Entity enemyEntity;
    public Entity bulletEntity;
    public Entity boomEntity;
  
    
    public Entity test;
    void Start()
    {
        
        instance = this;
        _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _blobAssetStore = new BlobAssetStore();
        _settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
        enemyEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(enemyprefab, _settings);
        bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulleytprefab, _settings);
        boomEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(boomParticle, _settings);

        test = _manager.Instantiate(boomEntity);

        Translation translation = new Translation
        {
            Value = float3.zero
        };

        _manager.SetComponentData(test, translation);
      
        
    }

    public void PlayShoot()
    {

    }
    public void PlayBoom()
    {

    }

    public void PlayBehit()
    {

    }


    private void OnDestroy()
    {
        if (_blobAssetStore != null)
            _blobAssetStore.Dispose();

    }
}
