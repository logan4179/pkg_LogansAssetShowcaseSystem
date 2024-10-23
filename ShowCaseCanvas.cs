using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace LogansAssetShowcaseSystem
{
    public class ShowCaseCanvas : MonoBehaviour
    {
        [SerializeField] private LASS_Manager _manager;

		[SerializeField] private ShowcaseEntry[] foundEntriesInScene = null;

        [SerializeField] private List<ShowcaseCategory> showcaseCategories = null;

        [Header("REFERENCE (MAIN CONTROLS)")]
        [SerializeField] private TMP_Dropdown dd_categories;
        [SerializeField] private TMP_Dropdown dd_entries;
        [SerializeField] private TextMeshProUGUI tmp_Title;
        [SerializeField] private TextMeshProUGUI tmp_description;
        [SerializeField] private Toggle tgl_loopEffects;

        [Header("REFERENCE (SUB CONTROLS)")]
        [SerializeField] private GameObject Group_Animation;
        [SerializeField] private TMP_Dropdown dd_Parameter;

		void Start()
        {
            CheckIfKosher();

            tmp_Title.text = string.Empty;
            tmp_description.text = string.Empty;

            for ( int i = 0; i < showcaseCategories.Count; i++ )
            {
                if ( i == 0 )
                {
                    showcaseCategories[i].ParentGameObject.SetActive( true);
                }
                else
                {
					showcaseCategories[i].ParentGameObject.SetActive(false);
				}
			}

			//dd_entries.value = 0;
			UI_DD_Categories_action();
		}



        [ContextMenu("z call FetchAll()")]
        public void FetchAll()
        {
            showcaseCategories = new List<ShowcaseCategory>();

            foundEntriesInScene = _manager.GetComponentsInChildren<ShowcaseEntry>();
            print($"found '{foundEntriesInScene.Length}' total entries in scene. current categories amount: '{showcaseCategories.Count}'");

            dd_entries.options = new List<TMP_Dropdown.OptionData>();

			foreach ( ShowcaseEntry entry in foundEntriesInScene )
            {
                ShowcaseCategory existingCatData = FindShowcaseCategoryDataByCategory( entry.Category );

                if( existingCatData == null )
                {
                    print($"found new category; '{entry.Category}'");
                    showcaseCategories.Add( new ShowcaseCategory( entry ) );
				}
                else
                {
                    existingCatData.AddEntry( entry ); //this will do nothing if the entry already exists
                }
			}

			dd_categories.options = new List<TMP_Dropdown.OptionData>();
			dd_categories.captionText.text = "Categories";
			foreach ( ShowcaseCategory categoryData in showcaseCategories )
            {
				dd_categories.options.Add( new TMP_Dropdown.OptionData(categoryData.Category) );
			}
		}

        /// <summary>
        /// Returns a ShowCaseCategoryData object based on a supplied category string.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private ShowcaseCategory FindShowcaseCategoryDataByCategory( string category )
        {
            foreach ( ShowcaseCategory data in showcaseCategories )
            {
                if( data.Category == category )
                {
                    return data;
                }
            }

            return null;
        }

		#region UI METHODS----------------------------------------------
		public void UI_DD_Categories_action()
        {
            print($"{nameof(UI_DD_Categories_action)}, {nameof(dd_categories)}.value: '{dd_categories.value}'. Count: '{dd_categories.options.Count}'");

			for (int i = 0; i < showcaseCategories.Count; i++)
			{
				if (i == dd_categories.value)
				{
					showcaseCategories[i].ParentGameObject.SetActive(true);
				}
				else
				{
					showcaseCategories[i].ParentGameObject.SetActive(false);
				}
			}

			dd_entries.options = 
                showcaseCategories[dd_categories.value].CachedDropdownOptionData_entries;

            dd_entries.value = 0; //adding this was necessary to prevent an error thrown when the entries drop down action calls focusonentry()
            UI_DD_Entries_action();
        }

        public void UI_DD_Entries_action()
        {
			print($"{nameof(UI_DD_Entries_action)}. {nameof(dd_entries)}.value: '{dd_entries.value}'. count: '{dd_entries.options.Count}'");

			LASS_Manager.Instance.FocusOnEntry(
                showcaseCategories[dd_categories.value].Entries[dd_entries.value]
                );

            LASS_Manager.Instance.FocusedEntry.SupplyCanvasWithMyInfo(
                tmp_Title, tmp_description, dd_Parameter
                );

            if( LASS_Manager.Instance.FocusedEntry._Animator != null )
            {
                Group_Animation.SetActive( true );
            }
            else
            {
				Group_Animation.SetActive( false );
			}
		}

        public void UI_Btn_playEffects_action()
        {
            LASS_Manager.Instance.FocusedEntry.PlayEffects();
        }

        public void UI_Toggle_loopEffects_action()
        {
            LASS_Manager.Instance.FocusedEntry.LoopEffects = tgl_loopEffects.isOn;
        }

        public void UI_dd_animatorParameters_action()
        {
            LASS_Manager.Instance.FocusedEntry.PlayAnimation(dd_Parameter.value);
        }

		#endregion

		public bool CheckIfKosher()
        {
            bool amKosher = true;

            if( dd_categories == null )
            {
                amKosher = false;
                Debug.LogError($"{nameof(dd_categories)} reference was null!");
            }

			if ( dd_entries == null )
			{
				amKosher = false;
				Debug.LogError($"{nameof(dd_entries)} reference was null!");
			}

			if ( tmp_Title == null )
			{
				amKosher = false;
				Debug.LogError($"{nameof(tmp_Title)} reference was null!");
			}

			if ( tmp_description == null )
			{
				amKosher = false;
				Debug.LogError($"{nameof(tmp_description)} reference was null!");
			}

			if ( tgl_loopEffects == null )
			{
				amKosher = false;
				Debug.LogError($"{nameof(tgl_loopEffects)} reference was null!");
			}

			return amKosher;
        }
	}
}