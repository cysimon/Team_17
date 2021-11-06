using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEvent
{
    public bool on = false;

    public TestEvent(bool _on)
    {
        on = _on;
    }
}
public class Test : MonoBehaviour
{
    public int i = 0;
    Subscription<TestEvent> subscription;
    Text counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = GetComponent<Text>();
        subscription = EventBus.Subscribe<TestEvent>(Listener);
    }

    // Update is called once per frame
    void Update()
    {
        counter.text = i.ToString("0");
    }

    void Listener(TestEvent event_in)
    {
        if (event_in.on)
        {
            i++;
        }
    }
}
