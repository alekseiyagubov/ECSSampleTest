using Components;
using Components.Core;
using Data;
using Leopotam.EcsLite;

namespace Systems
{
    public class GatesInitializeSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            
            foreach (var sharedDataGate in sharedData.Gates)
            {
                var entity = world.NewEntity();
                
                var gatesPool = world.GetPool<Gate>();
                ref var gateComponent = ref gatesPool.Add(entity);
                gateComponent.ButtonPosition = sharedDataGate.Button.position;

                var gatesTransformPool = world.GetPool<GateTransform>();
                ref var gateTransformComponent = ref gatesTransformPool.Add(entity);
                
                gateTransformComponent.DoorTransform = sharedDataGate.Gate;
                gateTransformComponent.ClosedPosition = sharedDataGate.Gate.position;
            }
        }
    }
}