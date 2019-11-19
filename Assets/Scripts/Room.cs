using System.Collections.Generic;
using UnityEngine;

namespace Ceramic3d.Test
{
    public class Room : MonoBehaviour
    {
        private List<Draggable> draggables;

        public Vector3 Size;
        public Material FrontMaterial;
        public Material BackMaterial;
        public IEnumerable<Draggable> Draggables => draggables;

        public void Register(Draggable draggable)
        {
            draggables.Add(draggable);
        }

        private void Awake()
        {
            draggables = new List<Draggable>();
        }

        private void Start()
        {
            var collider = GetComponent<BoxCollider>();
            collider.size = Size;

            var floorFrontTransform = GameObject.CreatePrimitive(PrimitiveType.Plane).transform;
            var leftWallFrontTransform = GameObject.CreatePrimitive(PrimitiveType.Plane).transform;
            var rightWallFrontTransform = GameObject.CreatePrimitive(PrimitiveType.Plane).transform;
            var floorBackTransform = GameObject.CreatePrimitive(PrimitiveType.Plane).transform;
            var leftWallBackTransform = GameObject.CreatePrimitive(PrimitiveType.Plane).transform;
            var rightWallBackTransform = GameObject.CreatePrimitive(PrimitiveType.Plane).transform;

            floorFrontTransform.parent = transform;
            leftWallFrontTransform.parent = transform;
            rightWallFrontTransform.parent = transform;
            floorBackTransform.parent = transform;
            leftWallBackTransform.parent = transform;
            rightWallBackTransform.parent = transform;

            floorFrontTransform.localScale = new Vector3(Size.x / 10, 1, Size.z / 10);
            leftWallFrontTransform.localScale = new Vector3(Size.y / 10, 1, Size.z / 10);
            rightWallFrontTransform.localScale = new Vector3(Size.x / 10, 1, Size.y / 10);
            floorBackTransform.localScale = new Vector3(Size.x / 10, 1, Size.z / 10);
            leftWallBackTransform.localScale = new Vector3(Size.y / 10, 1, Size.z / 10);
            rightWallBackTransform.localScale = new Vector3(Size.x / 10, 1, Size.y / 10);

            leftWallFrontTransform.Rotate(0, 0, -90);
            rightWallFrontTransform.Rotate(90, 0, 0);
            leftWallBackTransform.Rotate(0, 0, 90);
            rightWallBackTransform.Rotate(-90, 0, 0);
            floorBackTransform.Rotate(180, 0, 0);

            floorFrontTransform.Translate(0, -Size.y / 2, 0, Space.World);
            leftWallFrontTransform.Translate(-Size.x / 2, 0, 0, Space.World);
            rightWallFrontTransform.Translate(0, 0, -Size.z / 2, Space.World);
            floorBackTransform.Translate(0, -Size.y / 2, 0, Space.World);
            leftWallBackTransform.Translate(-Size.x / 2, 0, 0, Space.World);
            rightWallBackTransform.Translate(0, 0, -Size.z / 2, Space.World);

            floorFrontTransform.GetComponent<MeshRenderer>().material = FrontMaterial;
            leftWallFrontTransform.GetComponent<MeshRenderer>().material = FrontMaterial;
            rightWallFrontTransform.GetComponent<MeshRenderer>().material = FrontMaterial;
            floorBackTransform.GetComponent<MeshRenderer>().material = BackMaterial;
            leftWallBackTransform.GetComponent<MeshRenderer>().material = BackMaterial;
            rightWallBackTransform.GetComponent<MeshRenderer>().material = BackMaterial;

            floorFrontTransform.gameObject.layer = LayerMask.NameToLayer("Room");
            leftWallFrontTransform.gameObject.layer = LayerMask.NameToLayer("Room");
            rightWallFrontTransform.gameObject.layer = LayerMask.NameToLayer("Room");
        }
    }
}
