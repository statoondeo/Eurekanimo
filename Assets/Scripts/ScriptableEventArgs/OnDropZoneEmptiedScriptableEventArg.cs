public class OnDropZoneEmptiedScriptableEventArg : ScriptableEventArg
{
	public DropZone DropZone { get; protected set; }

	public OnDropZoneEmptiedScriptableEventArg(DropZone dropZone) : base()
	{
		DropZone = dropZone;
	}
}
