using System;
using UnityEngine;

public interface IAppSystem
{
    bool Exists();
    void OnInstantiation();
}

/// <summary>
/// A component similar to a singleton, peristent through all scenes.
/// Accessed through Instance member.
/// See startup-behaviour doc
///  
/// ## How to use
/// ### From scratch (new project)
/// Create a prefab named "Systems" at the root level of a "Resources" folder.
/// ### Extending
/// For each game system (component inheriting from AppSystem), create a child on Systems and attach component.
/// The order of children matters. First children instantiated first. 
/// Components to use "AwakeSingleton" for regular constructor/awake stuff.
/// Order of "Awake" calls not guaranteed, so don't use it./ 
/// </summary>
public class AppSystem<T> : MonoBehaviour, IAppSystem where T : AppSystem<T>
{
    [SerializeField][ReadOnly] private bool instantiated = false;
    private const string systemPrefabName = "AppSystems";
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                _CreateSystems();
            }
            return instance;
        }
    }

    public bool Exists()
    {
        return instantiated;
    }

    private static bool DoesSystemObjectExist()
    {
        return GameObject.Find(systemPrefabName) != null;
    }

    private static void CreateSystems()
    {
        if (DoesSystemObjectExist())
        {
            Debug.LogWarning(systemPrefabName + " object already exists, not creating new");
        }
        else
        {
            _CreateSystems();
        }
    }

    private static void _CreateSystems()
    {
        if (DoesSystemObjectExist())
        {
            // Debug.LogError("Attempt to create " + systemPrefabName + " object when one already exists.\n" + 
            // "Are the components all in the systems prefab and in order?");
            return;
        }

        GameObject systems = Instantiate(GetSystemPrefab());
        systems.name = systemPrefabName;

        // How many active systems are there?
        int nSystems = 0;
        foreach (Transform child in systems.transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                if (child.TryGetComponent(out IAppSystem system))
                {
                    nSystems += 1;
                }
            }
        }

        // Instantiate all the systems
        int nSystemsActive = 0;
        for (int i = 0; i < nSystems; i++)
        {
            foreach (Transform child in systems.transform)
            {
                if (child.gameObject.activeInHierarchy)
                {
                    if (child.TryGetComponent(out IAppSystem system))
                    {
                        if (!system.Exists())
                        {
                            system.OnInstantiation();
                            if (system.Exists())
                            {
                                nSystemsActive += 1;

                            }
                        }
                    }
                }
                if (nSystemsActive == nSystems)
                {
                    break;
                }
            }
            if (nSystemsActive == nSystems)
            {
                break;
            }
        }

        // What systems (if any) failed?
        if (nSystemsActive != nSystems)
            foreach (Transform child in systems.transform)
            {
                if (child.gameObject.activeInHierarchy)
                {
                    if (child.TryGetComponent(out IAppSystem system))
                    {
                        if (!system.Exists())
                        {
                            Debug.LogError(string.Format("Could not instantiate all Systems. {0} failed", system));
                        }
                    }
                }
            }
    }

    private static GameObject GetSystemPrefab()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("");

        GameObject systemPrefab = null;
        foreach (GameObject prefab in prefabs)
        {
            if (prefab.name == systemPrefabName)
            {
                systemPrefab = prefab;
                break;
            }
        }

        if (systemPrefab == null)
        {
            Debug.LogError("Could not find prefab: " + systemPrefabName);
        }

        return systemPrefab;
    }

    public void OnInstantiation()
    {
        try
        {
            instance = this.GetComponent<T>();  // this seems wrong
            AwakeSystem();
            instantiated = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning(string.Format("Error instiating system: {0}\n{1}", name, e));
        }
    }

    protected virtual void AwakeSystem() { }
}