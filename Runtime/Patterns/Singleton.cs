using UnityEngine;

namespace UnityUtilities.Runtime
{
    public class Singleton<T> : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static Singleton<T> Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning(
                    $"there is already a singleton instance in {this.gameObject.name} other than {Instance.gameObject.name}",
                    this.gameObject);
                Destroy(this);
            }
        }
    }
}