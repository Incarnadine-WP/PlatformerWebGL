using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// тестил как работает, в проекте не используется
public static class ComponentExtention
{
    public static void Activate(this Component component) => component.gameObject.SetActive(true);
    public static void Deactivate(this Component component) => component.gameObject.SetActive(false);
}
