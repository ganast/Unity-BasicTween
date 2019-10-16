using UnityEngine;

namespace com.ganast.UnityEngine {

    public class DynamicValueManager {

        public enum InterpolationLogic {
            LERP,
            DAMP
        }

        public const float DEFAULT_DURATION = 1.0f;

        public const InterpolationLogic DEFAULT_INTERPOLATION_LOGIC = InterpolationLogic.DAMP;

        private InterpolationLogic logic;

        private float v, v0, v1, t, vmin, vmax;

        private float duration;

        private object _lock = new object();

        public DynamicValueManager(float v, InterpolationLogic logic, float duration) {
            this.logic = logic;
            SetDuration(duration);
            SetValue(v);
        }

        public DynamicValueManager(float v, float vmin, float vmax, InterpolationLogic logic, float duration) {
            this.logic = logic;
            SetDuration(duration);
            SetLimits(vmin, vmax);
            SetValue(v);
        }

        public DynamicValueManager(float v, float duration) : this(v, DEFAULT_INTERPOLATION_LOGIC, duration) {
        }

        public DynamicValueManager(float v, InterpolationLogic logic) : this(v, logic, DEFAULT_DURATION) {
        }

        public DynamicValueManager(float v) : this(v, DEFAULT_INTERPOLATION_LOGIC, DEFAULT_DURATION) {
        }

        public DynamicValueManager(float v, float vmin, float vmax, float duration) : this(v, vmin, vmax, DEFAULT_INTERPOLATION_LOGIC, duration) {
        }

        public DynamicValueManager(float v, float vmin, float vmax, InterpolationLogic logic) : this(v, vmin, vmax, logic, DEFAULT_DURATION) {
        }

        public DynamicValueManager(float v, float vmin, float vmax) : this(v, vmin, vmax, DEFAULT_INTERPOLATION_LOGIC, DEFAULT_DURATION) {
        }

        public float GetValue() {
            return v;
        }

        public float GetTime() {
            return t;
        }

        public void SetValue(float value, bool relative = false) {
            lock (_lock) {
                SetImmediate(value, relative);
            }
        }

        public void SetTarget(float target, bool relative = false) {
            lock (_lock) {
                SetInterpolation(target, relative);
            }
        }

        public void SetTarget(float target, float duration, bool relative = false) {
            lock (_lock) {
                SetInterpolation(target, relative);
                SetDuration(duration);
            }
        }

        public void SetRange(float target, float origin, bool relative = false) {
            lock (_lock) {
                SetInterpolation(target, origin, relative);
            }
        }

        public void SetRange(float target, float origin, float duration, bool relative = false) {
            lock (_lock) {
                SetInterpolation(target, origin, relative);
                SetDuration(duration);
            }
        }

        public float GetDuration() {
            return duration;
        }

        public void SetDuration(float duration) {
            this.duration = duration;
        }

        public float GetMin() {
            return vmin;
        }

        public void SetMin(float vmin) {
            this.vmin = vmin;
        }

        public float GetMax() {
            return vmax;
        }

        public void SetMax(float vmax) {
            this.vmax = vmax;
        }

        public void GetLimits(out float vmin, out float vmax) {
            vmin = this.vmin;
            vmax = this.vmax;
        }

        public void SetLimits(float vmin, float vmax) {
            SetMin(vmin);
            SetMax(vmax);
        }

        public void UnsetMin() {
            vmin = float.NaN;
        }

        public void UnsetMax() {
            vmax = float.NaN;
        }

        public void UnsetLimits() {
            UnsetMin();
            UnsetMax();
        }

        public void Update() {

            // Debug.LogFormat("v={0}, v0={1}, v1={2}, dt={3}, t={4}", v, v0, v1, Time.time - t, t);

            if (!Equal(v, v1)) {
                switch (logic) {
                    case InterpolationLogic.LERP:
                        // no need to clamp t as Lerp does that anyway...
                        t += Time.deltaTime;
                        v = Mathf.Lerp(v0, v1, t / duration);
                        break;
                    case InterpolationLogic.DAMP:
                        v = Mathf.SmoothDamp(v, v1, ref v0, duration);
                        break;
                }
            }
            else {
                SetImmediate(v1, false);
            }
        }

        private void SetImmediate(float value, bool relative = false) {
            if (relative) {
                v += value;
            }
            else {
                v = value;
            }
            v = Sanitize(v);
            switch (logic) {
                case InterpolationLogic.LERP:
                    v0 = v;
                    t = 0.0f;
                    break;
                case InterpolationLogic.DAMP:
                    v0 = 0.0f;
                    break;
            }
            v1 = v;
        }

        private void SetInterpolation(float target, bool relative = false) {
            switch (logic) {
                case InterpolationLogic.LERP:
                    v0 = v;
                    t = 0.0f;
                    break;
                case InterpolationLogic.DAMP:
                    v0 = 0.0f;
                    break;
            }
            if (relative) {
                v1 += target;
            }
            else {
                v1 = target;
            }
            v1 = Sanitize(v1);
        }

        private void SetInterpolation(float target, float origin, bool relative = false) {
            v = Sanitize(origin);
            SetInterpolation(target, relative);
        }

        public float Sanitize(float f) {
            if (!float.IsNaN(vmin) && f < vmin) {
                return vmin;
            }
            else if (!float.IsNaN(vmax) && f > vmax) {
                return vmax;
            }
            else {
                return f;
            }
        }

        public static bool Equal(float a, float b) {
            return Mathf.Abs(a - b) < 0.0001f;
        }
    }
}