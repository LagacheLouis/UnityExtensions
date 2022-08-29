using System.Collections.Generic;
using UnityEngine;

namespace llagache
{
    public static class RigidbodyExtensions
    {

        public static void SetVelocityWithDrag(this Rigidbody rigidbody, Vector3 velocity, float drag, (bool, bool, bool) lockAxis = default)
        {
            Vector3 velocityDelta = (velocity - rigidbody.velocity) * drag;
            if(lockAxis.Item1) velocityDelta.x = 0;
            if(lockAxis.Item2) velocityDelta.y = 0;
            if(lockAxis.Item3) velocityDelta.z = 0;
            rigidbody.AddForce(velocityDelta, ForceMode.VelocityChange);
        }
        
        public static void AddJumpForce(this Rigidbody rigidbody, float height, float gravity)
        {
            Vector3 velocity = rigidbody.velocity;
            velocity.y = Mathf.Sqrt(2 * height * gravity);
            rigidbody.velocity = velocity;
        }

    }
}