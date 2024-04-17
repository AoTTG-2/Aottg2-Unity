using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeTodoList
{
	[Serializable]
	public class CTL_KeywordObject
	{
		public const char SEPARATOR = '¤'; //CTL...¤ can't be used in the key as it act as separator

		[SerializeField]
		string mKey;
		[SerializeField]
		Color mColor;
		[SerializeField]
		bool mVisible;
		[SerializeField]
		bool mValid;

		public bool mNeedFocus = false;

		public bool mIsDragged = false;

		public Texture2D mTexture;

		public Texture2D GetTexture()
		{
			if (mTexture == null)
				RefreshTexture();
			return mTexture;
		}

		public CTL_KeywordObject()
		{
			mKey = "";
			mColor = new Color(.25f, 1f, .5f, 1f);
			mVisible = true;
			RefreshTexture();
		}

		public void RefreshTexture()
		{
			Color fadeColor = new Color(mColor.r, mColor.g, mColor.b, 0);
			if(mTexture == null)
				mTexture = new Texture2D(2, 1);

			switch (CTL_Settings.ElementColorType)
			{
				case CTL_Settings.eElementColorType.SOLID:
					mTexture.SetPixel(0, 0, mColor);
					mTexture.SetPixel(1, 0, mColor);
					break;
				case CTL_Settings.eElementColorType.GRADIENT_LEFT:
					mTexture.SetPixel(0, 0, fadeColor);
					mTexture.SetPixel(1, 0, mColor);

					break;
				case CTL_Settings.eElementColorType.GRADIENT_RIGHT:
					mTexture.SetPixel(0, 0, mColor);
					mTexture.SetPixel(1, 0, fadeColor);
					break;
			}

			mTexture.filterMode = FilterMode.Trilinear;
			mTexture.alphaIsTransparency = true;
			mTexture.Apply();
		}

		public string KeySearch
		{
			//get { return CTL_Settings.CaseSensitive?"//" + Key: "//" + Key.ToLower(); }
			get { return CTL_Settings.CaseSensitive ? Key : Key.ToLower(); }
		}

		public bool SimilarKey(CTL_KeywordObject other)
		{
			return (CTL_Settings.CaseSensitive && Key == other.Key) || (!CTL_Settings.CaseSensitive && Key.ToLower() == other.Key.ToLower());
		}

		public string Key
		{
			get { return mKey; }
			set
			{
				string newKey = value.Replace(SEPARATOR.ToString(), "").Replace(CTL_KeywordList.SEPARATOR.ToString(), "");
				if(newKey != mKey)
				{
					mKey = newKey;
					CTL_Settings.KeywordsList.KeywordModified(this, true);
				}
			}
		}

		public Color Color
		{
			get { return mColor; }
			set
			{
				if (value != mColor)
				{
					mColor = value;
					CTL_Settings.KeywordsList.KeywordModified(this, false);
					RefreshTexture();
				}
			}
		}

		public bool Visible
		{
			get { return mVisible; }
			set
			{
				if (value != mVisible)
				{
					CTL_TodoObject.sNeedRefreshDisplayedElementsList = true;
					mVisible = value;
					CTL_Settings.KeywordsList.KeywordModified(this, false);
				}
			}
		}

		public bool Valid
		{
			get { return mValid; }
			set
			{
				if (value != mValid)
				{
					mValid = value;
					CTL_Settings.KeywordsList.KeywordModified(this, false);
				}
			}
		}

		public string Export()
		{
			return	mKey + 
					SEPARATOR + "#" + ColorUtility.ToHtmlStringRGBA(mColor) + 
					SEPARATOR + mVisible.ToString() + 
					SEPARATOR + mValid.ToString();
		}

		public static CTL_KeywordObject LoadFromString(string str)
		{
			CTL_KeywordObject obj = new CTL_KeywordObject();
			string[] splitted = str.Split(SEPARATOR);
			try
			{
				obj.Key = splitted[0];
				Color col = Color.black;
				ColorUtility.TryParseHtmlString(splitted[1], out col);
				obj.Color = col;
				bool.TryParse(splitted[2], out obj.mVisible);
				bool.TryParse(splitted[3], out obj.mValid);

				if(obj.mColor.a == 0)
				{
					obj.mColor.a = 1;
				}
			}
			catch (Exception)
			{
				obj.mValid = false;
			}

			return obj;
		}
	}
}

