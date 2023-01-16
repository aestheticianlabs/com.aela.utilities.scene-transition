using UnityEngine;
using UnityEngine.Events;

namespace AeLa.Utilities.SceneTransition
{
	public class DoOnSceneTransitionComplete : MonoBehaviour
	{
		public UnityEvent OnAwake;
		public UnityEvent OnSceneTransitionComplete;
		
		private void Awake()
		{
			OnAwake?.Invoke();
			SceneTransitionManager.OnSceneTransitionComplete += OnEvent;
		}

		private void OnDestroy()
		{
			SceneTransitionManager.OnSceneTransitionComplete -= OnEvent;
		}

		private void OnEvent(string scene)
		{
			OnSceneTransitionComplete?.Invoke();
		}
	}
}