using UnityEngine;
using Cinemachine;

namespace OutcoreInternetAdventure.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [HideInInspector] public PlayerEvents events;

        [SerializeField] bool moveX, moveY;
        [SerializeField] float speed = 2.0f;
        [SerializeField] float _standardCameraSize = 5f;
        [SerializeField] float _smallCameraSize = 2.0f;
        [SerializeField] float _largeCameraSize = 10f;
        [SerializeField] CinemachineVirtualCamera _virtualCamera;
        [SerializeField] GameObject objectToFollow;

        public void Awake() => gameObject.transform.position = objectToFollow.transform.position;

        void Update()
        {
            float interpolation = speed * Time.deltaTime;

            Vector3 position = this.transform.position;
            if (moveY)
                position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y, interpolation);
            if (moveX)
                position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation);

            this.transform.position = position;
        }

        public void SetCameraSizeLarge()
        {
            SetCameraSize(_largeCameraSize);
        }
        public void SetCameraSizeStandard()
        {
            SetCameraSize(_standardCameraSize);
        }
        public void SetCameraSizeSmall()
        {
            SetCameraSize(_smallCameraSize);
        }
        public void SetCameraSize(float size)
        {
            _virtualCamera.m_Lens.OrthographicSize = size;
        }
    }
}