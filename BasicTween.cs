/**
 * BasicTween v1.0 - A very simple tool for managing values (currently only
 * floats) that need to be set directly, interpolated towards a target or
 * interpolated within a range easily and transparently.
 *
 * Copyright (C) 2019 George Anastassakis (ganast@ganast.com)
 *
 * This program is free software: you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by the
 * Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
 * more details.
 *
 * You should have received a copy of the GNU General Public License along with
 * this program.  If not, see <https://www.gnu.org/licenses/>.
 *
 * VERSION: 1.1.0, 20191128
 */
 
using UnityEngine;

namespace com.ganast.UnityEngine {

    public class BasicTween {

        public enum InterpolationLogic {
            LINEAR,
            SMOOTH,
            SPRING
        }

        public enum RateLogic {
            SPEED,
            DURATION
        }

        public const float DEFAULT_R = 1.0f;

        public const float UNLIMITED = float.NaN;

        private float v, v0, v1, t, r, d, vmin, vmax;
        
        private InterpolationLogic interpolationLogic;

        private RateLogic rateLogic;

        private object _lock = new object();

        public BasicTween(float v, float vmin, float vmax, InterpolationLogic interpolationLogic, RateLogic rateLogic, float r) {
            this.interpolationLogic = interpolationLogic;
            this.rateLogic = rateLogic;
            this.r = r;
            this.vmin = vmin;
            this.vmax = vmax;
            SetImmediate(v, false);
        }

        public BasicTween(float v, InterpolationLogic interpolationLogic, RateLogic rateLogic, float r)  : this(v, UNLIMITED, UNLIMITED, interpolationLogic, rateLogic, r) {
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

        public void SetRange(float target, float origin, bool relative = false) {
            lock (_lock) {
                SetInterpolation(target, origin, relative);
            }
        }

        public void Update() {

            // Debug.LogFormat("v={0}, v0={1}, v1={2}, dt={3}, t={4}", v, v0, v1, Time.time - t, t);

            lock (_lock) {

                if (!Equal(v, v1)) {

                    switch (interpolationLogic) {

                        case InterpolationLogic.LINEAR:

                            // no need to clamp t as Lerp does that anyway...
                            t += Time.deltaTime;

                            v = Mathf.Lerp(v0, v1, t / d);
                            break;

                        case InterpolationLogic.SMOOTH:

                            t += Time.deltaTime;

                            v = Mathf.SmoothStep(v0, v1, Mathf.Clamp01(t / d));
                            break;

                        case InterpolationLogic.SPRING:
                            v = Mathf.SmoothDamp(v, v1, ref v0, d);
                            break;
                    }
                }
                else {
                    SetImmediate(v1, false);
                }
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
            switch (interpolationLogic) {
                case InterpolationLogic.LINEAR:
                case InterpolationLogic.SMOOTH:
                    v0 = v;
                    t = 0.0f;
                    break;
                case InterpolationLogic.SPRING:
                    v0 = 0.0f;
                    break;
            }
            v1 = v;
        }

        private void SetInterpolation(float target, bool relative = false) {
            switch (interpolationLogic) {
                case InterpolationLogic.LINEAR:
                case InterpolationLogic.SMOOTH:
                    v0 = v;
                    t = 0.0f;
                    break;
                case InterpolationLogic.SPRING:
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
            switch (rateLogic) {
                case RateLogic.DURATION:
                    d = r;
                    break;
                case RateLogic.SPEED:
                    d = Mathf.Abs(v1 - v) / r;
                    break;
            }
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