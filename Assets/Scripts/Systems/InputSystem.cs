using Leopotam.EcsLite;
using UnityEngine;
using Input = Components.Input;

namespace Systems
{
    public class InputSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsPool<Input> _inputPool;
        private EcsFilter _inputFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            var entity = _world.NewEntity();

            _inputPool = _world.GetPool<Input>();
            _inputPool.Add(entity);

            _inputFilter = _world.Filter<Input>().End();
        }

        public void Run(IEcsSystems systems)
        {
            UpdateMousePressPosition();
        }

        private void UpdateMousePressPosition()
        {
            if (!UnityEngine.Input.GetMouseButtonDown(0))
            {
                return;
            }
            
            var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit))
            {
                return;
            }

            foreach (var entity in _inputFilter)
            {
                ref var inputComponent = ref _inputPool.Get(entity);
                inputComponent.Position = hit.point;
            }
        }
    }
}