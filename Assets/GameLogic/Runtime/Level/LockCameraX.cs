using Unity.Cinemachine;
using UnityEngine;

namespace CoinDash.GameLogic.Runtime.Level
{
    [ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
    public class LockCameraX : CinemachineExtension
    {
        public float xPosition;
        
        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var pos = state.RawPosition;
                pos.x = xPosition;
                state.RawPosition = pos;

                // var yPos = Mathf.Abs(vcam.Follow.position.x) / state.Lens.Aspect;
                if (vcam.Follow == null) return;
                
                // state.Lens.OrthographicSize = Mathf.Clamp(vcam.Follow.position.x, 7f / state.Lens.Aspect, 10f / state.Lens.Aspect);
            }
        }
    }
}