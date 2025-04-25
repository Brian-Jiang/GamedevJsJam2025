using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CoinDash.GameLogic.Runtime.Level
{
    public class PointsSpawnerShape : MonoBehaviour
    {
#if UNITY_EDITOR

        [AssetsOnly]
        public GameObject pointPrefab;
        
        public float pointsDensity = 1;
        public float pointsDensitySingle = 1;

        [Button]
        public void SpawnPoints()
        {
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            
            if (TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider2D))
            {
                var radius = circleCollider2D.radius;
                var pointsCountSingle = Mathf.FloorToInt(radius * pointsDensitySingle);
                for (var j = 0; j < pointsCountSingle; j++)
                {
                    var center = circleCollider2D.offset;
                    var rInner = (float) j / pointsCountSingle * radius;
                    var circumference = 2 * Mathf.PI * rInner;
                    var pointsCount = Mathf.FloorToInt(circumference * pointsDensity);
                    
                    var angleStep = 360f / pointsCount;
                    for (int i = 0; i < pointsCount; i++)
                    {
                        var angle = i * angleStep;
                        
                        // var d = (j + 1f) / pointsCountSingle * radius;
                        var x = center.x + rInner * Mathf.Cos(angle * Mathf.Deg2Rad);
                        var y = center.y + rInner * Mathf.Sin(angle * Mathf.Deg2Rad);

                        var pointInstance = (GameObject)PrefabUtility.InstantiatePrefab(pointPrefab, transform);
                        pointInstance.transform.localPosition = new Vector3(x, y, 0f);


                        // var point = Instantiate(pointPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                        // point.name = $"Point_{i}";
                    }
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