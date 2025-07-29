namespace CustomLogic.Editor.Models
{
    abstract class BaseModel
    {
        public string ObsoleteMessage { get; set; }
        public bool IsObsolete => string.IsNullOrEmpty(ObsoleteMessage) == false;
    }
}
