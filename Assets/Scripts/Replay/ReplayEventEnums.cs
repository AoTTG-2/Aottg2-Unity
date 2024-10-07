namespace Replay
{
    public enum ReplayEventCategory
    {
        Map,
        Human,
        Titan,
        Camera,
        Chat
    }

    public enum ReplayEventBaseAction
    {
        Spawn,
        Despawn,
        State
    }

    public enum ReplayEventMapAction
    {
        SetMap
    }

    public enum ReplayEventCameraAction
    {
        Follow
    }

    public enum ReplayEventChatAction
    {
        AddLine
    }
}
