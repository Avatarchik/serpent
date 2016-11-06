/*using System;
using UnityEngine;
using NUnit.Framework;

namespace Serpent {
    [TestFixture]
    public class ContinuousPathTest {

        private const float kInterval = 3;

        private ContinuousPath path;

        [SetUp]
        public void Init() {
            path = new ContinuousPath(kInterval);
        }

        [Test]
        public void InitialState() {
            Assert.AreEqual(kInterval, path.interval);
            Assert.Throws<InvalidOperationException>(() => {
                path.Shrink(0);
            });
            Assert.Throws<InvalidOperationException>(() => {
                path.GetValueAt(0, SnakeSpace.FromHead);
            });
        }

        [Test]
        public void GrowAndShrink() {
            // Initial point
            var vt = MakeVT(0);
            path.Grow(vt);
            Assert.AreEqual(path.Length, 0);
            Assert.AreEqual(path.GetValueAt(0, SnakeSpace.FromHead), vt);
            Assert.AreEqual(2, path.intPath.Size);

            // Intermediate point
            var vt2 = MakeVT(kInterval * 0.3333333333f);
            Assert.AreEqual(kInterval * 0.3333333333f, path.Length, 0.00001f);

            // Grow argument must be not further than "interval" from last integer point.
            // This forces user to explicitly tell ContinuousPath, where to insert
            // integer points (for that, second Grow() argument should be used).
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var vt3 = MakeVT(kInterval * 1.5f);
                path.Grow(vt3);
            });

            // isInteger is true, but bad value
            var offsets = new float[] {
                +path.distanceTolerance * 2,
                -path.distanceTolerance * 2
            };
            foreach (float offset in offsets) {
                var vt4 = MakeVT(kInterval + offset);
                path.Grow(vt4, true);
                Assert.Throws<ArgumentOutOfRangeException>(() => {
                    path.Grow(vt4, true);
                });
                Assert.AreEqual(2, path.intPath.Size);
            }

            // isInteger is true and value is ok
            var vt5 = MakeVT(kInterval);
            path.Grow(vt5, true);
            Assert.AreEqual(3, path.intPath.Size);
            Assert.AreEqual(kInterval, path.Length, 0.00001f);

            // TODO: Shrink(), Clear(), GetValue()
        }

        private ValueTransform MakeVT(float offset) {
            return new ValueTransform(new Vector3(offset, 0, 0),
                Quaternion.identity);
        }
    }
} // namespace Serpent
*/