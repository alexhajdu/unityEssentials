/*
 * 
 * Last update: 18.04.2017
 * AH
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Object = UnityEngine.Object;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using AH.Helpers;
using System.Collections.ObjectModel;

namespace AH.Helpers
{
	public class Helpers
	{
		public static int GetIndexFromBitmaskedArray ( int input )
		{
			return ( int ) Mathf.Log ( input, 2 );
		}

		//	Advanced modulo

		public static int mod ( int k, int n )
		{
			return ( ( k %= n ) < 0 ) ? k + n : k;
		}

		public static void RGBToHSV ( Color c, out float h, out float s, out float v )
		{

			float r = c.r;
			float g = c.g;
			float b = c.b;

			float min, max, delta;
			min = Mathf.Min ( Mathf.Min ( r, g ), b );
			max = Mathf.Max ( Mathf.Max ( r, g ), b );
			v = max;
			delta = max - min;

			if ( max != 0.0f )
				s = delta / max;
			else
			{
				// r = g = b = 0		// s = 0, v is undefined
				v = 0;
				s = 0;
				h = 0;
				return;
			}
			if ( r == max )
			{
				h = ( g - b ) / delta;        // between yellow & magenta
			}
			else if ( g == max )
			{
				h = 2 + ( b - r ) / delta;    // between cyan & yellow
			}
			else
			{
				h = 4 + ( r - g ) / delta;    // between magenta & cyan
			}
			h *= 60;                // degrees
			if ( h < 0 )
			{
				h += 360;
			}
		}

		public static Color HSVToRGB ( float h, float s, float v )
		{

			while ( h < 0 )
			{
				h += 360;
			}
			;
			while ( h >= 360 )
			{
				h -= 360;
			}
			;

			float hf = h / 60.0f;
			int i = Mathf.FloorToInt ( hf );
			float f = hf - i;
			float pv = v * ( 1 - s );
			float qv = v * ( 1 - s * f );
			float tv = v * ( 1 - s * ( 1 - f ) );

			switch ( i )
			{

				case 0:
					return new Color ( v, tv, pv );
				case 1:
					return new Color ( qv, v, pv );
				case 2:
					return new Color ( pv, v, tv );
				case 3:
					return new Color ( pv, qv, v );
				case 4:
					return new Color ( tv, pv, v );
				case 5:
					return new Color ( v, pv, qv );
				case 6:
					return new Color ( v, tv, pv );
				case -1:
					return new Color ( v, pv, qv );
				default:
					return new Color ( v, v, v );

			}
		}
	}
}

public static class Check
{
	public static void Null ( Object obj )
	{
		if ( obj == null )
		{
			Debug.LogError ( "Object should not be null." );
		}
	}

	public static void Array ( Object [] objs, int expectedCount, bool minimum = false )
	{

		if ( expectedCount > 0 && ( ( !minimum && objs.Length != expectedCount ) || ( minimum && objs.Length < expectedCount ) ) )
		{
			Debug.LogError ( "Array is not as long as expected." );
			return;
		}

		for ( int i = 0; i < objs.Length; i++ )
		{

			if ( objs [ i ] == null )
			{
				Debug.LogError ( "Element in this array should not be null." );
				return;
			}
		}
	}

	public static void LayerMask ( LayerMask layerMask )
	{
		if ( layerMask.value == 0 )
		{
			Debug.LogError ( "Layer mask should not be empty." );
		}
	}

	public static void Zero ( int num )
	{
		if ( num == 0 )
		{
			Debug.LogError ( "This variable should not be 0." );
		}
	}

	public static void String ( string s )
	{
		if ( s == null || s == "" )
		{
			Debug.LogError ( "This string should not be null or empty" );
		}
	}
}

public static class ExtensionMethods
{
	#region List

	public static bool IsNullOrEmpty<T> ( this List<T> list )
	{
		if ( list == null || list.Count == 0 )
		{
			Debug.LogWarning ( "This list should be not null or empty" );
			return true;
		}

		return false;
	}

	#endregion

	#region Array

	public static bool IsNullOrEmpty<T> ( this T [] array )
	{
		if ( array == null || array.Length == 0 )
		{
			Debug.LogWarning ( "This array should be not null or empty" );
			return true;
		}

		return false;
	}

	#endregion

	public static bool ContainsLayer ( this LayerMask layerMask, int layer )
	{
		return ( layerMask.value & ( 1 << layer ) ) != 0;
	}

	#region Color

	public static Color SaturatedColor ( this Color color, float saturation )
	{
		float h, s, v;

		Helpers.RGBToHSV ( color, out h, out s, out v );
		s = saturation;
		return Helpers.HSVToRGB ( h, s, v );
	}

	public static Color GetColorFromString( this Color color, string colorString )
	{
		color = Color.clear;
		ColorUtility.TryParseHtmlString ( colorString, out color );
		return color;
	}

	/// <summary>
	/// Saturates the color by percent -> 1.2 ( 20 more ), 0.8 ( 20% less )
	/// </summary>
	/// <returns>The color by percent.</returns>
	/// <param name="color">Color.</param>
	/// <param name="percentValue">Saturation increase.</param>
	public static Color SaturateColorByPercent ( this Color color, float percentValue )
	{
		float h, s, v;

		Helpers.RGBToHSV ( color, out h, out s, out v );
		s *= percentValue;
		return Helpers.HSVToRGB ( h, s, v );
	}

	/// <summary>
	/// Adjusts the HSV v value by percent 
	/// </summary>
	/// <returns>The HS v value by percent.</returns>
	/// <param name="color">Color.</param>
	/// <param name="percentValue">Percent value -> 1.2 ( 20% more ), 0.8 ( 20% less )</param>
	public static Color Adjust_HSV_ValueByPercent ( this Color color, float percentValue )
	{
		float h, s, v;

		Helpers.RGBToHSV ( color, out h, out s, out v );
		v *= percentValue;
		return Helpers.HSVToRGB ( h, s, v );
	}


	public static Color ColorWithAlpha ( this Color color, float alpha )
	{
		color.a = alpha;
		return color;
	}

	public static Color ColorWithValue ( this Color color, float value )
	{
		float h, s, v;

		Helpers.RGBToHSV ( color, out h, out s, out v );
		v = value;
		return Helpers.HSVToRGB ( h, s, v );
	}

	public static string ColorToRGBHex ( this Color color )
	{
		return string.Format ( "#{0:X2}{1:X2}{2:X2}", ToByte ( color.r ), ToByte ( color.g ), ToByte ( color.b ) );
	}

	private static byte ToByte ( float f )
	{
		f = Mathf.Clamp01 ( f );
		return ( byte ) ( f * 255 );
	}
		
	#endregion

	#region GameObject

	public static void SetLayerRecursively ( this GameObject obj, int layer )
	{
		obj.layer = layer;

		foreach ( Transform child in obj.transform )
		{
			child.gameObject.SetLayerRecursively ( layer );
		}
	}

	public static T GetFirstComponentUpwardWithInterface<T>(this GameObject inGameObject) where T : class
    {
        return GameObjectUtils.GetFirstComponentUpwardWithInterface<T>(inGameObject);
    }

    public static T GetFirstComponentUpward<T>(this GameObject inGameObject) where T : Component
    {
        return GameObjectUtils.GetFirstComponentUpward<T>(inGameObject);
    }

    public static T GetComponentWithInterface<T>(this GameObject inGameObject) where T : class
    {
        return GameObjectUtils.GetComponentWithInterface<T>(inGameObject);
    }

    public static string GetFullName(this GameObject inGameObject)
    {
        return GameObjectUtils.GetFullName(inGameObject);
    }

	#endregion

	#region Bounds

	public static Bounds GrowBounds ( this Bounds a, Bounds b )
	{
		Vector3 max = Vector3.Max ( a.max, b.max );
		Vector3 min = Vector3.Min ( a.min, b.min );

		a = new Bounds ( ( max + min ) * 0.5f, max - min );
		return a;
	}

	#endregion

	#region Image and SpriteRenderer using DG TWeening

	public static Tween DOFade_AH ( this Image img, float endAlpha, float duration, float delay, Ease ease = Ease.OutExpo, Action onComplete = null )
	{
		return DOTween.To ( () => img.color.a, x => {
			var cl = img.color;
			cl.a = x;
			img.color = cl;
		}, endAlpha, duration ).SetDelay ( delay ).SetEase ( ease ).OnComplete ( () => {
			if ( onComplete != null )
				onComplete ();
		} );
	}

	public static Tween DOFadeLooping_AH ( this Image img, float endAlpha, float duration, float delay, Ease ease = Ease.OutExpo, int loops = -1, LoopType loopType = LoopType.Yoyo )
	{
		return DOTween.To ( () => img.color.a, x => {
			var cl = img.color;
			cl.a = x;
			img.color = cl;
		}, endAlpha, duration ).SetDelay ( delay ).SetLoops ( loops, loopType ).SetEase ( ease );
	}

	public static Tween DOColor_AH ( this Image img, Color endColor, float duration, float delay, Ease ease = Ease.OutExpo, Action onComplete = null )
	{
		return DOTween.To ( () => img.color, x => {
			img.color = x;
		}, endColor, duration ).SetDelay ( delay ).SetEase ( ease ).OnComplete ( () => {
			if ( onComplete != null )
				onComplete ();
		} );
	}

	public static Tween DOFade_AH ( this SpriteRenderer img, float endAlpha, float duration, float delay, Ease ease = Ease.OutExpo, Action onComplete = null )
	{
		return DOTween.To ( () => img.color.a, x => {
			var cl = img.color;
			cl.a = x;
			img.color = cl;
		}, endAlpha, duration ).SetDelay ( delay ).SetEase ( ease ).OnComplete ( () => {
			if ( onComplete != null )
				onComplete ();
		} );
	}

	public static Tween DOColor_AH ( this SpriteRenderer img, Color endColor, float duration, float delay, Ease ease = Ease.OutExpo, Action onComplete = null )
	{
		return DOTween.To ( () => img.color, x => {
			img.color = x;
		}, endColor, duration ).SetDelay ( delay ).SetEase ( ease ).OnComplete ( () => {
			if ( onComplete != null )
				onComplete ();
		} );
	}

	public static Tween DOFade_AH ( this RawImage img, float endAlpha, float duration, float delay, Ease ease = Ease.OutExpo, Action onComplete = null )
	{
		return DOTween.To ( () => img.color.a, x => {
			var cl = img.color;
			cl.a = x;
			img.color = cl;
		}, endAlpha, duration ).SetDelay ( delay ).SetEase ( ease ).OnComplete ( () => {
			if ( onComplete != null )
				onComplete ();
		} );
	}

	public static Tween DOColor_AH ( this RawImage img, Color endColor, float duration, float delay, Ease ease = Ease.OutExpo, Action onComplete = null )
	{
		return DOTween.To ( () => img.color, x => {
			img.color = x;
		}, endColor, duration ).SetDelay ( delay ).SetEase ( ease ).OnComplete ( () => {
			if ( onComplete != null )
				onComplete ();
		} );
	}

	public static Tween DOFade_AH ( this Text label, float endAlpha, float duration, float delay, Ease ease = Ease.OutExpo, Action onComplete = null )
	{
		return DOTween.To ( () => label.color.a, x => {
			var cl = label.color;
			cl.a = x;
			label.color = cl;
		}, endAlpha, duration ).SetDelay ( delay ).SetEase ( ease ).OnComplete ( () => {
			if ( onComplete != null )
				onComplete ();
		} );
	}

	public static Tween DOColor_AH ( this Text label, Color endColor, float duration, float delay, Ease ease = Ease.OutExpo, Action onComplete = null )
	{
		return DOTween.To ( () => label.color, x => {
			label.color = x;
		}, endColor, duration ).SetDelay ( delay ).SetEase ( ease ).OnComplete ( () => {
			if ( onComplete != null )
				onComplete ();
		} );
	}

	#endregion

	internal class GameObjectUtils {

    public static T GetFirstComponentUpward<T>(GameObject inGameObject) where T : Component
    {
        if(inGameObject == null)
            return null;

        T t = inGameObject.GetComponent<T>();

        if(t != null)
            return t;
		
		if(inGameObject.transform.parent == null || inGameObject.transform.parent.gameObject == null)
			return null;

        return GetFirstComponentUpward<T>(inGameObject.transform.parent.gameObject);
    }

	//	------------------

    public static T GetComponentWithInterface<T>(GameObject inGameObject) where T : class
    {
        foreach(Component comp in inGameObject.GetComponents<Component>())
        {
            T t = comp as T;
            if(t != null)
                return t;
        }

        return default(T);
    }

    public static T GetFirstComponentUpwardWithInterface<T>(GameObject inGameObject) where T : class
    {
        if(inGameObject != null)
        {
            foreach(Component comp in inGameObject.GetComponents<Component>())
            {
                T t = comp as T;
                if(t != null)
                    return t;
            }

            if(inGameObject.transform.parent != null && inGameObject.transform.parent.gameObject != null)
            {   return GetFirstComponentUpwardWithInterface<T>(inGameObject.transform.parent.gameObject);  }

        }

        return default(T);
    }
    static public string GetFullName(GameObject inObject)
    {
        if(inObject)
        {
            if(inObject.transform.parent)
                return GetFullName(inObject.transform.parent.gameObject) + "/" + inObject.name;

            return inObject.name;
        }

        return "";
    }
}
}
