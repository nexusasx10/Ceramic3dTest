using UnityEngine;

namespace Ceramic3d.Test
{
    public class Draggable : MonoBehaviour
    {
        private Room room;
        private Collider roomCollider;
        private Collider collider;
        private bool stickToFloor;

        private void Awake()
        {
            room = FindObjectOfType<Room>();
            roomCollider = room.GetComponent<Collider>();
            collider = GetComponent<Collider>();
            gameObject.layer = LayerMask.NameToLayer("Draggable");
        }

        public void Drag(Vector3 pointerPosition)
        {
            var ray = Camera.main.ScreenPointToRay(pointerPosition);
            var plane = new Plane(Camera.main.transform.forward, transform.position);
            if (!plane.Raycast(ray, out var enter))
                return;

            var position = ray.GetPoint(enter);

            var roomLeft = roomCollider.bounds.min.x;
            var roomRight = roomCollider.bounds.max.x;
            var roomBottom = roomCollider.bounds.min.y;
            var roomTop = roomCollider.bounds.max.y;
            var roomBack = roomCollider.bounds.min.z;
            var roomForward = roomCollider.bounds.max.z;

            var halfWidth = collider.bounds.extents.x;
            var halfHeight = collider.bounds.extents.y;
            var halfLength = collider.bounds.extents.z;

            var x = Mathf.Clamp(position.x,
                roomLeft + halfWidth,
                roomRight - halfWidth);
            var y = Mathf.Clamp(position.y,
                roomBottom + halfHeight,
                roomTop - halfHeight);
            var z = Mathf.Clamp(position.z,
                roomBack + halfLength,
                roomForward - halfLength);

            var stickToLeftWall = false;
            var stickToRightWall = false;
            var stickToSide = false;
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                stickToLeftWall = x < z;
                stickToRightWall = x >= z;
                if (stickToLeftWall)
                {
                    x = roomLeft + halfWidth;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.x);
                }
                else
                {
                    z = roomBack + halfLength;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.x);
                }

                stickToFloor = Mathf.Abs(y - (roomCollider.bounds.min.y + halfHeight)) < 0.2;
                if (stickToFloor)
                    y = roomCollider.bounds.min.y + halfHeight;
            }

            foreach (var draggable in room.Draggables)
            {
                if (draggable == this)
                    continue;

                var draggableCollider = draggable.GetComponent<Collider>();

                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    if (y + halfHeight > draggableCollider.bounds.min.y &&
                        y - halfHeight < draggableCollider.bounds.max.y)
                    {
                        if (stickToRightWall)
                        {
                            if (stickToSide = Mathf.Abs(x + halfWidth - draggableCollider.bounds.min.x) < 0.2)
                                x = draggableCollider.bounds.min.x - halfWidth;
                            else if (stickToSide = Mathf.Abs(x - halfWidth - draggableCollider.bounds.max.x) < 0.2)
                                x = draggableCollider.bounds.max.x + halfWidth;
                        }
                        else if (stickToLeftWall)
                        {
                            if (stickToSide = Mathf.Abs(z + halfLength - draggableCollider.bounds.min.z) < 0.2)
                                z = draggableCollider.bounds.min.z - halfLength;
                            else if (stickToSide = Mathf.Abs(z - halfLength - draggableCollider.bounds.max.z) < 0.2)
                                z = draggableCollider.bounds.max.z + halfLength;
                        }
                    }
                }

                if (stickToSide)
                    if (draggable.stickToFloor || Mathf.Abs(y - halfHeight - draggableCollider.bounds.min.y) < 0.2)
                        y = halfHeight + draggableCollider.bounds.min.y;
                    else if (Mathf.Abs(y + halfHeight - draggableCollider.bounds.max.y) < 0.2)
                        y = -halfHeight + draggableCollider.bounds.max.y;
            }

            transform.position = new Vector3(x, y, z);
        }
    }
}
