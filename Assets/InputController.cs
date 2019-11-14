using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CarParking
{
    public class InputController : MonoBehaviour
    {
        private bool uiBlock;

        public float MoveSensitivity;
        public float ZoomSensitivity;
        public float ZoomDelta { get; private set; }
        public Vector2 MoveDelta { get; private set; }

        private bool IsUIBlocks(Vector2 pointerPosition)
        {
            var pointerEventData = new PointerEventData(EventSystem.current) { position = pointerPosition };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            foreach (var result in raycastResults)
                if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;

            return false;
        }

        private void Update()
        {
            ZoomDelta = 0;
            MoveDelta = Vector2.zero;

            if (!IsUIBlocks(Input.mousePosition))
                ZoomDelta = -Input.GetAxis("Mouse ScrollWheel") * ZoomSensitivity;

            if (Input.GetMouseButtonDown(0) && IsUIBlocks(Input.mousePosition))
                uiBlock = true;

            if (Input.GetMouseButtonUp(0))
                uiBlock = false;

            if (uiBlock)
                return;

            if (Input.GetMouseButton(0))
                MoveDelta = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * MoveSensitivity;
        }

        private void Reset()
        {
            MoveSensitivity = 1f;
            ZoomSensitivity = 10f;
        }
    }
}
