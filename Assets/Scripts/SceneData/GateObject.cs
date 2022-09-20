using UnityEngine;

namespace SceneData
{
    public class GateObject : MonoBehaviour
    {
        [SerializeField] private Transform _button;
        [SerializeField] private Transform _gate;

        public Transform Button => _button;
        public Transform Gate => _gate;
    }
}
