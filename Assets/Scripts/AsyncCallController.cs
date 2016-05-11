using System;
using UnityEngine;
using System.Threading;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;

public class AsyncCallController : MonoBehaviour
{
    //The delegate that will execute the BeginInvoke method.
    private delegate string AsyncTaskDelegate();

    void Start()
    {
        CreateAsyncTask();
    }

    void CreateAsyncTask()
    {
        AsyncTaskDelegate asyncDelegate = new AsyncTaskDelegate(AsyncMethodBeingCalledOnOtherThread);
        asyncDelegate.BeginInvoke(new AsyncCallback(MethodCalledWhenAsyncMethodCompleted), null);
    }

    private string AsyncMethodBeingCalledOnOtherThread()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        UnityEngine.Debug.Log("Started calculating the heavyStuff...");
        HeavyCalculation();
        UnityEngine.Debug.Log("Done!");

        stopWatch.Stop();
        float executionDelta = stopWatch.Elapsed.Milliseconds;

        return "The async method has completed in -> " + executionDelta + " milliseconds.";
    }

    private void HeavyCalculation()
    {
        UnityEngine.Debug.Log("Doing some heavy math calculations..");

        //Just an example. Dont laugh me out LoLz..
        float heavyStuffValue = 1 + 1;
    }
    
    private void MethodCalledWhenAsyncMethodCompleted(IAsyncResult ar)
    {
        AsyncResult asyncResult = (AsyncResult)ar;
        AsyncTaskDelegate asyncDelegate = (AsyncTaskDelegate)asyncResult.AsyncDelegate;
        string returnData = asyncDelegate.EndInvoke(ar);

        UnityEngine.Debug.Log("Data returned from async callback [ " + returnData + " ]");
    }
}
