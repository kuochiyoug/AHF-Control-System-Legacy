using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Co1umbine
{
    [ExecuteInEditMode]
    public class LineRendererNode : MonoBehaviour
    {
        [SerializeField] private Transform parentNodes;
        [SerializeField] private Color color;
        [SerializeField] private float width;

        private LineRenderer line;

        // Start is called before the first frame update
        void Start()
        {
            if(this.TryGetComponent<LineRenderer>(out LineRenderer line))
            {
                this.line = line;
            }
            else
            {
                this.line = gameObject.AddComponent<LineRenderer>();
            }

            this.line.startColor = color;
            this.line.endColor = color;
            this.line.startWidth = 0;
            this.line.endWidth = width;
            this.line.material = new Material(Shader.Find("Sprites/Default"));

            if(parentNodes == null)
            {
                parentNodes = transform.parent;
            }
        }
        private void Update()
        {
            this.line.SetPositions(new Vector3[] { transform.position, parentNodes.position });
            var boneLen = (parentNodes.position - transform.position).magnitude;
            this.line.startColor = color;
            this.line.endColor = color;
            this.line.endWidth = this.width * boneLen;
        }
    }
}