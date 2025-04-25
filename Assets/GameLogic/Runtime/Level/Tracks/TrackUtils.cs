using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;

namespace CoinDash.GameLogic.Runtime.Level.Tracks
{
    public static class TrackUtils
    {
        public static UnityEngine.Splines.Spline GetSpline(UnityEngine.U2D.Spline spline)
        {
            if (spline == null)
            {
                Debug.LogError("Spline is null");
                return null;
            }

            var pointCount = spline.GetPointCount();
            var newSpline = new UnityEngine.Splines.Spline(pointCount);
            
            for (var i = 0; i < pointCount; i++)
            {
                var position = spline.GetPosition(i);
                var shapeTangentMode = spline.GetTangentMode(i);
                var tangentMode = GetTangentMode(shapeTangentMode);
                newSpline.Add(position, tangentMode);
                var knot = newSpline[i];
                knot.Rotation = quaternion.identity;
                if (tangentMode != TangentMode.Linear)
                {
                    var leftTangent = spline.GetLeftTangent(i);
                    var rightTangent = spline.GetRightTangent(i);
                    
                    knot.TangentIn = leftTangent;
                    knot.TangentOut = rightTangent;
                }
                
                newSpline.SetKnot(i, knot, BezierTangent.In);
            }

            return newSpline;
        }
        
        private static TangentMode GetTangentMode(ShapeTangentMode shapeTangentMode)
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
    }
}