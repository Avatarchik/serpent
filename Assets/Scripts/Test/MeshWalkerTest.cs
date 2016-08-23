using UnityEngine;
using System.Collections;

namespace Snake3D {

    [RequireComponent(typeof(MeshWalker))]
    public class MeshWalkerTest : MonoBehaviour {

        private MeshWalker walker;
    
	    void Start() {
            walker = GetComponent<MeshWalker>();

            MeshUtils.ApplyTransformToMesh(walker.meshFilter);

            walker.Init();
            walker.RespawnNearestToTransform();
	    }
	
	    void Update() {

	    }
    }

} // namespace Snake3D
