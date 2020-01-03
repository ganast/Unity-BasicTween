/// An implementation of R. Penner's quadratic easing equations (see
/// http://robertpenner.com/easing and penner_easing_terms_of_use.txt).
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

namespace com.ganast.Tween.Easing.Penner {

    /// <summary>
    /// An implementation of R. Penner's quadratic easing equations.
    /// </summary>
    public static class Quad {

        /// <summary>
        /// Quadratic easing-in.
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Beginning value</param>
        /// <param name="c">Value change</param>
        /// <param name="d">Duration</param>
        /// <returns>Value at current time</returns>
        public static float EaseIn(float t, float b, float c, float d) {
            return c * (t /= d) * t + b;
        }

        /// <summary>
        /// Quadratic easing-out.
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Beginning value</param>
        /// <param name="c">Value change</param>
        /// <param name="d">Duration</param>
        /// <returns>Value at current time</returns>
        public static float EaseOut(float t, float b, float c, float d) {
            return -c * (t /= d) * (t - 2) + b;
        }

        /// <summary>
        /// Quadratic easing-in/out.
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Beginning value</param>
        /// <param name="c">Value change</param>
        /// <param name="d">Duration</param>
        /// <returns>Value at current time</returns>
        public static float EaseInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) return c / 2 * t * t + b;
            return -c / 2 * ((--t) * (t - 2) - 1) + b;
        }
    }
}