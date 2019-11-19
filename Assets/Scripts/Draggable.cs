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

            var x = Mathf.Clamp(position.x,
                roomLeft + collider.bounds.extents.x,
                roomRight - collider.bounds.extents.x);
            var y = Mathf.Clamp(position.y,
                roomBottom + collider.bounds.extents.y,
                roomTop - collider.bounds.extents.y);
            var z = Mathf.Clamp(position.z,
                roomBack + collider.bounds.extents.z,
                roomForward - collider.bounds.extents.z);

            var stickToLeftWall = false;
            var stickToRightWall = false;
            var stickToSide = false;
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                stickToLeftWall = x < z;
                stickToRightWall = x >= z;
                if (stickToLeftWall)
                {
                    x = roomLeft + collider.bounds.extents.x;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.x);
                }
                else
                {
                    z = roomBack + collider.bounds.extents.z;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.x);
                }

                stickToFloor = Mathf.Abs(y - (roomCollider.bounds.min.y + collider.bounds.extents.y)) < 0.2;
                if (stickToFloor)
                    y = roomCollider.bounds.min.y + collider.bounds.extents.y;
            }

            foreach (var draggable in room.Draggables)
            {
                if (draggable == this)
                    continue;

                var draggableCollider = draggable.GetComponent<Collider>();

                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    if (y + collider.bounds.extents.y > draggableCollider.bounds.min.y &&
                        y - collider.bounds.extents.y < draggableCollider.bounds.max.y)
                    {
                        if (stickToRightWall)
                        {
                            if (stickToSide = Mathf.Abs(x + collider.bounds.extents.x - draggableCollider.bounds.min.x) < 0.2)
                                x = draggableCollider.bounds.min.x - collider.bounds.extents.x;
                            else if (stickToSide = Mathf.Abs(x - collider.bounds.extents.x - draggableCollider.bounds.max.x) < 0.2)
                                x = draggableCollider.bounds.max.x + collider.bounds.extents.x;
                        }
                        else if (stickToLeftWall)
                        {
                            if (stickToSide = Mathf.Abs(z + collider.bounds.extents.z - draggableCollider.bounds.min.z) < 0.2)
                                z = draggableCollider.bounds.min.z - collider.bounds.extents.z;
                            else if (stickToSide = Mathf.Abs(z - collider.bounds.extents.z - draggableCollider.bounds.max.z) < 0.2)
                                z = draggableCollider.bounds.max.z + collider.bounds.extents.z;
                        }
                    }
                }

                if (stickToSide)
                    if (draggable.stickToFloor || Mathf.Abs(y - collider.bounds.extents.y - draggableCollider.bounds.min.y) < 0.2)
                        y = collider.bounds.extents.y + draggableCollider.bounds.min.y;
                    else if (Mathf.Abs(y + collider.bounds.extents.y - draggableCollider.bounds.max.y) < 0.2)
                        y = -collider.bounds.extents.y + draggableCollider.bounds.max.y;
            }

            transform.position = new Vector3(x, y, z);
        }
    }
}
