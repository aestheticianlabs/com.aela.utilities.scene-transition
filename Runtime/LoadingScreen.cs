using System.Collections; using AeLa.Utilities.UI;
using UnityEngine;

#if UNITY_VIDEO
using UnityEngine.Video;
#endif

namespace AeLa.Utilities.SceneTransition
{
	[DefaultExecutionOrder(-100)]
	[RequireComponent(typeof(UIFader))]
	public class LoadingScreen : MonoBehaviour
	{
		public static LoadingScreen Instance { get; private set; }

#if UNITY_VIDEO
		[SerializeField] private VideoPlayer loadingVideo;
#endif

		/// <summary>
		/// Minimum time to spend on the loading screen
		/// </summary>
		[Tooltip("Minimum time to spend on the loading screen")] [SerializeField]
		private float minLoadingTime = 3f;

		private UIFader fader;

		private float loadStartTime;

		private void Awake()
		{
			if (Instance && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(this);

			fader = GetComponent<UIFader>();
#if UNITY_VIDEO
			loadingVideo.gameObject.SetActive(false);
#endif

			SceneTransitionManager.OnBeforeTransition += OnBeforeTransition;
			SceneTransitionManager.OnLoadProgress += OnLoadProgress;
			SceneTransitionManager.OnAfterUnload += OnAfterUnload;
			SceneTransitionManager.OnSceneTransitionComplete += OnSceneTransitionComplete;
		}

		private void OnBeforeTransition(string fromPath, string toPath)
		{
			var op = SceneTransitionManager.Instance.BlockingOperations.StartOperation();
			CoroutineUtils.Global.DoAfter(
				fader.FadeInCoroutine(), () =>
				{
					loadStartTime = Time.unscaledTime;
					op.Progress = 1f;
#if UNITY_VIDEO
					loadingVideo.gameObject.SetActive(true);
					loadingVideo.Play();
#endif
				}
			);
		}

		private void OnLoadProgress(float progress)
		{
			DebugFiltered.Log(SceneTransitionManager.Instance.LogLevel, $"Load progress {progress}");
		}

		private void OnAfterUnload(string obj)
		{
			
		}

		private IEnumerator WrapUpRoutine()
		{		
			// ensure we hit min loading time
			while (Time.unscaledTime - loadStartTime < minLoadingTime)
			{
				yield return null;
			}

#if UNITY_VIDEO
			// let loading video finish
			var loopPointReached = false;
			void WaitForLoopPoint(VideoPlayer source) => loopPointReached = true;
			loadingVideo.loopPointReached += WaitForLoopPoint;
			while (!loopPointReached) yield return null;
			loadingVideo.loopPointReached -= WaitForLoopPoint;

			// don't fade everything out until timescale has resumed, otherwise
			// we hang after the video is hidden but before the fader is faded
			while (Time.timeScale == 0) yield return null;

			// stop and hide video
			loadingVideo.Stop();
			loadingVideo.gameObject.SetActive(false);
#endif

			yield return fader.FadeOutCoroutine();
		}

		private void OnSceneTransitionComplete(string currentPath)
		{
			var op = SceneTransitionManager.Instance.BlockingOperations.StartOperation();

			CoroutineUtils.Global.DoAfter(StartCoroutine(WrapUpRoutine()), () => op.Progress = 1f);
		}
	}
}