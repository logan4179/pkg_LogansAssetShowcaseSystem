using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogansThirdPersonCamera;

namespace LogansAssetShowcaseSystem
{
    public class LASS_Manager : MonoBehaviour
    {
        public static LASS_Manager Instance;

        [Header("REFERENCE (EXTERNAL)")]
        [HideInInspector] public ShowcaseEntry FocusedEntry;
        [SerializeField] private ThirdPersonCamera _camera;

        //[Header("CALCULATED")]
        private Vector3 v_calculatedCameraGoal = Vector3.zero;

        private float cd_effectAlarm = 0f;

        private float cachedMouseX, cachedMouseY;
        private float dist_mouseFollow = 1f;

		private void Awake()
		{
			Instance = this;
		}

		void Start()
        {
            CheckIfKosher();
        }

        void Update()
        {
			if ( Input.GetKey(KeyCode.Mouse0) )
            {
				cachedMouseX = Input.GetAxis("Mouse X");
                cachedMouseY = Input.GetAxis("Mouse Y");
            }
            else
            {
                cachedMouseX = 0f;
                cachedMouseY = 0f;
            }

            if( Input.mouseScrollDelta.y != 0f )
            {
                dist_mouseFollow -= Input.mouseScrollDelta.y * 0.2f;

                if( dist_mouseFollow < 0f )
                {
                    dist_mouseFollow = 0.1f;
                }
                else if( dist_mouseFollow > 15f )
                {
                    dist_mouseFollow = 15f;
                }

                _camera.CachedFollowDistance = dist_mouseFollow;
            }

			if ( FocusedEntry != null && _camera != null )
            {
				_camera.UpdateCamera(
                    cachedMouseX, cachedMouseY, Time.deltaTime
                    );
            }

            if (FocusedEntry != null)
            {
                if (FocusedEntry.Category == "Visual Effects" && FocusedEntry.LoopEffects )
                {
                    cd_effectAlarm -= Time.deltaTime;

                    if (cd_effectAlarm <= 0f)
                    {
                        FocusedEntry.PlayEffects();
                        cd_effectAlarm = FocusedEntry.Duration_effect;
                    }
                }
            }
        }

        public void FocusOnEntry( ShowcaseEntry entry )
        {
			FocusedEntry = entry;
            _camera.FollowTransform = entry.FocalPoint;
            dist_mouseFollow = entry.DefaultFocalPointDistance;

			_camera.CachedFollowDistance = entry.DefaultFocalPointDistance;

            //_camera.PlaceCameraAtDefaultPositionAndOrientation();
        }

		public bool CheckIfKosher()
		{
			bool amKosher = true;

			if ( _camera == null)
			{
				amKosher = false;
				Debug.LogError($"{nameof(LASS_Manager)}.{nameof(_camera)} reference was null! ");
			}

			return amKosher;
		}
	}
}