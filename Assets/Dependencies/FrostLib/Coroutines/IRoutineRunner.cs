using System.Collections;
using UnityEngine;

namespace FrostLib.Coroutines
{
    public interface IRoutineRunner
    {
        Coroutine StartRoutine(IEnumerator routine, bool stopOnSceneSwitch = true);
        
        void StopRoutine(IEnumerator routine);
        void StopRoutine(Coroutine routine);
    }
}