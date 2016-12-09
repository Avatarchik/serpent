using UnityEngine;
using Zenject;

namespace Serpent {

    public class GlobalInstaller : MonoInstaller {

        public override void InstallBindings() {
            Container.Bind<NetworkManager>().AsSingle();
        }
    }

} // namespace Serpent
