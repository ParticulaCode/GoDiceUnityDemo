using System;
using System.Threading;
using FrostLib.Scenes;

namespace FrostLib.Tasks
{
    public class CancellationTokenFactory : ICancellationTokenFactory
    {
        //Without this token UniTask.Yield and similar tasks will not stop when exiting the play mode in Editor
        //So we will link it with any other tokens as well
        private readonly CancellationToken _onAppClosingToken;

        public CancellationTokenFactory(CancellationToken onAppClosingToken) =>
            _onAppClosingToken = onAppClosingToken;

        public (CancellationToken Token, Action OnTaskFinished) GetSceneSwitchedToken()
        {
            var cts = new CancellationTokenSource();
            var disposeOnTaskFinished = new ExecuteOnSceneSwitchedOnce(() =>
            {
                if (cts.Token.CanBeCanceled)
                    cts.Cancel();

                cts.Dispose();
            });

            return (CancellationTokenSource
                    .CreateLinkedTokenSource(
                        cts.Token,
                        GetAppClosingToken())
                    .Token,
                OnTaskFinished: disposeOnTaskFinished.Dispose);
        }

        public CancellationToken GetAppClosingToken() => _onAppClosingToken;
    }
}