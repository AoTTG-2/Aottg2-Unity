using UnityEngine;

namespace Characters
{
    public class Wagon : MonoBehaviour
    {

        public GameObject leftWheel;
        public GameObject rightWheel;
        public GameObject horse;
        public Vector2 targetVelocity = Vector2.zero;

        private Transform leftWheelTransform;
        private Transform rightWheelTransform;
        private Rigidbody rigidbody;
        private bool grounded = false;

        private void Start()
        {
            // Get the rigidbody of the wagon
            rigidbody = GetComponent<Rigidbody>();

            // Get the transforms of the wheels
            leftWheelTransform = leftWheel.transform;
            rightWheelTransform = rightWheel.transform;
        }

        private void CheckGround()
        {
            grounded = true;
        }

        private void FixedUpdate()
        {
            if (rigidbody == null)
            {
                rigidbody = GetComponent<Rigidbody>();
                return;
            }

            CheckGround();

            // Use differential drive kinematics and the rigidbodies velocity to determine the rotation of the wheels
            // inverse kinematics (v_x - (L / 2) * omega) / r, (v_x + (L / 2) * omega) / r
            float L = Vector3.Distance(leftWheelTransform.position, rightWheelTransform.position);
            float r = leftWheelTransform.localScale.y / 2;

            // Find the velocity of the wagon along the forward vector
            var localVelocity = transform.InverseTransformDirection(rigidbody.velocity);

            float v_x = localVelocity.z;

            float omega = rigidbody.angularVelocity.y;

            float leftWheelRotationSpeed = (v_x - (L / 2) * omega) / r;
            float rightWheelRotationSpeed = (v_x + (L / 2) * omega) / r;

            float leftWheelRotation = leftWheelRotationSpeed * Time.deltaTime * Mathf.Rad2Deg;
            float rightWheelRotation = rightWheelRotationSpeed * Time.deltaTime * Mathf.Rad2Deg;

            leftWheelTransform.Rotate(leftWheelRotation, 0, 0);
            rightWheelTransform.Rotate(rightWheelRotation, 0, 0);
        }
    }
}
