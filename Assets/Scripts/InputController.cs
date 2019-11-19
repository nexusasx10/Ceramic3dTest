using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ceramic3d.Test
{
    [RequireComponent(typeof(Camera))]
    public class InputController : MonoBehaviour
    {
        public OverviewCamera OverviewCamera;

        public Draggable selectedDraggable;
        private bool isUiBlocks;

        private bool CheckUiBlocks(Vector2 pointerPosition)
        {
            foreach (var result in GetSelected(pointerPosition))
                if (result.layer == LayerMask.NameToLayer("UI"))
                    return true;
            return false;
        }

        private IEnumerable<GameObject> GetSelected(Vector2 pointerPosition)
        {
            var pointerEventData = new PointerEventData(EventSystem.current) { position = pointerPosition };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            foreach (var raycastResult in raycastResults)
                if (raycastResult.gameObject.layer == LayerMask.NameToLayer("Draggable"))
                    yield return raycastResult.gameObject;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                foreach (var selected in GetSelected(Input.mousePosition))
                {
                    var draggable = selected.GetComponent<Draggable>();
                    if (draggable != null)
                    {
                        selectedDraggable = draggable;
                        break;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
                selectedDraggable = null;

            if (selectedDraggable != null)
                selectedDraggable.Drag(Input.mousePosition);

            if (Input.GetMouseButtonDown(1) && CheckUiBlocks(Input.mousePosition))
                isUiBlocks = true;
            if (Input.GetMouseButtonUp(1))
                isUiBlocks = false;

            if (!isUiBlocks)
            {
                OverviewCamera.TargetRadius += -Input.GetAxis("Mouse ScrollWheel");
                if (Input.GetMouseButton(1))
                {
                    OverviewCamera.TargetHorizontalRotation += Input.GetAxis("Mouse X");
                    OverviewCamera.TargetVerticalRotation += -Input.GetAxis("Mouse Y");
                }
            }
        }
    }
}
