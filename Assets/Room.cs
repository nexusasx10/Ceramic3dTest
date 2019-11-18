using UnityEngine;

namespace Ceramic3d.Test
{
    public class Room : MonoBehaviour
    {
        public Vector3 Size;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = GetFrame(Size, Vector3.zero);
        }

        private Mesh GetFrame(Vector3 size, Vector3 center)
        {
            size /= 2;
            var mesh = new Mesh
            {
                vertices = new Vector3[]
                {
                    new Vector3(size.x + center.x, size.y + center.y, size.z + center.z),
                    new Vector3(size.x + center.x, size.y + center.y, -size.z + center.z),
                    new Vector3(size.x + center.x, -size.y + center.y, size.z + center.z),
                    new Vector3(size.x + center.x, -size.y + center.y, -size.z + center.z),
                    new Vector3(-size.x + center.x, size.y + center.y, size.z + center.z),
                    new Vector3(-size.x + center.x, size.y + center.y, -size.z + center.z),
                    new Vector3(-size.x + center.x, -size.y + center.y, size.z + center.z),
                    new Vector3(-size.x + center.x, -size.y + center.y, -size.z + center.z)
                },

                triangles = new int[]
                {
                    //0, 1, 2,
                    //3, 2, 1,

                    5, 4, 7,
                    6, 7, 4,
                
                    //4, 0, 6,
                    //2, 6, 0,

                    1, 5, 3,
                    7, 3, 5,

                    //1, 0, 5,
                    //4, 5, 0,

                    2, 3, 6,
                    7, 6, 3
                },

                normals = new Vector3[]
                {
                    -new Vector3(1, 1, 1),
                    -new Vector3(1, 1, -1),
                    -new Vector3(1, -1, 1),
                    -new Vector3(1, -1, -1),
                    -new Vector3(-1, 1, 1),
                    -new Vector3(-1, 1, -1),
                    -new Vector3(-1, -1, 1),
                    -new Vector3(-1, -1, -1)
                }
            };

            return mesh;
        }
    }
}
