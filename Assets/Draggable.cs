using UnityEngine;

namespace Ceramic3d.Test
{
    public class Draggable : MonoBehaviour
    {
        private Renderer roomRenderer;
        private Collider collider;
        private Draggable[] draggables;

        public bool StickToFloor { get; private set; }
        public bool StickToSide { get; private set; }

        private void Awake()
        {
            roomRenderer = FindObjectOfType<Room>().GetComponent<Renderer>();
            collider = GetComponent<Collider>();
        }

        public void Drag(Vector3 position)
        {
            StickToFloor = false;
            StickToSide = false;
            draggables = FindObjectsOfType<Draggable>();

            var x = Mathf.Clamp(position.x,
                roomRenderer.bounds.min.x + collider.bounds.extents.x,
                roomRenderer.bounds.max.x - collider.bounds.extents.x);
            var y = Mathf.Clamp(position.y,
                roomRenderer.bounds.min.y + collider.bounds.extents.y,
                roomRenderer.bounds.max.y - collider.bounds.extents.y);
            var z = Mathf.Clamp(position.z,
                roomRenderer.bounds.min.z + collider.bounds.extents.z,
                roomRenderer.bounds.max.z - collider.bounds.extents.z);

            if (!Input.GetKey(KeyCode.LeftControl))
            {
                if (x < z)
                {
                    x = roomRenderer.bounds.min.x + collider.bounds.extents.x;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.x);
                }
                else
                {
                    z = roomRenderer.bounds.min.z + collider.bounds.extents.z;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.x);
                }

                if (StickToFloor = Mathf.Abs(y - (roomRenderer.bounds.min.y + collider.bounds.extents.y)) < 0.2)
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

                        if (StickToSide = Mathf.Abs(x + collider.bounds.extents.x - draggableCollider.bounds.min.x) < 0.2)
                            x = draggableCollider.bounds.min.x - collider.bounds.extents.x;
                        else if (StickToSide = Mathf.Abs(x - collider.bounds.extents.x - draggableCollider.bounds.max.x) < 0.2)
                            x = draggableCollider.bounds.max.x + collider.bounds.extents.x;

                        else if (StickToSide = Mathf.Abs(z + collider.bounds.extents.z - draggableCollider.bounds.min.z) < 0.2)
                            z = draggableCollider.bounds.min.z - collider.bounds.extents.z;
                        else if (StickToSide = Mathf.Abs(z - collider.bounds.extents.z - draggableCollider.bounds.max.z) < 0.2)
                            z = draggableCollider.bounds.max.z + collider.bounds.extents.z;
                    }
                }

                if (StickToSide)
                    if (draggable.StickToFloor || Mathf.Abs(y - collider.bounds.extents.y - draggableCollider.bounds.min.y) < 0.2)
                        y = collider.bounds.extents.y + draggableCollider.bounds.min.y;
                    else if (Mathf.Abs(y + collider.bounds.extents.y - draggableCollider.bounds.max.y) < 0.2)
                        y = -collider.bounds.extents.y + draggableCollider.bounds.max.y;

                if (collider.bounds.Intersects(draggableCollider.bounds) && !StickToSide)
                    y = y > draggableCollider.bounds.center.y
                        ? draggableCollider.bounds.max.y + collider.bounds.extents.y
                        : draggableCollider.bounds.min.y - collider.bounds.extents.y;
            }

            transform.position = new Vector3(x, y, z);
        }
    }
}
