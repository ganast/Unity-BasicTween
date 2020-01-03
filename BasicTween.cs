/// BasicTween v1.2 - A very simple tool for managing variables (currently only
/// floats) that need to be set directly, interpolated towards a target or
/// interpolated within a range easily and transparently.
/// 
/// Copyright (c) 2020 George Anastassakis (ganast@ganast.com)
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"),
/// to deal in the Software without restriction, including without limitation
/// the rights to use, copy, modify, merge, publish, distribute, sublicense,
/// and/or sell copies of the Software, and to permit persons to whom the
/// Software is furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
/// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
/// IN THE SOFTWARE.

using System;

namespace com.ganast.Tween {

    /// <summary>
    /// A very simple tween library that manages a single variable. Relies on client-side time
    /// updates and supports a variety of easing functions.
    /// </summary>
    public class BasicTween {

        /// <summary>
        /// A delegate for tweening functions based on R. Penner's format
        /// <seealso cref="http://robertpenner.com/easing"/>.
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Beginning value</param>
        /// <param name="c">Value change</param>
        /// <param name="d">Duration</param>
        /// <returns>Value at current time</returns>
        public delegate float EasingFunction(float t, float b, float c, float d);

        /// <summary>
        /// Determines whether rate-of-change is interpreted as either interpolation speed or
        /// duration.
        /// </summary>
        public enum RateLogic {
            SPEED,
            DURATION
        }

        /// <summary>
        /// A constant to indicate lack of limit.
        /// </summary>
        public const float UNLIMITED = float.NaN;

        /// <summary>
        /// Value of the variable managed by this implementation.
        /// </summary>
        private float v;

        /// <summary>
        /// Starting value of tweening range.
        /// </summary>
        private float v0;

        /// <summary>
        /// Ending value of tweening range.
        /// </summary>
        private float v1;

        /// <summary>
        /// Interpolation time. Updated by client code when <see cref="Update(float)"/> is called.
        /// </summary>
        private float t;

        /// <summary>
        /// Rate-of-change. Represents either interpolation speed or duration, according to
        /// <see cref="rateLogic"/>.
        /// </summary>
        private float r;

        /// <summary>
        /// Interpolation duration. Calculated every time a new tweening range is set depending on
        /// rate-of-change and <see cref="rateLogic"/>.
        /// </summary>
        private float d;

        /// <summary>
        /// Minimum value for range-limited variables.
        /// </summary>
        private float vmin;

        /// <summary>
        /// Maximum value for range-limited variables.
        /// </summary>
        private float vmax;

        /// <summary>
        /// Easing function used for interpolation.
        /// </summary>
        private EasingFunction easingFunction;

        /// <summary>
        /// Determines whether rate-of-change shall be interpreted as either intepolation
        /// speed or duration.
        /// </summary>
        private RateLogic rateLogic;

        /// <summary>
        /// Public API lock.
        /// </summary>
        private object _lock = new object();

        /// <summary>
        /// Constructs a <see cref="BasicTween"/> for a range-limited variable.
        /// </summary>
        /// <param name="v">TODO</param>
        /// <param name="vmin">TODO</param>
        /// <param name="vmax">TODO</param>
        /// <param name="easingFunction">TODO</param>
        /// <param name="rateLogic">TODO</param>
        /// <param name="r">TODO</param>
        public BasicTween(float v, float vmin, float vmax, EasingFunction easingFunction, RateLogic rateLogic, float r) {
            this.easingFunction = easingFunction;
            this.rateLogic = rateLogic;
            this.r = r;
            this.vmin = vmin;
            this.vmax = vmax;
            SetImmediate(v, false);
        }

        /// <summary>
        /// Constructs a <see cref="BasicTween"/> for an non-limited variable.
        /// </summary>
        /// <param name="v">TODO</param>
        /// <param name="easingFunction">TODO</param>
        /// <param name="rateLogic">TODO</param>
        /// <param name="r">TODO</param>
        public BasicTween(float v, EasingFunction easingFunction, RateLogic rateLogic, float r) :
            this(v, UNLIMITED, UNLIMITED, easingFunction, rateLogic, r) {
        }

        /// <summary>
        /// Returns the value of the managed variable.
        /// </summary>
        /// <returns>Value of the managed variable at the time of calling.</returns>
        public float GetValue() {
            return v;
        }

        /// <summary>
        /// Returns the interpolation time.
        /// </summary>
        /// <returns>Interpolation time at the time of calling.</returns>
        public float GetTime() {
            return t;
        }

        /// <summary>
        /// Immediately sets or changes the managed variable to or by the specified value.
        /// </summary>
        /// <param name="value">TODO</param>
        /// <param name="relative">TODO</param>
        public void SetValue(float value, bool relative = false) {
            lock (_lock) {
                SetImmediate(value, relative);
            }
        }

        /// <summary>
        /// Initiates tweening from the managed variable's current value to the specified absolute
        /// or relative target value, after making sure the target value is within limits.
        /// </summary>
        /// <param name="target">TODO</param>
        /// <param name="relative">TODO</param>
        public void SetTarget(float target, bool relative = false) {
            lock (_lock) {
                SetInterpolation(target, relative);
            }
        }

        /// <summary>
        /// Immediately sets the managed variable to the specified origin value and initiates
        /// tweening to the specified absolute or relative target value, after making sure the
        /// entire tweening range is within limits.
        /// </summary>
        /// <param name="target">TODO</param>
        /// <param name="origin">TODO</param>
        /// <param name="relative">TODO</param>
        public void SetRange(float target, float origin, bool relative = false) {
            lock (_lock) {
                SetInterpolation(target, origin, relative);
            }
        }

        /// <summary>
        /// Updates the value of the managed variable for the specified amount of elapsed time.
        /// </summary>
        /// <param name="dt">TODO</param>
        public void Update(float dt) {

            // Debug.LogFormat("v={0}, v0={1}, v1={2}, dt={3}, t={4}", v, v0, v1, dt, t);
            lock (_lock) {

                if (!Equal(v, v1)) {

                    t += dt;

                    v = easingFunction(t, v0, v1 - v0, d);
                }
                else {
                    SetImmediate(v1, false);
                }
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="value">TODO</param>
        /// <param name="relative">TODO</param>
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

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="target">TODO</param>
        /// <param name="relative">TODO</param>
        private void SetInterpolation(float target, bool relative = false) {

            v0 = v;

            if (relative) {
                v1 += target;
            }
            else {
                v1 = target;
            }

            v1 = Sanitize(v1);

            t = 0.0f;

            switch (rateLogic) {

                case RateLogic.DURATION:
                    d = r;
                    break;

                case RateLogic.SPEED:
                    d = Math.Abs(v1 - v) / r;
                    break;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="target">TODO</param>
        /// <param name="origin">TODO</param>
        /// <param name="relative">TODO</param>
        private void SetInterpolation(float target, float origin, bool relative = false) {
            v = Sanitize(origin);
            SetInterpolation(target, relative);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="f">TODO</param>
        /// <returns>TODO</returns>
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

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="a">TODO</param>
        /// <param name="b">TODO</param>
        /// <returns>TODO</returns>
        public static bool Equal(float a, float b) {
            return Math.Abs(a - b) < 0.0001f;
        }
    }
}