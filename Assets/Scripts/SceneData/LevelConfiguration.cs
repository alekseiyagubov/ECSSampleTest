using UnityEngine;

namespace SceneData
{
    public class LevelConfiguration : MonoBehaviour
    {
        [SerializeField] private GateObject[] _gates;
        [SerializeField] private Transform _player;
        [SerializeField] private float _playerSpeed;
        [SerializeField] private float _gateOpeningTime;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Vector3 _openedGateOffset;

        public GateObject[] Gates => _gates;
        public Transform Player => _player;
        public float PlayerSpeed => _playerSpeed;
        public float GateOpeningTime => _gateOpeningTime;
        public Animator PlayerAnimator => _playerAnimator;
        public Vector3 OpenedGateOffset => _openedGateOffset;
    }
}