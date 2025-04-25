using System.Collections.Generic;
using System.Linq;
using CoinDash.GameLogic.Runtime.PlayerData;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;
using Spline = UnityEngine.Splines.Spline;

namespace CoinDash.GameLogic.Runtime.Level
{
    public class TrackInfo
    {
        // In world position
        public float start;
        public float end;
        public SpriteShapeController spriteShapeController;
    }
    
    public class TracksOld : LevelObject
    {
        public int[] enter0;
        public int[] enter1;
        public int[] enter2;
        public int[] exit0;
        public int[] exit1;
        public int[] exit2;
        
        private List<TrackInfo> trackInfos = new List<TrackInfo>();
        private bool init;
        
        public int GetTrackCount()
        {
            InitTrackInfo();
            
            return trackInfos.Count;
        }
        
        private void InitTrackInfo()
        {
            if (init) return;
            
            init = true;
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.TryGetComponent<SpriteShapeController>(out var spriteShapeController))
                {
                    var trackInfo = GetTrackInfo(spriteShapeController);
                    trackInfos.Add(trackInfo);
                }
            }
        }

        public Vector2 GetStartPosition()
        {
            InitTrackInfo();
            
            if (trackInfos.Count == 0)
            {
                Debug.LogWarning("No track info found");
                return Vector2.zero;
            }
            
            var minY = float.MaxValue;
            var minTrackIndex = -1;
            for (var i = 0; i < trackInfos.Count; i++)
            {
                var trackInfo = trackInfos[i];
                if (trackInfo.start < minY)
                {
                    minY = trackInfo.start;
                    minTrackIndex = i;
                }
            }
            
            if (minTrackIndex == -1)
            {
                Debug.LogWarning("No track info found");
                return Vector2.zero;
            }
            
            // var foundTrackInfo = trackInfos[minTrackIndex];
            return GetTrackPosition(minTrackIndex, minY, out _);
        }

        private TangentMode GetTangentMode(ShapeTangentMode shapeTangentMode)
        {
            switch (shapeTangentMode)
            {
                case ShapeTangentMode.Linear:
                    return TangentMode.Linear;
                case ShapeTangentMode.Continuous:
                    return TangentMode.Continuous;
                case ShapeTangentMode.Broken:
                    return TangentMode.Broken;
                default:
                    return TangentMode.Linear;
            }
        }
        
        private TrackInfo GetTrackInfo(SpriteShapeController spriteShapeController)
        {
            var pointCount = spriteShapeController.spline.GetPointCount();
            
            var spline = new Spline(pointCount);
            for (var i = 0; i < pointCount; i++)
            {
                var point = spriteShapeController.spline.GetPosition(i);
                point += spriteShapeController.transform.position;
                var shapeTangentMode = spriteShapeController.spline.GetTangentMode(i);
                var tangentMode = GetTangentMode(shapeTangentMode);
                spline.Add(point, tangentMode);
            }
            
            var min = float.MaxValue;
            var max = float.MinValue;
            for (var i = 0; i < pointCount; i++)
            {
                var point = spriteShapeController.spline.GetPosition(i);
                point += spriteShapeController.transform.position;
                if (point.y < min)
                {
                    min = point.y;
                }

                if (point.y > max)
                {
                    max = point.y;
                }
            }
            
            return new TrackInfo
            {
                start = min,
                end = max,
                spriteShapeController = spriteShapeController,
            };
        }
        
        public Vector2 GetTrackPosition(int trackIndex, float y, out bool shouldSwitchTrack)
        {
            InitTrackInfo();
            
            var trackInfo = trackInfos[trackIndex];
            var pointCount = trackInfo.spriteShapeController.spline.GetPointCount();
            Vector3 lastPoint = Vector3.zero;
            float maxY = float.MinValue;
            for (var i = 0; i < pointCount; i++)
            {
                var point = trackInfo.spriteShapeController.spline.GetPosition(i);
                point += trackInfo.spriteShapeController.transform.position;
                if (point.y > maxY)
                {
                    maxY = point.y;
                    lastPoint = point;
                }
            }
            // var lastPoint = trackInfo.spriteShapeController.spline.GetPosition(pointCount - 1);
            if (y >= maxY)
            {
                shouldSwitchTrack = true;
                return lastPoint;
            }
            
            for (var i = 0; i < pointCount - 1; i++)
            {
                var point = trackInfo.spriteShapeController.spline.GetPosition(i);
                point += trackInfo.spriteShapeController.transform.position;
                var nextPoint = trackInfo.spriteShapeController.spline.GetPosition(i + 1);
                nextPoint += trackInfo.spriteShapeController.transform.position;
                if (y >= point.y && y <= nextPoint.y)
                {
                    shouldSwitchTrack = false;
                    var t = (y - point.y) / (nextPoint.y - point.y);
                    return Vector2.Lerp(point, nextPoint, t);
                }
            }

            {
                Vector3 firstPoint = Vector3.zero;
                int firstIndex = 0;
                float minY = float.MaxValue;
                for (var i = 0; i < pointCount; i++)
                {
                    var point = trackInfo.spriteShapeController.spline.GetPosition(i);
                    point += trackInfo.spriteShapeController.transform.position;
                    if (point.y < minY)
                    {
                        minY = point.y;
                        firstPoint = point;
                        firstIndex = i;
                    }
                }

                if (firstPoint.y - y < 1f)
                {
                    var nextPoint = trackInfo.spriteShapeController.spline.GetPosition(firstIndex + 1);
                    nextPoint += trackInfo.spriteShapeController.transform.position;
                    
                    shouldSwitchTrack = false;
                    var t = (y - firstPoint.y) / (nextPoint.y - firstPoint.y);
                    return Vector2.Lerp(firstPoint, nextPoint, t);
                }
            }

            Debug.LogWarning($"Track position not found for track index {trackIndex} and y {y}");
            shouldSwitchTrack = false;
            return Vector2.zero;
        }

        public bool CheckIfCanSwitchTrack(int trackIndex, float y)
        {
            var trackInfo = trackInfos[trackIndex];
            return y < trackInfo.end && y > trackInfo.start;
        }

        public bool IsCloseToEnd(int trackIndex, float y)
        {
            var trackInfo = trackInfos[trackIndex];
            return y > trackInfo.end - 2f;
        }

        public int GetExitIDFromTrackIndex(int trackIndex)
        {
            InitTrackInfo();
            
            if (exit0.Contains(trackIndex))
            {
                return 0;
            }
            
            if (exit1.Contains(trackIndex))
            {
                return 1;
            }
            
            if (exit2.Contains(trackIndex))
            {
                return 2;
            }
            
            Debug.LogWarning($"Exit ID not found for track index {trackIndex}");
            return -1;
        }

        public int GetTrackIndexFromEnterID(int enterID)
        {
            InitTrackInfo();
            
            if (enterID == 0)
            {
                return enter0[0];
            }
            
            if (enterID == 1)
            {
                return enter1[0];
            }
            
            if (enterID == 2)
            {
                return enter2[0];
            }
            
            Debug.LogWarning($"Track index not found for enter ID {enterID}");
            return -1;
        }
        
        public int GetTrackIndexFromGameObject(GameObject gameObject)
        {
            InitTrackInfo();
            
            for (var i = 0; i < trackInfos.Count; i++)
            {
                if (trackInfos[i].spriteShapeController.gameObject == gameObject)
                {
                    // TODO may not right
                    return i;
                }
            }

            Debug.LogWarning($"Track index not found for game object {gameObject.name}");
            return -1;
        }

        // /// <inheritdoc />
        // public override LevelObjectData Serialize()
        // {
        //     return new BoosterData();
        // }
        //
        // /// <inheritdoc />
        // public override void Deserialize(string data)
        // {
        //     
        // }
    }
}