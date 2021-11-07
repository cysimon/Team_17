using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundStage { LV1, LV2, LV3 };

public class BackgroundChangeEvent
{
    public BackgroundStage input;

    public BackgroundChangeEvent(BackgroundStage _in) 
    {
        input = _in;
    }
}

public class BackgroundController : MonoBehaviour
{
    // 0 - Lv1
    // 1 - Lv2
    // 2 - Lv3
    public Sprite[] backgroundImages;
    Subscription<BackgroundChangeEvent> subscription;
    public SpriteRenderer thisRenderer;

    // Start is called before the first frame update
    void Start()
    {
        subscription = EventBus.Subscribe<BackgroundChangeEvent>(Listener);
    }

    void Listener(BackgroundChangeEvent event_in) {
        if (event_in.input == BackgroundStage.LV2)
        {
            thisRenderer.sprite = backgroundImages[1];
        }
        else if (event_in.input == BackgroundStage.LV3)
        {
            thisRenderer.sprite = backgroundImages[2];
        }
        else
        {
            thisRenderer.sprite = backgroundImages[0];
        }
    }
}
