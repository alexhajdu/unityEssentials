using UnityEngine;
using UnityEditor;

    public class Demo_SO : ScriptableObject
    {
        public int foo;

        [MenuItem ( "Alex/Create/SO" )]
        public static void CreateAsset ( )
        {
            ScriptableObjectUtility.CreateAsset<Demo_SO> ( );
        }
    }
