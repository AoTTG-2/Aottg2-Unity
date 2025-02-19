using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor
{
    /// <summary>
    /// A command that stores the prior and post values of a generic field.
    /// Currently inspector fields are not reversible and synchronization requires looping over everything.
    /// We need to fix this.
    /// </summary>
    class InspectorEditFieldCommand<T> : InspectorCommand
    {
        private int _id;
        private FieldInfo _field;
        private T _oldValue;
        private T _newValue;

        public InspectorEditFieldCommand(MapObject obj, FieldInfo field, T oldValue, T newValue)
        {
            _id = obj.ScriptObject.Id;
            _field = field;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override void Execute()
        {
            var obj = MapLoader.IdToMapObject[_id];
            _field.SetValue(obj.ScriptObject, _newValue);

        }

        public override void Unexecute()
        {
            var obj = MapLoader.IdToMapObject[_id];
            _field.SetValue(obj.ScriptObject, _oldValue);
        }
    }
}
