using System;
using System.Threading;

namespace FrostLib.Tasks
{
    public class CancellationLink : IDisposable
    {
        public CancellationToken Token { get; }

        private CancellationTokenSource _cts;

        public CancellationLink(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource();

            Token = CancellationTokenSource
                .CreateLinkedTokenSource(
                    cancellationToken,
                    _cts.Token)
                .Token;
        }

        public void CancelAndDispose()
        {
            if (_cts == null || _cts.IsCancellationRequested)
                return;

            _cts.Cancel();
            Dispose();
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _cts = null;
        }
    }
}