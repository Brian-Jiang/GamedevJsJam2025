using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.GameData
{
    [Serializable]
    public class SegmentInfo
    {
        [AssetsOnly]
        public GameObject prefab;
        public float length;
        public int incomingTrackCount;
        public int outgoingTrackCount;
    }
    
    [CreateAssetMenu(fileName = "SegmentConfig", menuName = "GameData/SegmentConfig", order = 0)]
    public class SegmentConfig : ScriptableObject
    {
        public SegmentInfo initialSegment;
        public List<SegmentInfo> segmentInfos;
        
        public SegmentInfo GetRandomSegment(int neededIncomingTrackCount)
        {
            var possibleSegments = segmentInfos.FindAll(segmentInfo => segmentInfo.incomingTrackCount == neededIncomingTrackCount);
            if (possibleSegments.Count == 0)
            {
                Debug.LogError($"No segments with {neededIncomingTrackCount} incoming track count found.");
                return null;
            }

            return possibleSegments[UnityEngine.Random.Range(0, possibleSegments.Count)];
        }

#if UNITY_EDITOR

        // [Button]
        // private void GetAllSegments()
        // {
        //     string[] assetGUIDs = AssetDatabase.FindAssets("t:GameObject", new[] { "Assets/Art/Prefabs/Level/Segments" });
        //     segmentInfos = new List<SegmentInfo>();
        //     foreach (var assetGUID in assetGUIDs)
        //     {
        //         var assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
        //         var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        //         var segmentInfo = new SegmentInfo
        //         {
        //             prefab = prefab
        //         };
        //         segmentInfos.Add(segmentInfo);
        //     }
        // }
        
        [Button]
        private void UpdateLengthAll()
        {
            UpdateLength(initialSegment);
            foreach (var segmentInfo in segmentInfos)
            {
                UpdateLength(segmentInfo);
            }
        }
        
        private void UpdateLength(SegmentInfo segmentInfo)
        {
            var bounds = new Bounds();
            var renderers = segmentInfo.prefab.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            if (bounds.center.y - bounds.extents.y < 0)
            {
                Debug.LogWarning($"Segment {segmentInfo.prefab.name} has negative y position.");
                Debug.LogWarning($"bounds.center.y: {bounds.center.y}, bounds.extents.y: {bounds.extents.y}");
            }
                
            segmentInfo.length = (int) bounds.size.y;
        }
                
#endif
    }
}