using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtensionMethods
{
    public static TInterface GetInterface<TInterface>(this Component component) where TInterface : class
    {
        TInterface targetInterface = null;

        if(typeof(TInterface).IsInterface == true)
        {
            Component[] allComponents = component.GetAllComponents();

            for (int i = 0; i < allComponents.Length && targetInterface == null; i++)
            {
                targetInterface = allComponents[i] as TInterface;
            }
        }

        return targetInterface;
    }

    public static TInterface[] GetInterfaces<TInterface>(this Component component) where TInterface : class
    {
        List<TInterface> targetInterfaces = new List<TInterface>();

        if (typeof(TInterface).IsInterface == true)
        {
            Component[] allComponents = component.GetAllComponents();

            for (int i = 0; i < allComponents.Length; i++)
            {
                if(allComponents[i] is TInterface)
                {
                    targetInterfaces.Add(allComponents[i] as TInterface);
                }
            }
        }

        return targetInterfaces.ToArray();
    }

    private static Component[] GetAllComponents(this Component component)
    {
        return component.GetComponents<Component>();
    }
}
