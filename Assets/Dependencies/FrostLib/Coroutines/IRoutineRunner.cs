using System.Collections;
using UnityEngine;

namespace FrostLib.Coroutines
{
    public interface IRoutineRunner
    {
        Coroutine StartRoutine(IEnumerator routine);
        
        void StopRoutine(IEnumerator routine);
        void StopRoutine(Coroutine routine);
    }
}