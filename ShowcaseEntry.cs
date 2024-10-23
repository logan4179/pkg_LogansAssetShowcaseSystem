using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEditor;
using UnityEngine;
using LogansMultiEffectHandler;

namespace LogansAssetShowcaseSystem
{
    public class ShowcaseEntry : MonoBehaviour
    {
        public string Title;

        public string Description;

        public string Category;

        public Transform FocalPoint;

        public float DefaultFocalPointDistance = 3f;

        [SerializeField] private MultiEffectHandler[] effectHandlers;
        public bool LoopEffects = true;
        public float Duration_effect = 1f;

        [Header("ANIMATION")]
        public Animator _Animator;
        private AnimatorControllerParameter[] _animatorParameters;
		private List<TMP_Dropdown.OptionData> CachedDropdownOptionData_parameters;

		//[Header("DEBUG")]
        //[SerializeField] private bool drawingGizmos = false;

		private void Start()
		{
			CheckIfKosher();

            if ( Category == "Visual Effects" )
            {
                effectHandlers = GetComponentsInChildren<MultiEffectHandler>();
            }

            if ( _Animator != null )
            {
                _animatorParameters = _Animator.parameters;
				//print($"got '{_animatorParameters.Length}' parameters...");

				CachedDropdownOptionData_parameters = new List<TMP_Dropdown.OptionData>();
				foreach ( AnimatorControllerParameter prmtr in _animatorParameters )
				{
                    //print($"'{prmtr.name}', '{prmtr.type}'");
                    CachedDropdownOptionData_parameters.Add( new TMP_Dropdown.OptionData(prmtr.name) );
				}


				/*
                AnimatorClipInfo[] infos = _Animator.GetCurrentAnimatorClipInfo(0);
                print($"got '{infos.Length}' clipinfos...");
                AnimatorStateInfo stateInfo = _Animator.GetCurrentAnimatorStateInfo(0);
                
                foreach ( AnimatorClipInfo info in infos )
                {
                    print($"'{info.clip.name}', clips: '{info.}'");
                }
                */
			}
		}

		/*
        [ContextMenu("z call SaveCameraOffset()")]
        public void SaveCameraOffset()
        {
            CameraPosition = SceneView.GetAllSceneCameras()[0].transform.position;
            CameraRotation = SceneView.GetAllSceneCameras()[0].transform.rotation;
            print($"{nameof(CameraPosition)} set to: '{CameraPosition}'.");
        }
        */

		/*
		private void OnDrawGizmos()
		{
            if( !drawingGizmos )
            {
                return;
            }
		}
        */

        public void SupplyCanvasWithMyInfo( 
            TextMeshProUGUI tmp_title, TextMeshProUGUI tmp_description, TMP_Dropdown dd_animParams )
        {
            tmp_title.text = Title;
            tmp_description.text = Description;

            if( _Animator != null )
            {
				dd_animParams.options = CachedDropdownOptionData_parameters;
			}
        }

        public void PlayEffects()
        {
            foreach ( MultiEffectHandler handler in effectHandlers )
            {
                handler.PlayAll();
            }
        }

        public void PlayAnimation( int indx )
        {
            if ( _animatorParameters[indx].type == AnimatorControllerParameterType.Float )
            {
                //_Animator.SetFloat(_animatorParameters[indx].name) //todo
            }
			else if ( _animatorParameters[indx].type == AnimatorControllerParameterType.Int )
			{
				//_Animator.SetFloat(_animatorParameters[indx].name) //todo
			}
			else if ( _animatorParameters[indx].type == AnimatorControllerParameterType.Bool )
			{
                _Animator.SetBool( _animatorParameters[indx].name, true );
			}
			else if ( _animatorParameters[indx].type == AnimatorControllerParameterType.Trigger )
			{
				_Animator.SetTrigger( _animatorParameters[indx].name );
			}
		}

		public bool CheckIfKosher()
        {
            bool amKosher = true;

            if( FocalPoint == null )
            {
                amKosher = false;
                Debug.LogError($"{nameof(FocalPoint)} reference was null for {nameof(ShowcaseEntry)}: '{name}'!");
            }

            return amKosher;
        }
	}
}
