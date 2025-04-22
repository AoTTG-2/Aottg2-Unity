using Map;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor
{
    class SetParentCommand : InspectorCommand
    {
        private List<int> _previousParents = new List<int>();
        private List<int> _previousSiblingIndices = new List<int>();
        private int _newParent;
        private int? _newSiblingIndex;
        private List<int> _ids = new List<int>();

        public SetParentCommand(List<MapObject> objs, int newParent, int? newSiblingIndex)
        {
            // Assert.AreEqual(objs.Count, 1); // TODO: for now only allow single element moving.
            _newParent = newParent;
            _newSiblingIndex = newSiblingIndex;
            foreach (MapObject obj in objs)
            {
                _ids.Add(obj.ScriptObject.Id);
                _previousParents.Add(obj.Parent);
                _previousSiblingIndices.Add(obj.SiblingIndex);
            }
        }

        public override void Execute()
        {
            foreach (int id in _ids)
                MapLoader.EditorOnMoveObject(id, _newParent, _newSiblingIndex);
        }

        public override void Unexecute()
        {
            //Assert.AreEqual(_ids.Count, _previousParents.Count);
            //Assert.AreEqual(_ids.Count, _previousSiblingIndices.Count);

            for (int i = 0; i < _ids.Count; i++)
                MapLoader.EditorOnMoveObject(_ids[i], _previousParents[i], _previousSiblingIndices[i]);
        }
    }
}
