/// <summary>
/// TODO: Credit and reference R. Penner's work.
/// TODO: Add license and copyright.
/// </summary>
namespace com.ganast.UnityEngine.Tween.Easing {

    public static class UnityEngine {

        public static float Lerp(float t, float b, float c, float d) {
			return Mathf.Lerp(b, b + c, Mathf.Clamp01(t / d));
        }

        public static float SmoothStep(float t, float b, float c, float d) {
            return Mathf.SmoothStep(b, b + c, Mathf.Clamp01(t / d));
        }
    }
}