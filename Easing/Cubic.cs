/// <summary>
/// TODO: Credit and reference R. Penner's work.
/// TODO: Add license and copyright.
/// </summary>
namespace com.ganast.Tween.Easing {

    public static class Cubic {

        public static float EaseIn(float t, float b, float c, float d) {
            return c * (t /= d) * t * t + b;
        }

        public static float EaseOut(float t, float b, float c, float d) {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        }

        public static float EaseInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t + 2) + b;
        }
    }
}