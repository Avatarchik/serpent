using UnityEngine;
using System.Collections;

namespace Serpent {

    using Builder = PlanetSurfaceBuilder;

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PlanetLevel : MonoBehaviour, IInitializable {

        public Builder.Config config = new Builder.Config();

        public void Init() {
            Debug.Assert(transform.lossyScale == Vector3.one);

            Builder.Result result = Builder.Build(config);

            GetComponent<MeshFilter>().mesh = result.mesh;

            // Apply heightmap as texture
            Material material = GetComponent<Renderer>().material;
            material.SetTexture("_HeightCubemap", result.heightMap);
        }
    }

} // namespace Serpent
