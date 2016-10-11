using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

namespace inloop
{
    [CustomEditor ( typeof ( ColliderFit ) )]
    [CanEditMultipleObjects]
    public class ColliderFitEditor : Editor
    {
        public override void OnInspectorGUI ( )
        {
            DrawDefaultInspector ( );
            ColliderFit cf = ( ColliderFit ) target;
            GameObject go = cf.gameObject;
            var img = go.GetComponent<Image> ( );
            go.GetComponent<BoxCollider> ( ).size = new Vector3 ( img.rectTransform.rect.width + cf.offset.x, img.rectTransform.rect.height + cf.offset.y, 1f );
        }

    }
}