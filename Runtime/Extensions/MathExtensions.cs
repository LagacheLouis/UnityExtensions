using UnityEngine;

namespace llagache
{
    public static class MathExtensions
    {
        public static float Remap (
            float value,
            (float, float) rangeIn,
            (float, float) rangeOut
        )
        {
            return (value - rangeIn.Item1)
                   / (rangeIn.Item2 - rangeIn.Item1)
                   * (rangeOut.Item2 - rangeOut.Item1)
                   + rangeOut.Item1;
        }
        
        public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime)
        {
            Vector3 c = current.eulerAngles;
            Vector3 t = target.eulerAngles;
            return Quaternion.Euler(
                Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
                Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
                Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
            );
        }

        public static float Step(float y, float x)
        {
            return (x >= y) ? 1 : 0;
        }

        public static float Frac(float f)
        {
            return f - Mathf.Floor(f);
        }
    }
}