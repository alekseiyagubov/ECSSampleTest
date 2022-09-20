using Components;
using Components.Core;
using Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class GateTransformUpdateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _gatesFilter;
        private EcsPool<Gate> _gatesPool;
        private EcsPool<GateTransform> _gatesTransformPool;
        private Vector3 _openedGateOffset;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _gatesFilter = _world.Filter<Gate>().End();
            _gatesPool = _world.GetPool<Gate>();
            
            _gatesTransformPool = _world.GetPool<GateTransform>();

            var sharedData = systems.GetShared<SharedData>();
            _openedGateOffset = sharedData.GameConfiguration.OpenedGateOffset;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var gateEntity in _gatesFilter)
            {
                ref var gateComponent = ref _gatesPool.Get(gateEntity);
                ref var gateTransformComponent = ref _gatesTransformPool.Get(gateEntity);

                var closedPosition = gateTransformComponent.ClosedPosition;
                var openedPosition = closedPosition + _openedGateOffset;
                var openingProgress = gateComponent.OpeningProgress;
                gateTransformComponent.DoorTransform.position = Vector3.Lerp(closedPosition, openedPosition, openingProgress);
            }
        }
    }
}