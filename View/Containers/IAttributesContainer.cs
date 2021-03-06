﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Exolutio.Model.PIM;

namespace Exolutio.View
{
    /// <summary>
    /// Interface for control displaying attributes. 
    /// </summary>
    public interface IAttributesContainer<TMember, TTextBox> : ITextBoxContainer, IEnumerable<TTextBox>
    {
        /// <summary>
        /// Visualized collection 
        /// </summary>
        ICollection<TMember> AttributesCollection
        {
            get;
        }

        /// <summary>
        /// Adds visualization of <paramref name="member"/> to the control
        /// </summary>
        /// <param name="member">visualized attribute</param>
        /// <returns>Control displaying the attribute</returns>
        TTextBox AddMember(TMember member);

        /// <summary>
        /// Removes visualization of <paramref name="member"/>/
        /// </summary>
        /// <param name="member">removed attribute</param>
        void RemoveMember(TMember member);

        /// <summary>
        /// Reflects changs in <see cref="AttributesCollection"/>.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event arguments</param>
        void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e);

        /// <summary>
        /// Removes all attriutes
        /// </summary>
        void Clear();

        /// <summary>
        /// Cancels editing if any of the displayed attributes is being edited. 
        /// </summary>
        void CancelEdit();
    }
}