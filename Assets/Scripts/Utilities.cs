using UnityEngine;
using System.Collections;

namespace Snake3D {

    public class Utilities : MonoBehaviour {

        public static T FindNearestParentWithComponent<T>(Transform trans) where T : Component {
            for (; trans != null; trans = trans.parent) {
                T component = trans.GetComponent<T>();
                if (component != null)
                    return component;
            }

            return null;
        }


    }

} // namespace Snake3D
