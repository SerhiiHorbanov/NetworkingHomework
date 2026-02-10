using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewVisor", menuName = "ScriptableObjects/Visor")]
    public class VisorSO : ScriptableObject
    {
        public GameObject _Prefab;
        public Sprite _Icon;
    }
}
