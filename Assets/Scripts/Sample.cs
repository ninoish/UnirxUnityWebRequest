using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Sample : MonoBehaviour {

    public class SampleResponse
    {
        public int userId;
        public int id;
        public string title;
        public string body;

        public override string ToString()
        {
            return userId + " , " + id + " , " + title + " , " + body;
        }
    }

    private float _waitSec = 10f;
    private bool _hasDisposed = false;
    private List<IDisposable> _subscriptions = new List<IDisposable>();

    private void Start()
    {

        var headers = new Dictionary<string, string>
        {
            {"ContentType", "application/json"}
        };

        var progress = new Progress<float>(progressValue => Debug.Log("progress : " + progressValue));

        // var url = "https://jsonplaceholder.typicode.com/posts/" + i;
        var url = "";

        var getSubscribe = UnirxUnityWebRequest
            .Get<SampleResponse>(url, headers, progress)
            .Subscribe(
                res => { Debug.Log("success + " + res); },
                exception => { Debug.Log(exception.Message); },
                () => { Debug.Log("complete"); }
            );

        _subscriptions.Add(getSubscribe);

    }

    private void Update()
    {
        _waitSec -= Time.deltaTime;
        if (_waitSec > 0 || _hasDisposed) return;

        // cancel web request
        _subscriptions[0].Dispose();
    }
}
