using UnityEngine;

namespace com.ganast.Tween.Easing.UnityEngine {

    public static class Lerp {

        public static float EaseIn(float t, float b, float c, float d) {
            return Mathf.Lerp(b, b + c, Mathf.Clamp01(t / d));
        }

        public static float EaseOut(float t, float b, float c, float d) {
            return Mathf.Lerp(b, b + c, Mathf.Clamp01(t / d));
        }

        public static float EaseInOut(float t, float b, float c, float d) {
            return Mathf.Lerp(b, b + c, Mathf.Clamp01(t / d));
        }
    }
}