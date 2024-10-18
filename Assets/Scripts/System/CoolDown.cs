using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public static class CoolDown
{
    public static void Timer(float time, Action onTimerEnds, CompositeDisposable _disposable)
    {
        Observable.Timer(TimeSpan.FromSeconds(time))
            .DoOnCompleted(() => {_disposable.Clear(); onTimerEnds?.Invoke();})
            .Subscribe(_ => { })
            .AddTo(_disposable);
    }
}
