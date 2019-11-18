using UnityEngine;

namespace Ceramic3d.Test
{
    public class Draggable : MonoBehaviour
    {
        private Renderer roomRenderer;
        private Collider collider;
        private Draggable[] draggables;
        private bool stickToFloor;

        private void Awake()
        {
            roomRenderer = FindObjectOfType<Room>().GetComponent<Renderer>();
            collider = GetComponent<Collider>();
        }

        public void Drag(Vector3 position)
        {
            draggables = FindObjectsOfType<Draggable>();

            var roomLeft = roomRenderer.bounds.min.x;
            var roomRight = roomRenderer.bounds.max.x;
            var roomBottom = roomRenderer.bounds.min.y;
            var roomTop = roomRenderer.bounds.max.y;
            var roomBack = roomRenderer.bounds.min.z;
            var roomForward = roomRenderer.bounds.max.z;

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

                stickToFloor = Mathf.Abs(y - (roomRenderer.bounds.min.y + collider.bounds.extents.y)) < 0.2;
                if (stickToFloor)
                    y = roomRenderer.bounds.min.y + collider.bounds.extents.y;
            }

            foreach (var draggable in draggables)
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

                var intersects = Mathf.Abs(x - draggableCollider.bounds.center.x) < collider.bounds.extents.x + draggableCollider.bounds.extents.x - 0.01 &&
                    Mathf.Abs(y - draggableCollider.bounds.center.y) < collider.bounds.extents.y + draggableCollider.bounds.extents.y - 0.01 &&
                    Mathf.Abs(z - draggableCollider.bounds.center.z) < collider.bounds.extents.z + draggableCollider.bounds.extents.z - 0.01;

                if (intersects)
                    y = y > draggableCollider.bounds.center.y
                        ? draggableCollider.bounds.max.y + collider.bounds.extents.y
                        : draggableCollider.bounds.min.y - collider.bounds.extents.y;
            }

            transform.position = new Vector3(x, y, z);
        }
    }
}
