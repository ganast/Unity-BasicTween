/// An implementation of linear tweening functions based on Unity Engine's
/// Mathf.Lerp functionality. Formatted as easing-in, -out and -in/out functions
/// as a convenience (i.e., for easy in-place substitution in code and drop-in
/// at runtime).
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

using UnityEngine;

namespace com.ganast.Tween.Easing.UnityEngine {

    /// <summary>
    /// An implementation of linear tweening functions based on Unity Engine's
    /// <see cref="Mathf.Lerp(float, float, float)"/>.
    /// </summary>
    public static class Lerp {

        /// <summary>
        /// Linear easing-in. Identical to <see cref="LinearTween(float, float,
        /// float, float)"/>.
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Beginning value</param>
        /// <param name="c">Value change</param>
        /// <param name="d">Duration</param>
        /// <returns>Value at current time</returns>
        public static float EaseIn(float t, float b, float c, float d) {
            return LinearTween(t, b, c, d);
        }

        /// <summary>
        /// Linear easing-out. Identical to <see cref="LinearTween(float, float,
        /// float, float)"/>.
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Beginning value</param>
        /// <param name="c">Value change</param>
        /// <param name="d">Duration</param>
        /// <returns>Value at current time</returns>
        public static float EaseOut(float t, float b, float c, float d) {
            return LinearTween(t, b, c, d);
        }

        /// <summary>
        /// Linear easing-in/out. Identical to <see cref="LinearTween(float,
        /// float, float, float)"/>.
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Beginning value</param>
        /// <param name="c">Value change</param>
        /// <param name="d">Duration</param>
        /// <returns>Value at current time</returns>
        public static float EaseInOut(float t, float b, float c, float d) {
            return LinearTween(t, b, c, d);
        }

        /// <summary>
        /// Simple linear tweening using Unity Engine's <see cref="Mathf.Lerp(
        /// float, float, float)"/>.
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Beginning value</param>
        /// <param name="c">Value change</param>
        /// <param name="d">Duration</param>
        /// <returns>Value at current time</returns>
        public static float LinearTween(float t, float b, float c, float d) {
            return Mathf.Lerp(b, b + c, Mathf.Clamp01(t / d));
        }
    }
}