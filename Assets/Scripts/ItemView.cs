using UnityEngine;
using UnityEngine.EventSystems;

namespace Ceramic3d.Test
{
    public class ItemView : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
    {
        private InputController inputController;
        private Room room;
        private bool selected;

        public GameObject Prefab;

        private void Awake()
        {
            inputController = FindObjectOfType<InputController>();
            room = FindObjectOfType<Room>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            selected = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!selected)
                return;

            selected = false;

            var obj = Instantiate(Prefab);
            var draggable = obj.GetComponent<Draggable>();
            inputController.selectedDraggable = draggable;
            room.Register(draggable);
        }
    }
}
