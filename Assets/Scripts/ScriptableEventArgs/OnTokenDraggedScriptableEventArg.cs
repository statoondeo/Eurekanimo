public class OnTokenDraggedScriptableEventArg : ScriptableEventArg
{
	public DragNDroppable Token { get; protected set; }

	public OnTokenDraggedScriptableEventArg(DragNDroppable token) : base()
	{
		Token = token;
	}
}
