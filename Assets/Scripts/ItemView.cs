using UnityEngine;
using UnityEngine.EventSystems;

namespace Ceramic3d.Test
{
    public class ItemView : MonoBehaviour, IPointerDownHandler
    {
        private InputController inputController;
        private Room room;

        public GameObject Prefab;

        private void Awake()
        {
            inputController = FindObjectOfType<InputController>();
            room = FindObjectOfType<Room>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var obj = Instantiate(Prefab);
            var draggable = obj.GetComponent<Draggable>();
            inputController.selectedDraggable = draggable;
            room.Register(draggable);
        }
    }
}
