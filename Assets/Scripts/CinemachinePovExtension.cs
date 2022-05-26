using UnityEngine;
using Cinemachine;
namespace ApocalipseZ
{
    public class CinemachinePovExtension : CinemachineExtension
    {

        [SerializeField]private float horizontalSpeed = 10f;
        [SerializeField]private float verticalSpeed = 10f;
        [SerializeField]private float clampAngle = 80f;
        private InputManager PInputManager;
        public InputManager InputManager
        {
            get
            {
                if ( PInputManager == null )
                {
                    PInputManager = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ( );
                }
                return PInputManager;
            }
        }

        private Vector3 startingRotation;

        public Vector3 GetStartrotation ( )
        {
            return startingRotation;
        }

        protected override void PostPipelineStageCallback ( CinemachineVirtualCameraBase vcam , CinemachineCore.Stage stage , ref CameraState state , float deltaTime )
        {
            if ( vcam.Follow )
            {
                if ( stage == CinemachineCore.Stage.Aim )
                {
                    if ( startingRotation == null )
                    {
                        startingRotation = transform.localRotation.eulerAngles;
                    }
                    Vector2 deltaInput = InputManager.GetMouseDelta();
                    startingRotation.x += deltaInput.x * horizontalSpeed * Time.deltaTime;
                    startingRotation.y += -deltaInput.y * verticalSpeed * Time.deltaTime;
                    startingRotation.y = Mathf.Clamp ( startingRotation.y , -clampAngle , clampAngle );
                    state.RawOrientation = Quaternion.Euler ( startingRotation.y , startingRotation.x , 0 );

                }
            }
        }


    }
}
