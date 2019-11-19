using UnityEngine;

namespace Ceramic3d.Test
{
    public class MouseFollower : MonoBehaviour
    {
        private void Update()
        {
            transform.position = Input.mousePosition;
        }
    }
}
