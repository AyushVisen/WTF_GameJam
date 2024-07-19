using UnityEngine;

public class BehaviourTreeProcessor : MonoBehaviour
{
	[SerializeField] private BehaviourTree tree;

	void Start()
	{
		tree = tree.Clone();
	}
	void Update()
	{
		tree.Update();
	}
}
