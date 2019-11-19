using UnityEngine;

namespace Ceramic3d.Test
{
    public class OverviewCamera : MonoBehaviour
    {
        public Transform Target;
        [Range(0.001f, 1)]
        public float Smoothness;

        public float MaxVerticalRotation;
        public float MinVerticalRotation;
        public float MinRadius;
        public float MaxRadius;

        public float TargetVerticalRotation;
        public float TargetHorizontalRotation;
        public float TargetRadius;

        private float currentHorizontalRotation;
        private float currentVerticalRotation;
        private float currentRadius;
        
        protected void Start()
        {
            transform.position = Target.position + Target.forward;
            currentHorizontalRotation = 0;
            currentVerticalRotation = 0;
            currentRadius = 1;
        }

        protected virtual void Update()
        {
            TargetVerticalRotation = Mathf.Min(TargetVerticalRotation, MaxVerticalRotation);
            TargetVerticalRotation = Mathf.Max(TargetVerticalRotation, MinVerticalRotation);
            TargetRadius = Mathf.Min(TargetRadius, MaxRadius);
            TargetRadius = Mathf.Max(TargetRadius, MinRadius, 0);

            var smoothness = 1.001f - Smoothness;

            var horizontalRotationDelta = TargetHorizontalRotation - currentHorizontalRotation;
            transform.RotateAround(Target.position, Vector3.up, horizontalRotationDelta * smoothness);
            currentHorizontalRotation += horizontalRotationDelta * smoothness;

            var verticalRotationDelta = TargetVerticalRotation - currentVerticalRotation;
            transform.RotateAround(Target.position, Vector3.Cross(transform.position - Target.position, Vector3.up), verticalRotationDelta * smoothness);
            currentVerticalRotation += verticalRotationDelta * smoothness;

            var radiusDelta = TargetRadius - currentRadius;
            transform.position += (transform.position - Target.position).normalized * radiusDelta * smoothness;
            currentRadius += radiusDelta * smoothness;

            transform.LookAt(Target.position);
        }

        private void Reset()
        {
            Smoothness = 0.8f;
        }
    }
}
