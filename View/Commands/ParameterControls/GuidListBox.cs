using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using EvoX.Controller.Commands.Reflection;
using EvoX.Model;
using EvoX.Model.PIM;
using EvoX.Model.PSM;
using EvoX.SupportingClasses;

namespace EvoX.View.Commands.ParameterControls
{
    public class GuidListBox : ListBox,
        IOperationParameterControl, IOperationParameterControl<Guid>, IOperationParameterControl<EvoXObject>,
        IOperationParameterControlWithConsistencyCheck
    {
        public delegate bool VerifyDelegate(params object[] verifiedObjects);

        public bool AllowNullInput { get; set; }

        public Type LookedUpType { get; set; }

        public ProjectVersion ProjectVersion { get; set; }

        public void InitControl()
        {
            Items.Clear();
        }

        protected object SuggestedValue { get; set; }

        public void SetSuggestedValue(object suggestedValue)
        {
            this.SuggestedValue = suggestedValue;
        }

        object IOperationParameterControl.Value
        {
            get { return Value; }
        }

        public EvoXObject Value
        {
            get
            {
                Guid guid = (this as IOperationParameterControl<Guid>).Value;
                if (guid != Guid.Empty)
                    return ProjectVersion.Project.TranslateComponent<EvoXObject>(guid);
                else
                    return null;
            }
        }

        Guid IOperationParameterControl<Guid>.Value
        {
            get
            {
                if (this.SelectedIndex != -1)
                    return Guid.Parse(this.SelectedValue.ToString());
                else
                    return Guid.Empty;
            }
        }

        public List<Guid> Values
        {
            get 
            { 
                List<Guid> result = new List<Guid>();
                foreach (ListBoxItem selectedItem in this.SelectedItems)
                {
                    string strValue = selectedItem.Tag.ToString();
                    result.Add(Guid.Parse(strValue));
                }

                return result;
            }
        }

        public void LoadAllPossibleValues(Type componentType)
        {
            if (componentType == typeof (PSMSchema))
            {
                Items.Clear();
                foreach (PSMSchema psmSchema in ProjectVersion.PSMSchemas)
                {
                    ListBoxItem listItem = new ListBoxItem();
                    listItem.Content = psmSchema.ToString();
                    listItem.Tag = psmSchema.ID.ToString();
                    Items.Add(listItem);
                    SelectedIndex = 0;
                }
            }
            else if (componentType == typeof (PIMSchema))
            {
                ListBoxItem listItem = new ListBoxItem();
                listItem.Content = ProjectVersion.PIMSchema.ToString();
                listItem.Tag = ProjectVersion.PIMSchema.ID.ToString();
                Items.Add(listItem);
                SelectedIndex = 0;
            }
            else if (componentType == typeof(Schema))
            {
                Items.Clear();
                ListBoxItem listItem = new ListBoxItem();
                listItem.Content = ProjectVersion.PIMSchema.ToString();
                listItem.Tag = ProjectVersion.PIMSchema.ID.ToString();
                Items.Add(listItem);
                SelectedIndex = 0;
                foreach (PSMSchema psmSchema in ProjectVersion.PSMSchemas)
                {
                    ListBoxItem psmListBoxItem = new ListBoxItem();
                    psmListBoxItem.Content = psmSchema.ToString();
                    psmListBoxItem.Tag = psmSchema.ID.ToString();
                    Items.Add(psmListBoxItem);
                    SelectedIndex = 0;
                }
            }
            else
            {
                throw new NotImplementedException(string.Format("Member GuidLookup.LoadAllPossibleValues not implemented for type {0}.", componentType.Name));
            }

            if (AllowNullInput)
            {
                Items.Insert(0, new ListBoxItem {Content = "(null)", Tag = Guid.Empty.ToString()});
                SelectedIndex = 0;
            }

            if (SuggestedValue != null && SuggestedValue is EvoXObject)
            {
                this.SelectedItem = ((IEnumerable)this.Items).FirstOrDefault(i => ((ListBoxItem)i).Tag.ToString() == ((EvoXObject)SuggestedValue).ID.ToString());
            }
        }

        public void MakeValuesConsistentWith(object superiorObject)
        {
            Items.Clear();

            if (superiorObject != null)
            {
                Schema schema;
                Guid superiorObjectGuid = Guid.Empty;
                if (typeof (PIMComponent).IsAssignableFrom(LookedUpType) && (superiorObject is PSMComponent))
                {
                    schema = ((PSMComponent) superiorObject).ProjectVersion.PIMSchema;
                    superiorObjectGuid = ((PSMComponent) superiorObject).ID;
                }
                else if (superiorObject is Schema)
                {
                    schema = (Schema)superiorObject;
                    superiorObjectGuid = ((Schema)superiorObject).ID;
                }
                else if (superiorObject is Component)
                {
                    schema = ((Component) superiorObject).Schema;
                    superiorObjectGuid = ((Component)superiorObject).ID;
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (LookedUpType == typeof(AttributeType))
                {
                    foreach (AttributeType schemaComponent in schema.ProjectVersion.AttributeTypes)
                    {
                        if (ConsistencyChecker == null || superiorObjectGuid == Guid.Empty ||
                            ConsistencyChecker.VerifyConsistency(superiorObject, schemaComponent))
                        {
                            ListBoxItem listItem = new ListBoxItem();
                            listItem.Content = schemaComponent.ToString();
                            listItem.Tag = schemaComponent.ID.ToString();
                            Items.Add(listItem);
                        }
                    }
                }
                else
                {
                    foreach (Component schemaComponent in schema.SchemaComponents)
                    {
                        //if (schemaComponent.GetType() == LookedUpType)
                        if (LookedUpType.IsAssignableFrom(schemaComponent.GetType()))
                        {
                            if (ConsistencyChecker == null ||
                                ConsistencyChecker.VerifyConsistency(superiorObject, schemaComponent))
                            {
                                ListBoxItem listItem = new ListBoxItem();
                                listItem.Content = schemaComponent.ToString();
                                listItem.Tag = schemaComponent.ID.ToString();
                                Items.Add(listItem);
                            }
                        }
                    }
                }
            }

            bool select1 = Items.Count > 0;

            if (AllowNullInput)
            {
                Items.Insert(0, new ListBoxItem { Content = "(null)", Tag = Guid.Empty.ToString() });
                SelectedIndex = 0;
            }

            if (Items.Count > 0)
            {
                if (Items.Count > 1 && select1)
                {
                    SelectedIndex = select1 ? 1 : 0;
                }
                else
                {
                    SelectedIndex = 0;
                }
                
                //OnSelectionChanged(null);
            }

            if (SuggestedValue != null && SuggestedValue is EvoXObject)
            {
                this.SelectedItem = this.Items.FirstOrDefault(i => ((ListBoxItem)i).Tag.ToString() == ((EvoXObject)SuggestedValue).ID.ToString());
            }
            //OnSelectionChanged(null);
        }

        public IOperationParameterControl SuperiorPropertyEditor { get; set; }

        public ParameterConsistency ConsistencyChecker { get; set; }
        public void OnSuperiorPropertyChanged(object sender, EventArgs e)
        {
            this.MakeValuesConsistentWith(SuperiorPropertyEditor.Value);
        }
    }
}