using UnityEngine;
using UnityEngine.UI;

namespace NSmirnov.Core.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class EmptyGraphic : Graphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}