using UnityEngine;
using JetBlack.Core.Collections.Generic;

public class SnakeKernel : MonoBehaviour, IInitializable {
    
    public int pointsNum = 64;
    public bool showDebugInfo = false;

    public ICircularBuffer<Vector3> Path { get; private set; }

#if UNITY_EDITOR
    void Update() {
        DebugDraw();
    }
#endif

    public void Init() {
        Path = new CircularBuffer<Vector3>(pointsNum);
    }

    public void PushToEnd(Vector3 dest) {
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
            Debug.DrawLine(Path[i], Path[i + 1], color);
        }
    }
#endif // UNITY_EDITOR
}
