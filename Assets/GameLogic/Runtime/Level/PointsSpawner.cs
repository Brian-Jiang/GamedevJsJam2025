using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using Unity.Mathematics;
using UnityEditor;
#endif

namespace CoinDash.GameLogic.Runtime.Level
{
    [RequireComponent(typeof(SplineContainer))]
    public class PointsSpawner : MonoBehaviour
    {
#if UNITY_EDITOR

        [AssetsOnly]
        public GameObject pointPrefab;
        
        public float step = 1f;
        public int pointsDensity = 1;
        public float randomRange = 0.5f;
        
        [Button]
        public void SpawnPoints()
        {
            SplineContainer splineContainer = GetComponent<SplineContainer>();
            if (splineContainer == null)
            {
                Debug.LogError("SplineContainer not found");
                return;
            }
            
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            
            foreach(var spline in splineContainer.Splines)
            {
                // float splineLength = spline.GetLength();
                // float stepT = step / splineLength;
                // for (float i = 0; i < 1f; i += stepT)
                // {
                //     var position = spline.EvaluatePosition(i);
                //     // var stage = PrefabStageUtility.GetCurrentPrefabStage();
                //     // var stageScene = stage.scene;
                //     var pointInstance = (GameObject) PrefabUtility.InstantiatePrefab(pointPrefab, transform);
                //     pointInstance.transform.position = position;
                //     // GameObject point = Instantiate(pointPrefab, position, Quaternion.identity);
                //     // point.transform.SetParent(transform);
                // }
                
                var scatterCount = Mathf.FloorToInt(spline.GetLength() * pointsDensity);
                for (int i = 0; i < scatterCount; i++)
                {
                    // Pick a random position along the spline parameter.
                    float t = Random.Range(0f, 1f);

                    // Get the spline position as a Vector2.
                    var position = spline.EvaluatePosition(t).xy;

                    // Get the tangent (direction) at that point.
                    var tangent = math.normalize(spline.EvaluateTangent(t)).xy;

                    // Compute the perpendicular direction by rotating the tangent by 90 degrees.
                    var perpendicular = new float2(-tangent.y, tangent.x);

                    // Apply a random offset along the perpendicular.
                    float offset = Random.Range(-randomRange, randomRange);
                    Vector2 scatterPosition = position + perpendicular * offset;

                    // Instantiate the object at the computed 2D position.
                    // Convert the Vector2 to Vector3 (with z=0).
                    // Instantiate(objectPrefab, new Vector3(scatterPosition.x, scatterPosition.y, 0f), Quaternion.identity, transform);
                    var pointInstance = (GameObject) PrefabUtility.InstantiatePrefab(pointPrefab, transform);
                    pointInstance.transform.localPosition = new Vector3(scatterPosition.x, scatterPosition.y, 0f);
                }
            }
        }
        
        [Button]
        public void ClearPoints()
        {
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
        
#endif
    }
}