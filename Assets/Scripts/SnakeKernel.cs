using UnityEngine;
using JetBlack.Core.Collections.Generic;

public struct Ring {
    public Vector3 position;
    public Quaternion rotation;

    public Ring(Vector3 position, Quaternion rotation) {
        this.position = position;
        this.rotation = rotation;
    }
}

public interface ISnakeKernel {
    void PushToEnd(Ring ring);
    void PopFromStart();
    ICircularBuffer<Ring> Path { get; }
}

public class SnakeKernel : MonoBehaviour, IInitializable, ISnakeKernel {
    
    public int pointsNum = 64;
    public bool showDebugInfo = false;

    public ICircularBuffer<Ring> Path { get; private set; }

#if UNITY_EDITOR
    void Update() {
        DebugDraw();
    }
#endif

    public void Init() {
        Path = new CircularBuffer<Ring>(pointsNum);
    }

    public void PushToEnd(Ring dest) {
        Path.Enqueue(dest);
    }

    public void PopFromStart() {
        Path.Dequeue();
    }

#if UNITY_EDITOR
    private void DebugDraw() {
        if (!showDebugInfo)
            return;

        int count = Path.Count;
        int capacity = Path.Capacity;

        for (int i = 0; i < count - 1; ++i) {
            float factor;
            {
                // In circular buffer space
                /*if (count - 2 == 0)
                    factor = 0;
                else
                    factor = (float)i / (count - 2);*/

                // In raw buffer space
                factor = (float)Path.RawPosition(i) / (capacity - 1);
            }

            var color = Color.Lerp(Color.green, Color.red, factor);
            Debug.DrawLine(Path[i].position, Path[i + 1].position, color);
        }
    }
#endif // UNITY_EDITOR
}
