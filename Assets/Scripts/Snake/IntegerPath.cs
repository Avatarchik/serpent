using UnityEngine;
using System.Collections;

namespace Snake3D {

    public enum SnakeSpace {
        FromHead,
        FromTail,
        FromStart
    }

    public class IntegerPath<T> : IEnumerable {
        public int stepsPassed { get; private set; }
        public readonly float interval;

        public float Mileage { get; } = 0;
        public int Size { get; } = 0;
        public float Length { get; } = 0;

        // TODO: make buffer "infinite"
        private CircularBuffer<T> buffer = new CircularBuffer<T>(512);

        public IntegerPath(float interval) {
            this.interval = interval;
        }

        public void PushToEnd(T value) {

        }

        public void PushToEnd(T[] values) {

        }

        public void PopFromStart() {

        }

        public T GetValueAt(int index, SnakeSpace space = SnakeSpace.FromTail) {
            return default(T);
        }

        public T this[int index] {
            get { return GetValueAt(index); }
        }

        public IEnumerator GetEnumerator() => buffer.GetEnumerator();
    }

} // namespace Snake3D
