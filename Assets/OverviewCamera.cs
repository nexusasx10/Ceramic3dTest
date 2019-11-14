using UnityEngine;

namespace CarParking
{
    public class OverviewCamera : MonoBehaviour
    {
        public Transform Target;
        [Range(0.001f, 1)]
        public float Smoothness;
        public float MaxVerticalRotation;
        public float MinVerticalRotation;
        public Vector2 InitialRotation;
        public float MinRadius;
        public float MaxRadius;

        private float horizontalRotationOffset;
        private float verticalRotationOffset;
        private float zoomOffset;

        private float currentHorizontalRotation = 0;
        private float currentVerticalRotation = 0;
        private float currentZoom = 1;

        private InputController uiInteraction;

        private void Awake()
        {
            uiInteraction = FindObjectOfType<InputController>();
        }

        protected void Start()
        {
            transform.position = Target.position + Target.forward;
            horizontalRotationOffset = InitialRotation.x;
            verticalRotationOffset = InitialRotation.y;
            zoomOffset = MaxRadius;
            currentHorizontalRotation = 0;
            currentVerticalRotation = 0;
            currentZoom = 1;
        }

        protected virtual void Update()
        {
            horizontalRotationOffset += uiInteraction.MoveDelta.x;
            verticalRotationOffset += uiInteraction.MoveDelta.y;
            zoomOffset += uiInteraction.ZoomDelta;

            if (currentVerticalRotation + verticalRotationOffset > MaxVerticalRotation)
                verticalRotationOffset -= currentVerticalRotation + verticalRotationOffset - MaxVerticalRotation;

            if (currentVerticalRotation + verticalRotationOffset < MinVerticalRotation)
                verticalRotationOffset += MinVerticalRotation - currentVerticalRotation - verticalRotationOffset;

            if (currentZoom + zoomOffset > MaxRadius)
                zoomOffset -= currentZoom + zoomOffset - MaxRadius;

            if (currentZoom + zoomOffset < MinRadius)
                zoomOffset += MinRadius - currentZoom - zoomOffset;

            if (currentZoom + zoomOffset <= 0)
                zoomOffset += -currentZoom - zoomOffset;

            transform.RotateAround(Target.position, Vector3.up, horizontalRotationOffset * (1.001f - Smoothness));
            currentHorizontalRotation += horizontalRotationOffset * (1.001f - Smoothness);
            horizontalRotationOffset -= horizontalRotationOffset * (1.001f - Smoothness);

            transform.RotateAround(Target.position, Vector3.Cross(transform.position - Target.position, Vector3.up), verticalRotationOffset * (1.001f - Smoothness));
            currentVerticalRotation += verticalRotationOffset * (1.001f - Smoothness);
            verticalRotationOffset -= verticalRotationOffset * (1.001f - Smoothness);

            transform.position += (transform.position - Target.position).normalized * zoomOffset * (1.001f - Smoothness);
            currentZoom += zoomOffset * (1.001f - Smoothness);
            zoomOffset -= zoomOffset * (1.001f - Smoothness);

            transform.LookAt(Target.position);
        }

        private void Reset()
        {
            Smoothness = 0.8f;
        }
    }
}
