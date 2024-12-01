using System.Collections.Generic;
using Map;
using Utility;


public class MapEditorObjects : IQueryable
{
    private Dictionary<int ,MapObject> _objects = new Dictionary<int, MapObject>();

    public MapEditorObjects()
    {
        foreach (MapObject obj in MapLoader.IdToMapObject.Values)
        {
            int objectID = obj.ScriptObject.Id;
           _objects.Add(objectID, obj);
        }
    }

    public List<string> Query(string query)
    {
        query = query.ToLower();
        var results = new List<string>();

        foreach (var obj in _objects.Values)
        {
            if (obj.ScriptObject.Name.ToLower().Contains(query))
                results.Add($"{obj.ScriptObject.Name} (ID: {obj.ScriptObject.Id})");
        }

        return results;
    }

    public List<string> GetAll()
    {
        var all = new List<string>();
        foreach (var obj in _objects.Values)
            all.Add($"{obj.ScriptObject.Name} (ID: {obj.ScriptObject.Id})");
        return all;
    }
}
