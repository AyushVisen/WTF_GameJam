using UnityEngine;

public class BehaviourTreeProcessor : MonoBehaviour
{
	[SerializeField] protected BehaviourTree Tree;

	protected virtual void Start()
	{
		Tree = Tree.Clone();
	}

	protected virtual void Update()
	{
		Tree.Update();
	}
}
