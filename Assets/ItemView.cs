using UnityEngine;
using UnityEngine.EventSystems;

namespace Ceramic3d.Test
{
    public class ItemView : MonoBehaviour, IPointerDownHandler
    {
        public GameObject Prefab;

        public void OnPointerDown(PointerEventData eventData)
        {
            var obj = Instantiate(Prefab);
            FindObjectOfType<InputController>().selectedDraggable = obj.GetComponent<Draggable>();
        }
    }
}
