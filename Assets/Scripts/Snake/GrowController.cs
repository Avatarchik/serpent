using UnityEngine;
using System.Collections;

namespace Snake3D {

    public interface IGrowable {
        void Grow(ValueTransform valTrans);
        void ShrinkToLength(float targetLength);
        float ComputeLength();

        /// Called after combination of Grow() and ShrinkToLength() has been run
        void ApplyChanges();
    }

    public class GrowController : MonoBehaviour, IInitializable {

        [NotNull] public Transform walker;
        [NotNull] public MonoBehaviour growable_;
        public float targetLength;

        private IGrowable growable;

        public void Init() {
            // Scale must be (1; 1; 1) because mesh normals depend on walker
            // transformation matrix and they need to be uniform
            Debug.Assert(walker.localScale == Vector3.one);

            growable = growable_ as IGrowable;
            Debug.Assert(growable != null);
        }

        void Update() {
            MaintainLength();
        }

        private void MaintainLength() {
            growable.Grow(new ValueTransform(walker));

            float currentLength = growable.ComputeLength();
            float shrinkLength = currentLength - targetLength;
            if (shrinkLength > 0)
                growable.ShrinkToLength(targetLength);

            growable.ApplyChanges();
        }
    }

} // namespace Snake3D
