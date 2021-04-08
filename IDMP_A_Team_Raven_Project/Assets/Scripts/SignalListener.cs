using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
   [SerializeField] private SignalSender signal;
    public UnityEvent signalEvent;

    public void OnsignalRaised()
    {
        signalEvent.Invoke();
    }

    private void OnEnable()
    {
        signal.RegisterListener(this);
    }

    private void OnDisable()
    {
        signal.DeRegisterListener(this);
    }
}

