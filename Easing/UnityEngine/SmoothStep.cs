using System;
using UnityEngine;

namespace com.ganast.Tween.Easing.UnityEngine {

    public static class SmoothStep {

        public static float EaseIn(float t, float b, float c, float d) {
            throw new NotImplementedException();
        }

        public static float EaseOut(float t, float b, float c, float d) {
            throw new NotImplementedException();
        }

        public static float EaseInOut(float t, float b, float c, float d) {
            return Mathf.SmoothStep(b, b + c, Mathf.Clamp01(t / d));
        }
    }
}