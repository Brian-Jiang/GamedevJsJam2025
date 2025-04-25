using System;
using System.Collections;
using System.Collections.Generic;
using CoinDash.GameLogic.Runtime.PlayerData;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class GameLevelManager : GameFacadeComponent
    {
        public Player ActivePlayer { get; private set; }
        public PlayerController PlayerController { get; }
        public InputController InputController { get; }
        public Camera MainCamera { get; private set; }
        public CinemachineCamera VirtualCamera { get; private set; }
        
        private readonly HashSet<LevelObject> levelObjects = new();
        private float topOffset;
        private int neededTrackCount;
        
        // public readonly Queue<TracksOld> activeTracks = new();
        
        public GameLevelManager(GameObject gameObject) : base(gameObject)
        {
            PlayerController = new PlayerController(this);
            InputController = new InputController(this);
        }

        public void Initialize()
        {
            // var segmentInstance = GenerateInitialSegment();
            
            // var coinInfo = GameFacade.GameDataManager.CoinConfig.coinInfos[0];
            // var playerPrefab = GameFacade.GameDataManager.PlayerConfig.playerPrefab;
            // var playerInstance = Object.Instantiate(playerPrefab);
            // playerInstance.transform.Translate(Vector3.up * 2f, Space.World);
            
            var playerInstance = GameObject.FindGameObjectWithTag("Player");
            var player = playerInstance.GetComponent<Player>();
            levelObjects.Add(player);
            ActivePlayer = player;
            
            // var tracks = segmentInstance.GetComponentInChildren<Tracks>();
            // coin.CurrentTrack = tracks;
            // coin.CurrentTrackIndex = 0;
            
            // var pathTriggerPrefab = GameFacade.GameDataManager.LevelConfig.pathTriggerPrefab;
            // var pathTriggerInstance = Object.Instantiate<GameObject>(
            //     pathTriggerPrefab, Vector3.up * 5f, Quaternion.identity);
            // var pathTrigger = pathTriggerInstance.GetComponent<PathTrigger>();
            // levelObjects.Add(pathTrigger);

            // PlayerController.MaxForce = GameFacade.GameDataManager.LevelConfig.defaultMaxForce;
            PlayerController.Power = GameFacade.GameDataManager.LevelConfig.defaultMaxPower;
            PlayerController.Score = 0;
            
            // SerializePlayerData();
            // GameFacade.PlayerDataManager.SaveAllData();

            // setup camera
            // var virtualCameraPrefab = GameFacade.GameDataManager.LevelConfig.virtualCameraPrefab;
            // var virtualCameraInstance = Object.Instantiate<GameObject>(virtualCameraPrefab);
            // VirtualCamera = virtualCameraInstance.GetComponent<CinemachineCamera>();
            // VirtualCamera.Target.TrackingTarget = ActivePlayer.transform;
            
            MainCamera = Camera.main;
            VirtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineCamera>();
            var aspectRatio = (float) Screen.width / Screen.height;
            VirtualCamera!.Lens.OrthographicSize = 6f / aspectRatio;
            VirtualCamera.Lens.OrthographicSize = 11.3f;  // TODO
            
            InputController.BindInput();
            InputController.OnMoveLeft += SwitchLeftTrack;
            InputController.OnMoveRight += SwitchRightTrack;
            InputController.OnSpeedUp += speedup =>
            {
                ActivePlayer.ChangeSpeedup(speedup);
            };
            InputController.OnInvincible += invincible =>
            {
                ActivePlayer.ChangeInvincible(invincible);
            };
            
            GameFacade.Scheduler.OnFixedUpdate += OnFixedUpdate;
            
            // load ui
            var inGameUIPrefab = GameFacade.GameDataManager.UIConfig.inGameUIPrefab;
            // var controlUIPrefab = GameFacade.GameDataManager.UIConfig.controlUIPrefab;
            GameFacade.UIManager.OpenUIView(inGameUIPrefab);
            // GameFacade.UIManager.OpenUIView(controlUIPrefab);
            
            // InputController.ToggleHandleInput(false);
        }

        // private void OnSwitchLeft(InputAction.CallbackContext context)
        // {
        //     SwitchLeftTrack();
        // }
        //
        // private void OnSwitchRight(InputAction.CallbackContext context)
        // {
        //     SwitchRightTrack();
        // }
        
        public void Restart()
        {
            // var InputActions = GameFacade.InputManager.InputActions;
            // InputActions.Player.SwitchLeft.performed -= OnSwitchLeft;
            // InputActions.Player.SwitchRight.performed -= OnSwitchRight;
            
            // foreach (var levelObject in levelObjects)
            // {
            //     Object.Destroy(levelObject.gameObject);
            // }
            
            levelObjects.Clear();
            // activeTracks.Clear();
            topOffset = 0f;
            neededTrackCount = 0;
            ActivePlayer = null;
            
            Time.timeScale = 1f;
            // Initialize();
            
            var playerInstance = GameObject.FindGameObjectWithTag("Player");
            var player = playerInstance.GetComponent<Player>();
            levelObjects.Add(player);
            ActivePlayer = player;
            
            PlayerController.Power = GameFacade.GameDataManager.LevelConfig.defaultMaxPower;
            PlayerController.Score = 0;
            
            MainCamera = Camera.main;
            VirtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineCamera>();
            var aspectRatio = (float) Screen.width / Screen.height;
            VirtualCamera!.Lens.OrthographicSize = 6f / aspectRatio;
            VirtualCamera.Lens.OrthographicSize = 11.3f;  // TODO
            
            VirtualCamera.OnTargetObjectWarped(ActivePlayer.transform, Vector3.zero);
        }
        
        // public GameObject GenerateInitialSegment()
        // {
        //     var segmentInfo = GameFacade.GameDataManager.SegmentConfig.initialSegment;
        //     neededTrackCount = segmentInfo.outgoingTrackCount;
        //     var segmentInstance = Object.Instantiate<GameObject>(
        //         segmentInfo.prefab, Vector3.up * topOffset, Quaternion.identity);
        //     var pathLevelObjects = segmentInstance.GetComponentsInChildren<LevelObject>();
        //     levelObjects.UnionWith(pathLevelObjects);
        //     topOffset += segmentInfo.length;
        //     return segmentInstance;
        // }
        
        // public float GenerateRandomSegment()
        // {
        //     var segmentInfo = GameFacade.GameDataManager.SegmentConfig.GetRandomSegment(neededTrackCount);
        //     neededTrackCount = segmentInfo.outgoingTrackCount;
        //     var segmentInstance = Object.Instantiate<GameObject>(
        //         segmentInfo.prefab, Vector3.up * topOffset, Quaternion.identity);
        //     
        //     var pathLevelObjects = segmentInstance.GetComponentsInChildren<LevelObject>();
        //     levelObjects.UnionWith(pathLevelObjects);
        //     
        //     var tracks = segmentInstance.GetComponentInChildren<Tracks>();
        //     activeTracks.Enqueue(tracks);
        //     
        //     topOffset += segmentInfo.length;
        //     return segmentInfo.length;
        // }
        
        public void SwitchLeftTrack()
        {
            if (ActivePlayer == null)
            {
                return;
            }
            
            ActivePlayer.SwitchLeftTrack();
        }
        
        public void SwitchRightTrack()
        {
            if (ActivePlayer == null)
            {
                return;
            }
            
            ActivePlayer.SwitchRightTrack();
        }
        
        public void RemoveLevelObject(LevelObject levelObject)
        {
            levelObjects.Remove(levelObject);
            Object.Destroy(levelObject.gameObject);
        }

        // private void SerializePlayerData()
        // {
        //     var playerData = GameFacade.PlayerDataManager.PlayerData;
        //     PlayerController.Serialize(playerData);
        //     
        //     playerData.levelObjects.Clear();
        //     foreach (var levelObject in levelObjects)
        //     {
        //         var levelObjectData = levelObject.Serialize();
        //         var typeName = levelObjectData.GetType().AssemblyQualifiedName;
        //         var json = JsonUtility.ToJson(levelObjectData, true);
        //         playerData.levelObjects.Add(new PolymorphicWrapper
        //         {
        //             type = typeName,
        //             json = json,
        //         });
        //     }
        //
        //     playerData.topOffset = topOffset;
        // }
        
        // private void DeserializePlayerData()
        // {
        //     var playerData = GameFacade.PlayerDataManager.PlayerData;
        //     PlayerController.Deserialize(playerData);
        //     
        //     foreach (var wrapper in playerData.levelObjects)
        //     {
        //         var type = Type.GetType(wrapper.type);
        //         if (type == null)
        //         {
        //             Debug.LogError($"Type {wrapper.type} not found");
        //             continue;
        //         }
        //
        //         var levelObjectData = (LevelObjectData) JsonUtility.FromJson(wrapper.json, type);
        //         switch (levelObjectData)
        //         {
        //             case CoinData coinData:
        //                 var coinId = coinData.coinId;
        //                 var coinInfo = GameFacade.GameDataManager.CoinConfig.coinInfos[coinId];
        //                 var gameObject = Object.Instantiate(coinInfo.prefab);
        //                 var coin = gameObject.GetComponent<Player>();
        //                 coin.Deserialize(wrapper.json);
        //                 levelObjects.Add(coin);
        //                 if (coinData.isActiveCoin)
        //                 {
        //                     ActivePlayer = coin;
        //                     coin.OnBecomeActive();
        //                 }
        //                 break;
        //             case PathData pathData:
        //                 var pathId = pathData.pathId;
        //                 var pathInfo = GameFacade.GameDataManager.PathConfig.pathInfos[pathId];
        //                 var pathInstance = Object.Instantiate(pathInfo.prefab);
        //                 var path = pathInstance.GetComponent<Path>();
        //                 path.Deserialize(wrapper.json);
        //                 levelObjects.Add(path);
        //                 break;
        //             case PathTriggerData pathTriggerData:
        //                 var pathTriggerPrefab = GameFacade.GameDataManager.LevelConfig.pathTriggerPrefab;
        //                 var pathTriggerInstance = Object.Instantiate<GameObject>(pathTriggerPrefab);
        //                 var pathTrigger = pathTriggerInstance.GetComponent<PathTrigger>();
        //                 pathTrigger.Deserialize(wrapper.json);
        //                 levelObjects.Add(pathTrigger);
        //                 break;
        //             case WallData wallData:
        //                 // var wallId = wallData.wallId;
        //                 // var wallInfo = GameFacade.GameDataManager.WallConfig.wallInfos[wallId];
        //                 var wallPrefab = GameFacade.GameDataManager.LevelConfig.wallPrefab;
        //                 var wallInstance = Object.Instantiate<GameObject>(wallPrefab);
        //                 var wall = wallInstance.GetComponent<Wall>();
        //                 wall.Deserialize(wrapper.json);
        //                 levelObjects.Add(wall);
        //                 break;
        //             case PointData pointData:
        //                 var pointPrefab = GameFacade.GameDataManager.LevelConfig.pointPrefab;
        //                 var pointInstance = Object.Instantiate<GameObject>(pointPrefab);
        //                 var point = pointInstance.GetComponent<Point>();
        //                 point.Deserialize(wrapper.json);
        //                 levelObjects.Add(point);
        //                 break;
        //             case SpotLightData spotLightData:
        //                 var spotLightPrefab = GameFacade.GameDataManager.LevelConfig.spotlightPrefab;
        //                 var spotLightInstance = Object.Instantiate<GameObject>(spotLightPrefab);
        //                 var spotLight = spotLightInstance.GetComponent<SpotLight>();
        //                 spotLight.Deserialize(wrapper.json);
        //                 levelObjects.Add(spotLight);
        //                 break;
        //             case BoosterData boosterData:
        //                 var boosterPrefab = GameFacade.GameDataManager.LevelConfig.boosterPrefab;
        //                 var boosterInstance = Object.Instantiate<GameObject>(boosterPrefab);
        //                 var booster = boosterInstance.GetComponent<Booster>();
        //                 booster.Deserialize(wrapper.json);
        //                 levelObjects.Add(booster);
        //                 break;
        //         }
        //     }
        //     
        //     topOffset = playerData.topOffset;
        // }

        private void OnFixedUpdate()
        {
            // if (skipFixedUpdate) {
            //     skipFixedUpdate = false;
            //     return;
            // }

            // if (SimulationState == SimulationState.Simulating) {
            //     var settled = true;
            //     foreach (var movingCoin in movingCoins) {  // TODO
            //         Rigidbody2D rb = movingCoin.GetComponent<Rigidbody2D>();
            //         if (Mathf.Abs(rb.linearVelocity.magnitude) > 0.1f) {
            //             settled = false;
            //             break;
            //         }
            //     }
            //
            //     if (settled) {
            //         // movingCoins.Clear();
            //         SimulationState = SimulationState.Stopped;
            //
            //         // foreach (var activeRegion in activeRegions) {
            //         //     activeRegion.TriggerEffectOnTurnEnd();
            //         // }
            //     
            //         PlayerController.HandleOnTurnSettled();
            //         
            //         SerializePlayerData();
            //         GameFacade.PlayerDataManager.SaveAllData();
            //         InputController.ToggleHandleInput(true);
            //
            //         // var currentVCam = useVcam1 ? vcam1 : vcam2;
            //         // var positionOffset = currentVCam.transform.position - ActiveCoin.Transform.position;
            //         //
            //         var offset = ActivePlayer.transform.position.y;
            //         // pathSegmentGenerator.WorldPositionAdjust(-offset);
            //         var levelObjectList = new List<LevelObject>(levelObjects);
            //         levelObjectList.Sort((levelObject1, levelObject2) => 
            //             levelObject1.transform.position.y.CompareTo(levelObject2.transform.position.y));
            //         var destroyThreshold = GameFacade.GameDataManager.LevelConfig.levelObjectDestroyThresholdDistance;
            //         foreach (var levelObject in levelObjectList) {
            //             levelObject.transform.Translate(Vector3.down * offset, Space.World);
            //             if (levelObject.transform.position.y < destroyThreshold) {
            //                 levelObjects.Remove(levelObject);
            //                 Object.Destroy(levelObject.gameObject);
            //             }
            //         }
            //         
            //         topOffset -= offset;
            //         //
            //         // // if (invincibleTurnLeft > 0) {
            //         // //     --invincibleTurnLeft;
            //         // //     if (invincibleTurnLeft == 0) {
            //         // //         EventCenter.Fire(GameEvent.OnEnterInvincible, new EnterInvincibleEvent {
            //         // //             isInvincible = false,
            //         // //         });
            //         // //     }
            //         // // }
            //         //
            //         // VirtualCamera.ForceCameraPosition(
            //         //     ActiveCoin.Transform.position + positionOffset, Quaternion.identity);
            //         CinemachineCore.OnTargetObjectWarped(ActivePlayer.transform, Vector3.down * offset);
            //         //
            //         // if (ActiveCoin.willDestroy) {
            //         //     var lastCoin = ActiveCoin;
            //         //     RespawnNewCoin();
            //         //     RemoveCoin(lastCoin);
            //         //     GameFacade.ActorManager.DestroyActor(lastCoin);
            //         // }
            //         //
            //         // SerializeLevel();
            //         
            //         OnTurnEnd?.Invoke();
            //     }
            // }
        }
    }
}