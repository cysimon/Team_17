using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestOnClick : MonoBehaviour
{
    public Button curr;
    // Start is called before the first frame update
    void Start()
    {
        curr.onClick.AddListener(Listener);
    }



    void Listener()
    {
        EventBus.Publish<TestEvent>(new TestEvent(true));
    }
}
