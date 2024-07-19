using UnityEngine;

public abstract class Node : ScriptableObject
{
	private NodeStatus currentStatus = NodeStatus.Processing;
	private bool hasStarted = false;
	public string guid;
	[HideInInspector]
	public Vector2 position;

	protected abstract void OnStart();
	protected abstract NodeStatus OnUpdate();
	protected abstract void OnStop();

	public NodeStatus Update()
	{
		if (!hasStarted)
		{
			OnStart();
			hasStarted = true;
		}

		currentStatus = OnUpdate();

		if (currentStatus is NodeStatus.Succeeded or NodeStatus.Failed)
		{
			OnStop();
			hasStarted = false;
		}

		return currentStatus;
	}

	public virtual Node Clone()
	{
		return Instantiate( this );
	}
}

public enum NodeStatus
{
	Processing,
	Succeeded,
	Failed
}
