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

using System; 

namespace com.ganast.Tween {

    public class BasicTween {

        /// <summary>
        /// A delegate for easing functions based on R. Penner's format (TODO: add ref).
        /// </summary>
        /// <param name="t">Time (TODO: elaborate)</param>
        /// <param name="b">Begin (TODO: elaborate) </param>
        /// <param name="c">Change (TODO: elaborate) </param>
        /// <param name="d">Duration (TODO: elaborate) </param>
        /// <returns></returns>
        public delegate float EasingFunction(float t, float b, float c, float d);

        public enum RateLogic {
            SPEED,
            DURATION
        }

        public const float UNLIMITED = float.NaN;

        private float v, v0, v1, t, r, d, vmin, vmax;

        private EasingFunction easingFunction;

        private RateLogic rateLogic;

        private object _lock = new object();

        public BasicTween(float v, float vmin, float vmax, EasingFunction easingFunction, RateLogic rateLogic, float r) {
            this.easingFunction = easingFunction;
            this.rateLogic = rateLogic;
            this.r = r;
            this.vmin = vmin;
            this.vmax = vmax;
            SetImmediate(v, false);
        }

        public BasicTween(float v, EasingFunction easingFunction, RateLogic rateLogic, float r) : this(v, UNLIMITED, UNLIMITED, easingFunction, rateLogic, r) {
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

        public void Update(float dt) {

            // Debug.LogFormat("v={0}, v0={1}, v1={2}, dt={3}, t={4}", v, v0, v1, Time.time - t, t);

            lock (_lock) {

                if (!Equal(v, v1)) {

                    t += dt;

                    // TODO: get value from easing function...
                    v = easingFunction(t, v0, v1 - v0, d);
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
            v0 = v;
            v1 = v;
            t = 0.0f;
        }

        private void SetInterpolation(float target, bool relative = false) {
            v0 = v;
            t = 0.0f;
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
                    d = Math.Abs(v1 - v) / r;
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
            return Math.Abs(a - b) < 0.0001f;
        }
    }
}