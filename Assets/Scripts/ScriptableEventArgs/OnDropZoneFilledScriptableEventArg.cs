public class OnDropZoneFilledScriptableEventArg : ScriptableEventArg
{
	public DropZone DropZone { get; protected set; }

	public OnDropZoneFilledScriptableEventArg(DropZone dropZone) : base()
	{
		DropZone = dropZone;
	}
}
