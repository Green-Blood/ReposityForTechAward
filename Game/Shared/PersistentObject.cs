using UnityEngine;

namespace Game.Shared
{
    public class PersistentObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}