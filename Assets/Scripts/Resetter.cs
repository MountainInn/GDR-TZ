using System.Linq;
using UnityEngine;

public class Resetter : MonoBehaviour
{
    public void ResetAllResetables()
    {
        var resetables = FindObjectsOfType<MonoBehaviour>().OfType<IResetable>();
        foreach (var item in resetables)
        {
            item.Reset();
        }
    }
}
