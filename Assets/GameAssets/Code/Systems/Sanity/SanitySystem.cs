using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SanitySystem {
    [SerializeField] private FloatReference SanityLevel;

    public SanitySystem() 
    {
        Bootstrap.EventBrokerComponent.Subscribe<Event.AddSanity>(AddSanityHandler);
    }

    ~SanitySystem()
    {
        Bootstrap.EventBrokerComponent.Unsubscribe<Event.AddSanity>(AddSanityHandler);
    }

    private void AddSanityHandler(BrokerEvent<Event.AddSanity> obj)
    {
        throw new NotImplementedException();
    }
}
