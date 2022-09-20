using Core;
using SceneData;
using UnityEngine;

namespace Data
{
    public class SharedData
    {
        public GameConfiguration GameConfiguration;
        public GateObject[] Gates;
        public Transform Player;
        public ITimeUpdater TimeUpdater;
        public Animator PlayerAnimator;
    }
}