using UnityEngine;
using System.Collections;

namespace Serpent {

    public interface IInitializable {
        void Init();
    }

    /**
     * Allows to init objects in particular order.
     * You must either leave "startIndependently" to true, to let it start automatically
     * or start it manually from another MyInitHelper, chaining them together.
     */
    public class InitHelper : MonoBehaviour, IInitializable {

        public bool startIndependently = true;
        public MonoBehaviour[] objects;

        private bool initialized = false;

        void Start() {
            if (startIndependently)
                Init();
        }

        public void Init() {
            Debug.Assert(initialized == false);
            initialized = true;

            foreach (MonoBehaviour comp in objects) {
                var obj = comp as IInitializable;
                Debug.Assert(obj != null);
                if (comp.enabled)
                    obj.Init();
            }
        }
    }

} // namespace Serpent
