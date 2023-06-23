using NSmirnov.Samples.Foundation;
using UnityEngine;

[RequireComponent(typeof(NSmirnov.Core.UI.Button))]
public class LoadItem : MonoBehaviour
{
    private Sample sample => Sample.Instance;
    private NSmirnov.Core.UI.Button button;
    [SerializeField] private ItemClass itemClass;

    private void Awake()
    {
        button = GetComponent<NSmirnov.Core.UI.Button>();
        button.onClick.AddListener(() =>
        {
            sample.LoadItems(itemClass);
        });
    }
}