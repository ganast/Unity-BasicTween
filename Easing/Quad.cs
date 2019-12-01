/// <summary>
/// TODO: Credit and reference R. Penner's work.
/// TODO: Add license and copyright.
/// </summary>
namespace com.ganast.UnityEngine.Tween.Easing {

    public static class Quad {

        public static float EaseIn(float t, float b, float c, float d) {
            return c * (t /= d) * t + b;
        }

        public static float EaseOut(float t, float b, float c, float d) {
            return -c * (t /= d) * (t - 2) + b;
        }

        public static float EaseInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) return c / 2 * t * t + b;
            return -c / 2 * ((--t) * (t - 2) - 1) + b;
        }
    }
}