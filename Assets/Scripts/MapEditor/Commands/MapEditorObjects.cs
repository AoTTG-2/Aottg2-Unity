using System.Collections.Generic;
using Map;
using Utility;


public class MapEditorObjects : IQueryable
{
    private Dictionary<int, MapObject> _objects = new Dictionary<int, MapObject>();

    public MapEditorObjects()
    {
        Sync();
    }

    public void Sync()
    {
        _objects.Clear(); // Clear the dictionary to avoid duplicates
        foreach (MapObject obj in MapLoader.IdToMapObject.Values)
        {
            int objectID = obj.ScriptObject.Id;
            _objects.Add(objectID, obj);
        }
    }
    
    public Dictionary<int, string> Query(string query)
    {


        query = query.ToLower();
        var results = new Dictionary<int, string>();

        foreach (var obj in _objects.Values)
        {
            if (obj.ScriptObject.Name.ToLower().Contains(query))
                results.Add(obj.ScriptObject.Id, $"{obj.ScriptObject.Name} (ID: {obj.ScriptObject.Id})");
        }

        return results;
    }

    public Dictionary<int, string> GetAll()
    {
        var all = new Dictionary<int, string>();
        foreach (var obj in _objects.Values)
            all.Add(obj.ScriptObject.Id, $"{obj.ScriptObject.Name} (ID: {obj.ScriptObject.Id})");
        return all;
    }
}
