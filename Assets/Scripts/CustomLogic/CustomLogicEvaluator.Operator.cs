using Characters;

namespace CustomLogic
{
    partial class CustomLogicEvaluator
    {
        private static object AddValues(object left, object right)
        {
            if (left is int l && right is int r)
                return l + r;

            var leftStr = left is string;
            var rightStr = right is string;
            if (leftStr || rightStr)
            {
                if (leftStr)
                    return (string)left + right;

                return left + (string)right;
            }

            if (left is CustomLogicVector3Builtin lv3 && right is CustomLogicVector3Builtin rv3)
                return new CustomLogicVector3Builtin(lv3.Value + rv3.Value);
            
            return left.UnboxToFloat() + right.UnboxToFloat();
        }

        private static object SubtractValues(object left, object right)
        {
            if (left is int l && right is int r)
                return l - r;
            if (left is CustomLogicVector3Builtin lv3 && right is CustomLogicVector3Builtin rv3)
                return new CustomLogicVector3Builtin(lv3.Value - rv3.Value);
            
            return left.UnboxToFloat() - right.UnboxToFloat();
        }

        private static object MultiplyValues(object left, object right)
        {
            if (left is int l && right is int r)
                return l * r;
            if (left is CustomLogicVector3Builtin lv3)
                return new CustomLogicVector3Builtin(lv3.Value * right.UnboxToFloat());
            if (right is CustomLogicVector3Builtin rv3)
                return new CustomLogicVector3Builtin(rv3.Value * left.UnboxToFloat());
            if (left is CustomLogicQuaternionBuiltin lq && right is CustomLogicQuaternionBuiltin rq)
                return new CustomLogicQuaternionBuiltin(lq.Value * rq.Value);
            return left.UnboxToFloat() * right.UnboxToFloat();
        }

        private static object DivideValues(object left, object right)
        {
            if (left is int l && right is int r)
                return l / r;
            if (left is CustomLogicVector3Builtin lv3)
                return new CustomLogicVector3Builtin(lv3.Value / right.UnboxToFloat());
            return left.UnboxToFloat() / right.UnboxToFloat();
        }

        public static bool CheckEquals(object left, object right)
        {
            if (left == null && right == null)
                return true;
            if (left != null)
                return left.Equals(right);
            return right.Equals(left);
        }
        
        public static CustomLogicCharacterBuiltin GetCharacterBuiltin(BaseCharacter character)
        {
            return character switch
            {
                Human human => new CustomLogicHumanBuiltin(human),
                BasicTitan titan => new CustomLogicTitanBuiltin(titan),
                BaseShifter shifter => new CustomLogicShifterBuiltin(shifter),
                _ => null
            };
        }
    }
}