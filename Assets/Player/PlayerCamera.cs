using UnityEngine;

namespace OutcoreInternetAdventure.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [HideInInspector] public PlayerEvents events;

        [SerializeField] bool moveX, moveY;
        [SerializeField] GameObject objectToFollow;
        [SerializeField] float speed = 2.0f;

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
    }
}