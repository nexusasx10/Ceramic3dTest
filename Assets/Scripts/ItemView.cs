using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ceramic3d.Test
{
    public class ItemView : MonoBehaviour, IPointerDownHandler
    {
        private InputController inputController;
        private Room room;
        private bool selected;
        private GameObject follower;

        public GameObject Prefab;
        public GameObject FollowerPrefab;

        private void Awake()
        {
            inputController = FindObjectOfType<InputController>();
            room = FindObjectOfType<Room>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            selected = true;
            follower = Instantiate(FollowerPrefab);
            follower.GetComponentInChildren<Image>().sprite = transform.Find("Image").GetComponentInChildren<Image>().sprite;
        }

        private void Update()
        {
            if (!selected)
                return;

            var pointerEventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            foreach (var result in raycastResults)
                if (result.gameObject.layer == LayerMask.NameToLayer("Room"))
                {
                    selected = false;
                    DestroyImmediate(follower.gameObject);

                    var obj = Instantiate(Prefab);
                    var draggable = obj.GetComponent<Draggable>();
                    draggable.Drag(Input.mousePosition);
                    inputController.selectedDraggable = draggable;
                    room.Register(draggable);
                    break;
                }
        }
    }
}
