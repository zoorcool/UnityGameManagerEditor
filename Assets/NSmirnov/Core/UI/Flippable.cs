using UnityEngine;
using UnityEngine.UI;

namespace NSmirnov.Core.UI
{
    [RequireComponent(typeof(RectTransform), typeof(Graphics))]
    public class Flippable : BaseMeshEffect
    {
        [SerializeField] private bool m_Horizontal = false;
        [SerializeField] private bool m_Veritical = false;

#if UNITY_EDITOR
        protected override void Awake()
        {
            OnValidate();
        }
#endif
        public bool horizontal
        {
            get { return this.m_Horizontal; }
            set { this.m_Horizontal = value; }
        }
        public bool vertical
        {
            get { return this.m_Veritical; }
            set { this.m_Veritical = value; }
        }
        public override void ModifyMesh(VertexHelper verts)
        {
            RectTransform rt = this.transform as RectTransform;

            for (int i = 0; i < verts.currentVertCount; ++i)
            {
                UIVertex uiVertex = new UIVertex();
                verts.PopulateUIVertex(ref uiVertex, i);

                uiVertex.position = new Vector3(
                    (this.m_Horizontal ? (uiVertex.position.x + (rt.rect.center.x - uiVertex.position.x) * 2) : uiVertex.position.x),
                    (this.m_Veritical ? (uiVertex.position.y + (rt.rect.center.y - uiVertex.position.y) * 2) : uiVertex.position.y),
                    uiVertex.position.z
                );

                verts.SetUIVertex(uiVertex, i);
            }
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            var components = gameObject.GetComponents(typeof(BaseMeshEffect));
            foreach (var comp in components)
            {
                if (comp.GetType() != typeof(Flippable))
                {
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
                }
                else break;
            }
            this.GetComponent<Graphic>().SetVerticesDirty();
            base.OnValidate();
        }
#endif
    }
}