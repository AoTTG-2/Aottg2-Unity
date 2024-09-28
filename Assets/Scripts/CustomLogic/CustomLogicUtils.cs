using System;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    static class CustomLogicUtils
    {
        public static string SerializeAst(CustomLogicBaseAst ast)
        {
            if (ast is CustomLogicPrimitiveExpressionAst primitiveAst)
            {
                if (primitiveAst.Value is float or int)
                    return primitiveAst.ToString();
                else if (primitiveAst.Value is bool b)
                    return b ? "true" : "false";
                else if (primitiveAst.Value is string s)
                    return s.Replace(',', ' ').Replace(':', ' ').Replace('|', ' ');
            }
            else if (ast is CustomLogicClassInstantiateExpressionAst classAst)
            {
                var paramCount = classAst.Parameters.Count;

                if (classAst.Name == "Color")
                {
                    Color hexColor = Color.white;
                    if (paramCount == 1)
                        ColorUtility.TryParseHtmlString(((CustomLogicPrimitiveExpressionAst)classAst.Parameters[0]).Value.ToString(), out hexColor);

                    Color255 value = paramCount switch
                    {
                        1 => new Color255(hexColor),
                        3 => new Color255(BaseAstToInt(classAst.Parameters[0]), BaseAstToInt(classAst.Parameters[1]), BaseAstToInt(classAst.Parameters[2])),
                        4 => new Color255(BaseAstToInt(classAst.Parameters[0]), BaseAstToInt(classAst.Parameters[1]), BaseAstToInt(classAst.Parameters[2]), BaseAstToInt(classAst.Parameters[3])),
                        _ => new Color255()
                    };

                    return string.Join("/", new string[] { value.R.ToString(), value.G.ToString(), value.B.ToString(), value.A.ToString() });
                }
                else if (classAst.Name == "Vector3")
                {
                    Vector3 value = paramCount switch
                    {
                        1 => new Vector3(BaseAstToFloat(classAst.Parameters[0]), BaseAstToFloat(classAst.Parameters[0]), BaseAstToFloat(classAst.Parameters[0])),
                        2 => new Vector3(BaseAstToFloat(classAst.Parameters[0]), BaseAstToFloat(classAst.Parameters[1]), 0),
                        3 => new Vector3(BaseAstToFloat(classAst.Parameters[0]), BaseAstToFloat(classAst.Parameters[1]), BaseAstToFloat(classAst.Parameters[2])),
                        _ => Vector3.zero
                    };

                    return string.Join("/", new string[] { value.x.ToString(), value.y.ToString(), value.z.ToString() });
                }
            }
            return string.Empty;
        }

        public static float BaseAstToFloat(CustomLogicBaseAst ast)
        {
            if (ast is CustomLogicPrimitiveExpressionAst primitiveAst)
                return Convert.ToSingle(primitiveAst.Value);

            return 0;
        }

        public static int BaseAstToInt(CustomLogicBaseAst ast)
        {
            if (ast is CustomLogicPrimitiveExpressionAst primitiveAst)
                return Convert.ToInt32(primitiveAst.Value);

            return 0;
        }
    }
}