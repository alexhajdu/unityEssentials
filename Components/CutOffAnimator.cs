using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace inloop
{
    public class CutOffAnimator : MonoBehaviour
    {
        public event System.Action OnTransitionEnded;

        public enum E_TransitionType { Show, Hide };

        [SerializeField]
        [Tooltip ( "In seconds" )]
        private float _transitionDuration;  //  In seconds

        [SerializeField]
        private Ease _ease;

        private float _cutoffParam;
        private Renderer _renderer;

        const string CUTOFF = "_Cutoff";

        #region Mono

        private void Awake ( )
        {
            _renderer = GetComponent<Renderer> ( );
        }

        void Update ( )
        {
            _cutoffParam = _renderer.material.GetFloat ( CUTOFF );
            Debug.Log ( _cutoffParam );
        }

        #endregion

        #region API

        public void DoTransition ( E_TransitionType type, float delay, float duration = -1, System.Action callback = null )
        {
            StartCoroutine ( IE_DoTransition ( type, delay, duration, callback ) );
        }

        public IEnumerator IE_DoTransition ( E_TransitionType type, float delay, float duration = -1f, System.Action callback = null )
        {
            //  Delay
            yield return new WaitForSeconds ( delay );

            //  Decide if use component or param duration
            duration = duration == -1f ? _transitionDuration : duration;

            float i;
            switch ( type )
            {
                case E_TransitionType.Show:

                    //  Set to 0
                    i = 0f;
                    _renderer.material.SetFloat ( CUTOFF, i );

                    //  Tween param
                    DOTween.To ( ( ) => i, x => i = x, 1, duration ).SetEase ( _ease );

                    //  Do transition
                    while ( _renderer.material.GetFloat ( CUTOFF ) < 1f )
                    {
                        _renderer.material.SetFloat ( CUTOFF, i );
                        yield return null;
                    }

                    //  Callback
                    if ( callback != null ) callback ( );
                    if ( OnTransitionEnded != null ) OnTransitionEnded ( );

                    break;

                case E_TransitionType.Hide:

                    //  Set to 1
                    i = 1f;
                    _renderer.material.SetFloat ( CUTOFF, i );

                    //  Tween param
                    DOTween.To ( ( ) => i, x => i = x, 0, duration ).SetEase ( _ease );

                    //  Do transition
                    while ( _renderer.material.GetFloat ( CUTOFF ) > 0f )
                    {
                        _renderer.material.SetFloat ( CUTOFF, i );
                        yield return null;
                    }

                    //  Callback
                    if ( callback != null ) callback ( );
                    if ( OnTransitionEnded != null ) OnTransitionEnded ( );

                    break;
            }
        }

        #endregion
    }
}