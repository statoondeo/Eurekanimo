public class OnTokenDroppedScriptableEventArg : ScriptableEventArg
{
	public DragNDroppable Token { get; protected set; }
	public DropZone DropZone { get; protected set; }

	public OnTokenDroppedScriptableEventArg(DragNDroppable token, DropZone container) : base()
	{
		Token = token;
		DropZone = container;
	}
}
