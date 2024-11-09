using UnityEngine.Events;
using UnityEngine;


namespace OutcoreInternetAdventure.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [HideInInspector] public PlayerEvents events;
        IInteractor _interactor;
        Collider2D _collision;
        [SerializeField] UnityEvent _onInteractAvaliable;
        [SerializeField] UnityEvent _onInteractNonAvaliable;
        [SerializeField] UnityEvent _onInteract;

        public Collider2D Collision { get { return _collision; } }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<IInteractor>() != null)
            {
            _interactor = collision.GetComponent<IInteractor>();
            _collision = collision;
            _onInteractAvaliable?.Invoke();
            }
        }



        public void TryInteract()
        {
            if (_interactor != null)
            {
                _interactor.StartEvent();
                _onInteract?.Invoke();
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            _interactor = null;
            _onInteractNonAvaliable?.Invoke();
            _collision = null;
        }
    }
}