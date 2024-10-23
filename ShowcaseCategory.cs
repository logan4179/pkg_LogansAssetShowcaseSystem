using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LogansAssetShowcaseSystem
{
    [System.Serializable]
    public class ShowcaseCategory
    {
        public string Category;

        public List<ShowcaseEntry> Entries;

        public List<TMP_Dropdown.OptionData> CachedDropdownOptionData_entries;

        public GameObject ParentGameObject;

        public ShowcaseCategory( ShowcaseEntry entry )
        {
            ParentGameObject = entry.transform.parent.gameObject;

            Category = entry.Category;

            Entries = new List<ShowcaseEntry> { entry };

            CachedDropdownOptionData_entries = 
                new List<TMP_Dropdown.OptionData>() 
                { new TMP_Dropdown.OptionData(entry.Title) };
        }

        /// <summary>
        /// Adds an entry to this category's entry list and adds the corresponding entry name to the 
        /// cached dropdown data collection. Does nothing if this entry has already been added to this category.
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry( ShowcaseEntry entry )
        {
            if ( !Entries.Contains(entry) )
            {
                Entries.Add( entry );
            }

            int spot = FindEntryInDropDownData( entry );

            if ( spot == -1 )
            {
                CachedDropdownOptionData_entries.Add( new TMP_Dropdown.OptionData(entry.Title) );
            }
        }
        
        private int FindEntryInDropDownData( ShowcaseEntry entry )
        {
            if ( CachedDropdownOptionData_entries != null && CachedDropdownOptionData_entries.Count > 0 )
            {
                for ( int i = 0; i < CachedDropdownOptionData_entries.Count; ++i )
                {
                    if( entry.Title == CachedDropdownOptionData_entries[i].text )
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public void PopulateDropdown( TMP_Dropdown dropdown )
        {
            dropdown.options = CachedDropdownOptionData_entries;
        }
        
    }
}