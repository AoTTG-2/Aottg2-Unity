using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace CodeTodoList
{
	public class CTL_Tools
	{
		public static long ThreadSafeTimeStamp()
		{
			return System.Diagnostics.Stopwatch.GetTimestamp() / (System.Diagnostics.Stopwatch.Frequency / 1000);
		}

		public static int GetShortcutId(EventModifiers mod)
		{
			switch (mod)
			{
				case EventModifiers.Shift:
					return 0;
				case EventModifiers.Control:
					return 1;
				case EventModifiers.Alt:
					return 2;
				case EventModifiers.Command:
					return 3;
				case EventModifiers.None:
					return 4;
				default:
					return 0;
			}
		}


		public static Color GenerateRandomKeywordColor()
		{

			Color _col = new Color();

			float[] rgb = new float[3] { UnityEngine.Random.Range(.6f, 1f), UnityEngine.Random.Range(.6f, 1f), UnityEngine.Random.Range(.6f, 1f) };

			float sum = rgb[0] + rgb[1] + rgb[2];
			if (sum > 2.5f)
			{
				rgb[UnityEngine.Random.Range(0, 3)] = .6f;
			}
			else if (sum <= 2.5f)
			{
				rgb[UnityEngine.Random.Range(0, 3)] = 1f;
			}
			_col.r = rgb[0];
			_col.g = rgb[1];
			_col.b = rgb[2];
			_col.a = 1;

			return _col;
		}

		public static object TruncateDecimal(object value, int decimalToKeep)
		{
			if (value is float)
			{
				return TruncateDecimal(Convert.ToSingle(value), decimalToKeep);
			}
			if (value is double)
			{
				return TruncateDecimal(Convert.ToDouble(value), decimalToKeep);
			}
			return value;
		}

		public static float TruncateDecimal(float value, int decimalToKeep)
		{
			float _multVal = Mathf.Pow(10f, decimalToKeep);
			int aux = (int)(value * _multVal);
			return aux / _multVal;
		}

		public static double TruncateDecimal(double value, int decimalToKeep)
		{
			double _multVal = Math.Pow(10f, decimalToKeep);
			int aux = (int)(value * _multVal);
			return aux / _multVal;
		}

		public static string RemoveFirstLines(string strIn, int linesToRemoveCount)
		{
			if (string.IsNullOrEmpty(strIn) || linesToRemoveCount <= 0)
				return strIn;
			try
			{
				int _ToRemoveId = strIn.IndexOf("\n") + 1;
				for (int i = 1; i < linesToRemoveCount; i++)
				{
					_ToRemoveId = strIn.IndexOf("\n", _ToRemoveId) + 1;
				}
				strIn = strIn.Remove(0, _ToRemoveId);
			}
			catch (Exception) { }

			return strIn;
		}

		public static bool IsNumericType(Type type)
		{
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Decimal:
				case TypeCode.Double:
				case TypeCode.Single:
					return true;
				default:
					return false;
			}
		}

		public static Texture2D ChangeTextureColor(Texture2D inTex, Color col)
		{
			Texture2D result = new Texture2D(inTex.width, inTex.height, inTex.format, false);

			Graphics.CopyTexture(inTex, result);

			Color[] _newCol = { col };
			float _a;
			for (int x = 0; x < inTex.width; x++)
			{
				for (int y = 0; y < inTex.height; y++)
				{
					_a = result.GetPixel(x, y).a;
					if (_a > 0f)
					{
						_newCol[0].a = _a;
						result.SetPixels(x, y, 1, 1, _newCol);
					}

				}
			}
			result.Apply();
			return result;
		}

#if UNITY_EDITOR
		protected static NamedBuildTarget GetNameBuildTarget()
		{
			BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
			BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
			NamedBuildTarget namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
			return namedBuildTarget;
		}
#endif

		public static void AddDefine(string define)
		{
			if (string.IsNullOrEmpty(define))
				return;
			List<string> _list = new List<string>();
			_list.Add(define);
			AddDefines(_list);
		}

		public static void AddDefines(List<string> definesToAdd)
		{
#if UNITY_EDITOR
			if (definesToAdd == null || definesToAdd.Count == 0)
				return;

			string _curDefines = PlayerSettings.GetScriptingDefineSymbols(GetNameBuildTarget());
			List<string> _definesList = new List<string>(_curDefines.Split(';'));

			foreach (string define in definesToAdd)
			{
				if (!_definesList.Contains(define))
					_definesList.Add(define);
			}

			InternalApplyDefine(string.Join(";", _definesList.ToArray()));
#endif
		}

		public static void RemoveDefine(string define)
		{
			if (string.IsNullOrEmpty(define))
				return;
			List<string> _list = new List<string>();
			_list.Add(define);
			RemoveDefines(_list);
		}

		public static void RemoveDefines(List<string> definesToRemove)
		{
#if UNITY_EDITOR
			if (definesToRemove == null || definesToRemove.Count == 0)
				return;

			string _curDefines = PlayerSettings.GetScriptingDefineSymbols(GetNameBuildTarget());
			List<string> _definesList = new List<string>(_curDefines.Split(';'));

			foreach (var define in definesToRemove)
			{
				_definesList.Remove(define);
			}

			InternalApplyDefine(string.Join(";", _definesList.ToArray()));
#endif
		}

		private static void InternalApplyDefine(string define)
		{
#if UNITY_EDITOR
			PlayerSettings.SetScriptingDefineSymbols(GetNameBuildTarget(), define);
#endif
		}

		public static void RemovePreprocessorDefinition(string toRemove)
		{
#if UNITY_EDITOR
			string _curDefines = PlayerSettings.GetScriptingDefineSymbols(GetNameBuildTarget());
			string[] _allDefines = _curDefines.Split(';');
			string _newDefine = "";
			for (int i = 0; i < _allDefines.Length; i++)
			{
				if (_allDefines[i] == toRemove)
					continue;
				_newDefine += _allDefines[i];
				if (i < _allDefines.Length - 1)
					_newDefine += ";";
			}
			if (_newDefine.EndsWith(";"))
				_newDefine = _newDefine.Substring(0, _newDefine.Length - 1);
			PlayerSettings.SetScriptingDefineSymbols(GetNameBuildTarget(), _newDefine);
#endif
		}

		public static string NormalizePath(string path)
		{
			if (string.IsNullOrEmpty(path))
				return null;
			return Path.GetFullPath(new Uri(path).LocalPath)
					   .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
					   .ToUpperInvariant();
		}

		public static string GetAssetFolderPath(bool normalized = false)
		{
			if (normalized)
				return NormalizePath(CTL_Settings.PATH_PROJECT_ROOT);
			return CTL_Settings.PATH_PROJECT_ROOT;
		}

		//get path relative to the asset folder
		public static string GetRelativePath(string path, bool normalized = true)
		{
			return path.Replace(GetAssetFolderPath(normalized), "");
		}


		public static string GetNotRelativePath(string relativePath, bool removeAssetFolder = true)
		{
			if (removeAssetFolder)
			{
				return NormalizePath(CTL_Settings.PATH_PROJECT_ROOT.Substring(0, CTL_Settings.PATH_PROJECT_ROOT.Length - "Assets".Length) + relativePath);
			}
			else
			{
				return NormalizePath(CTL_Settings.PATH_PROJECT_ROOT + relativePath);
			}
		}

		public static bool IsValidPath(string relativePath)
		{
			string fullPath = GetNotRelativePath(relativePath);
			try
			{
				FileInfo fileinfo = new FileInfo(fullPath);
				if (!fileinfo.Exists || (fileinfo.Extension.ToLower() != ".cs" && fileinfo.Extension.ToLower() != ".js"))
				{
					DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
					if (!dirInfo.Exists)
						return false;
				}
			}
			catch (Exception ex)
			{
				if (CTL_Settings.DEBUG_MODE)
				{
					Debug.LogWarning("invalid path : " + relativePath + "  error: " + ex.Message);
				}
			}

			return true;
		}

		public static Texture2D GenerateBgTexture(Color col)
		{
			Color[] pix = new Color[1];
			pix[0] = col;

			Texture2D result = new Texture2D(1, 1);
#if UNITY_EDITOR
			result.alphaIsTransparency = true;
#endif
			result.SetPixels(pix);
			result.Apply();
			return result;
		}
	}
}

