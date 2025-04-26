using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;

namespace CoinDash.GameLogic.Runtime.Level.Tracks
{
    public abstract class TrackBase : LevelObject
    {
        public SpriteShapeController spriteShapeController;
        public int maxSplineSearchIterations = 20;
        
        private UnityEngine.Splines.Spline spline;

        private void Awake()
        {
            spline = TrackUtils.GetSpline(spriteShapeController.spline);
        }

        public Vector2 GetStartPosition()
        {
            var position = spline.EvaluatePosition(0f);
            return transform.position + new Vector3(position.x, position.y, 0f);
        }

        private Vector2 GetEndPosition()
        {
            var position = spline.EvaluatePosition(1f);
            return transform.position + new Vector3(position.x, position.y, 0f);
        }

        public bool IsCloseToEnd(float y)
        {
            return y > GetEndPosition().y - 1f;
        }
        
        public bool IsInRange(float y)
        {
            // var start = GetStartPosition().y;
            var end = GetEndPosition().y;
            return y < end;
        }
        
        public Vector2 GetSplinePosition(float y)
        {
            float yLocal = y - transform.position.y;
            float tMin = 0f;
            float tMax = 1f;

            Vector3 pos = spline.EvaluatePosition(tMin);
            for (int i = 0; i < maxSplineSearchIterations; i++)
            {
                float tMid = (tMin + tMax) / 2f;
                pos = spline.EvaluatePosition(tMid);
                float yMid = pos.y;

                if (Mathf.Abs(yMid - yLocal) < 0.01f)
                {
                    return pos + transform.position;
                }

                if (yMid < yLocal)
                    tMin = tMid;
                else
                    tMax = tMid;
            }

            return pos + transform.position;
        }
    }
}