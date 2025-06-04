using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityUtilities.Runtime.ServiceLocator
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator global;
        private static Dictionary<Scene, ServiceLocator> sceneContainers;
        readonly ServiceManager services = new ServiceManager();

        private const string k_globalServiceLocatorName = "ServiceLocator [Global]";
        private const string k_sceneServiceLocatorName = "ServiceLocator [Scene]";

        private static List<GameObject> tmpSceneGameObjects;

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
        {
            if (global == this)
                Debug.LogWarning("ServiceLocator.ConfigureAsGlobal : Already configured as global", this);
            else if (global != null)
                Debug.LogError(
                    "ServiceLocator.ConfigureAsGlobal : Another ServiceLocator is already configured as global", this);
            else
            {
                global = this;

                if (dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
        }

        internal void ConfigureForScene()
        {
            Scene scene = gameObject.scene;

            if (sceneContainers.ContainsKey(scene))
            {
                Debug.LogError(
                    "ServiceLocator.ConfigureForScene : Another ServiceLocator is already configured for this scene",
                    this);
                return;
            }

            sceneContainers.Add(scene, this);
        }

        public static ServiceLocator Global
        {
            get
            {
                if (global != null) return global;

                // bootstrap or initialize the new instance of global as necessary
                if (FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is { } found)
                {
                    found.BootstrapOnDemand();
                    return global;
                }

                // bootstrap or initialize the new instance of global as necessary
                var container = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobalBootstrapper>().BootstrapOnDemand();
                return global;
            }
        }


        public static ServiceLocator For(MonoBehaviour mb)
        {
            return mb.GetComponentInParent<ServiceLocator>() ?? ForSceneOf(mb) ?? Global;
        }

        public static ServiceLocator ForSceneOf(MonoBehaviour mb)
        {
            Scene scene = mb.gameObject.scene;

            if (sceneContainers.TryGetValue(scene, out var container) && container != mb)
            {
                return container;
            }

            tmpSceneGameObjects.Clear();
            scene.GetRootGameObjects(tmpSceneGameObjects);

            foreach (var go in tmpSceneGameObjects.Where(go =>
                         go.GetComponent<ServiceLocatorSceneBootstrapper>() != null))
            {
                if (go.TryGetComponent(out ServiceLocatorSceneBootstrapper bootstrapper))
                {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            return global;
        }

        public ServiceLocator Register<T>(T service)
        {
            services.Register(service);
            return this;
        }

        public ServiceLocator Register(Type type, object service)
        {
            services.Register(type, service);
            return this;
        }

        public ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service)) return this;

            if (TryGetNextInHierarchy(out ServiceLocator container))
            {
                container.Get(out service);
                return this;
            }

            throw new ArgumentException($"ServiceLocator.Get : Service of type {typeof(T).FullName} not registered");
        }

        bool TryGetService<T>(out T service) where T : class => services.TryGet(out service);

        bool TryGetNextInHierarchy(out ServiceLocator container)
        {
            if (this == global)
            {
                container = null;
                return false;
            }

            container = transform.parent.OrNull()?.GetComponentInParent<ServiceLocator>() ?? ForSceneOf(this);
            return container != null;
        }

        private void OnDestroy()
        {
            if (this == global)
            {
                global = null;
            }
            else if (sceneContainers.ContainsValue(this))
            {
                sceneContainers.Remove(gameObject.scene);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        { 
            global = null;
            sceneContainers = new Dictionary<Scene, ServiceLocator>();
            tmpSceneGameObjects = new List<GameObject>();
        }
#if UNITY_EDITOR

        [MenuItem("GameObject/ServiceLocator/Add Global")]
        static void AddGlobalBootstrapper()
        {
            var go = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocatorGlobalBootstrapper));
        }

        [MenuItem("GameObject/ServiceLocator/Add Scene")]
        static void AddSceneBootstrapper()
        {
            var go = new GameObject(k_sceneServiceLocatorName, typeof(ServiceLocatorSceneBootstrapper));
        }
    }
#endif
}