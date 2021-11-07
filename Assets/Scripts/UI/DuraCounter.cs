using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityEvent 
{
    public int index = 0;
    public int new_count = 100;

    public DurabilityEvent(int _index, int _new_count)
    {
        index = _index;
        new_count = _new_count;
    }
}

public class DuraCounter : MonoBehaviour
{
    // 0 - NPC 1 Weapon
    // 1 - NPC 1 Armor
    // 2 - NPC 2 Weapon
    // 3 - NPC 2 Armor
    // 4 - NPC 3 Weapon
    // 5 - NPC 3 Armor
    // 6 - Boss Weapon
    // 7 - Boss Armor
    public Text[] curr;
    Subscription<DurabilityEvent> dura_subscription;

    void Start()
    {
        for (int i = 0; i < curr.Length; i++) curr[i].text = "100/100";

        dura_subscription = EventBus.Subscribe<DurabilityEvent>(Listener);
    }

    void Listener(DurabilityEvent event_in)
    {
        curr[event_in.index].text = event_in.new_count.ToString() + "/100";
    }
}
