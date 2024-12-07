using LibraryNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;

    private Dictionary<int, Entity> _entities = new();

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateEntity(string message)
    {
        var newEnt = new Entity(message);
        if (_entities.TryGetValue(newEnt.Id, out Entity entity))
        {
            entity.FieldValue = newEnt.FieldValue;
        }
        else
        {
            _entities.Add(newEnt.Id, newEnt);
        }

        UiManager.Instance.UpdateEnts(_entities.Select(e => e.Value.PresentText).ToList());
    }
}