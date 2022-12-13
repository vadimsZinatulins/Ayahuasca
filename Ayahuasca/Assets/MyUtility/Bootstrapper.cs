
using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute() => Object.Instantiate(Resources.Load("Systems"));
}
