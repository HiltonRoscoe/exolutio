using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using EvoX.Controller.Commands.Reflection;
using EvoX.Model;
using EvoX.Model.PIM;
using EvoX.Model.PSM;
using EvoX.SupportingClasses;
using EvoX.SupportingClasses.Reflection;

namespace EvoX.Web.OperationParameters
{
    public static class OperationParametersControlCreator
    {
        private struct InputHierarchyStruct
        {
            public Type SuperiorType { get; private set; }
            public string Label { get; private set; }
            public ParameterConsistency Consistency { get; private set; }

            public InputHierarchyStruct(Type superiorType, string label, string consistencyTypeKey) : this()
            {
                SuperiorType = superiorType;
                Label = label;
                Consistency = ConsistentWithAttribute.GetConsistencyInstance(consistencyTypeKey);
            }
        }

        private static Dictionary<Type, InputHierarchyStruct> inputHierarchy;

        private static void DefineInputHierarchy()
        {
            inputHierarchy = new Dictionary<Type, InputHierarchyStruct>
                                 {
                                     { typeof(Component), new InputHierarchyStruct(typeof(Schema), "Schema", SchemaComponentParameterConsistency.Key) },
                                     { typeof(AttributeType), new InputHierarchyStruct(typeof(Schema), "Schema", SchemaAttributeTypeParameterConsistency.Key) },
                                     { typeof(IHasCardinality), new InputHierarchyStruct(typeof(Schema), "Schema", SchemaComponentParameterConsistency.Key) },
                                     // PIM
                                     { typeof(PIMAttribute), new InputHierarchyStruct(typeof(PIMClass), "Class", PIMClassAttributeParameterConsistency.Key) },
                                     { typeof(PIMClass), new InputHierarchyStruct(typeof(PIMSchema), "Schema", PIMSchemaComponentParameterConsistency.Key) },
                                     { typeof(PIMAssociation), new InputHierarchyStruct(typeof(PIMSchema), "Schema", PIMSchemaComponentParameterConsistency.Key) },
                                     { typeof(PIMAssociationEnd), new InputHierarchyStruct(typeof(PIMAssociation), "Association", PIMAssociationAssociationEndParameterConsistency.Key) },
                                     { typeof(PIMComponent), new InputHierarchyStruct(typeof(PIMSchema), "Schema", PIMSchemaComponentParameterConsistency.Key) },
                                     // PSM
                                     { typeof(PSMAttribute), new InputHierarchyStruct(typeof(PSMClass) , "PSM class", PSMClassAttributeParameterConsistency.Key) },
                                     { typeof(PSMAssociation), new InputHierarchyStruct(typeof(PSMSchema), "PSM schema", PSMSchemaComponentParameterConsistency.Key) },
                                     { typeof(PSMAssociationMember), new InputHierarchyStruct(typeof(PSMSchema), "PSM schema", PSMSchemaComponentParameterConsistency.Key) },
                                     { typeof(PSMClass), new InputHierarchyStruct(typeof(PSMSchema), "PSM schema", PSMSchemaComponentParameterConsistency.Key) },
                                     { typeof(PSMComponent), new InputHierarchyStruct(typeof(PSMSchema), "PSM schema", PSMSchemaComponentParameterConsistency.Key) },
                                     { typeof(PSMContentModel), new InputHierarchyStruct(typeof(PSMSchema), "PSM schema", PSMSchemaComponentParameterConsistency.Key) },
                                     { typeof(PSMSchemaClass), new InputHierarchyStruct(typeof(PSMSchema), "PSM schema", PSMSchemaComponentParameterConsistency.Key) },
                                     { typeof(IPSMSemanticComponent), new InputHierarchyStruct(typeof(PSMSchema), "PSM schema", PSMSchemaComponentParameterConsistency.Key) }
                                 };
            //labelsForInputHierarchy = new Dictionary<Type, string>()
            //                    {
            //                         // PIM
            //                         { typeof(PIMAttribute), "Attribute"},
            //                         { typeof(PIMClass), "Class" },
            //                         { typeof(PIMAssociation), "Association" },
            //                         { typeof(PIMAssociationEnd), "Association end" },
            //                         { typeof(PIMComponent), "PIM component" },
            //                         // PSM
            //                         { typeof(PSMAttribute), "PSM attribute" },
            //                         { typeof(PSMAssociation), "PSM association" },
            //                         { typeof(PSMAssociationMember), "PSM association member" },
            //                         { typeof(PSMClass), "PSM class" },
            //                         { typeof(PSMComponent), "PSM component" },
            //                         { typeof(PSMContentModel), "PSM content model" },
            //                         { typeof(PSMSchemaClass), "PSM schema class" },
            //                     };
        }

        private static Dictionary<Type, InputHierarchyStruct> InputHierarchy
        {
            get
            {
                if (inputHierarchy == null)
                {
                    DefineInputHierarchy();
                }
                return inputHierarchy;
            }

        }

        public static List<Control> CreateControls(CommandDescriptor parametersDescriptors, ProjectVersion projectVersion)
        {
            Dictionary<PropertyInfo, List<Control>> createdControls = new Dictionary<PropertyInfo, List<Control>>();

            Dictionary<string, Control> propertyEditors = new Dictionary<string, Control>();

            for (int index = 0; index < parametersDescriptors.Parameters.Count; index++)
            {
                ParameterDescriptor parameter = parametersDescriptors.Parameters[index];
                PropertyInfo parameterProperty = parameter.ParameterPropertyInfo;
                List<Control> controlsForProperty = createdControls.CreateSubCollectionIfNeeded(parameterProperty);

                Label label = new Label();
                label.ID = string.Format("lParam{0}_op{2}_guid_{1}", index, parameterProperty.Name,
                                         parametersDescriptors.ShortCommandName);
                label.Text = string.Format("{0}: ", parameter.ParameterName);
                controlsForProperty.Add(label);

                if (parameterProperty.PropertyType == typeof (PIMSchema))
                {
                    PIMSchemaLookup pimSchemaLookup = new PIMSchemaLookup();
                    pimSchemaLookup.ID = string.Format("glParam{0}_op{2}_guid_{1}", index, parameter.ParameterPropertyName,
                                                       parametersDescriptors.ShortCommandName);
                    pimSchemaLookup.ProjectVersion = projectVersion;
                    pimSchemaLookup.InitControl();
                    controlsForProperty.Add(pimSchemaLookup);
                    propertyEditors[parameterProperty.Name] = pimSchemaLookup;
                }
                else if (parameterProperty.PropertyType == typeof (PSMSchema))
                {
                    PSMSchemaLookup psmSchemaLookup = new PSMSchemaLookup();
                    psmSchemaLookup.ID = string.Format("glParam{0}_op{2}_guid_{1}", index, parameter.ParameterPropertyName,
                                                       parametersDescriptors.ShortCommandName);
                    psmSchemaLookup.ProjectVersion = projectVersion;
                    psmSchemaLookup.InitControl();
                    controlsForProperty.Add(psmSchemaLookup);
                    propertyEditors[parameter.ParameterPropertyName] = psmSchemaLookup;
                }
                else if (parameterProperty.PropertyType.IsSubclassOf(typeof (Component)))
                {
                    GuidLookup guidLookup = new GuidLookup();
                    guidLookup.ID = string.Format("glParam{0}_op{2}_guid_{1}", index, parameter.ParameterPropertyName,
                                                  parametersDescriptors.ShortCommandName);
                    guidLookup.LookedUpType = parameterProperty.PropertyType;
                    guidLookup.AllowNullInput = parameter.AllowNullInput;
                    guidLookup.ProjectVersion = projectVersion;
                    guidLookup.InitControl();
                    controlsForProperty.Add(guidLookup);
                    propertyEditors[parameter.ParameterPropertyName] = guidLookup;
                }
                else if (parameterProperty.PropertyType == typeof (Guid))
                {
                    GuidLookup guidLookup = new GuidLookup();
                    guidLookup.ID = string.Format("glParam{0}_op{2}_guid_{1}", index, parameterProperty.Name,
                                                  parametersDescriptors.ShortCommandName);
                    guidLookup.LookedUpType = parameter.ComponentType;
                    guidLookup.AllowNullInput = parameter.AllowNullInput;
                    guidLookup.ProjectVersion = projectVersion;
                    guidLookup.InitControl();
                    controlsForProperty.Add(guidLookup);
                    propertyEditors[parameterProperty.Name] = guidLookup;
                }
                else if (parameterProperty.PropertyType == typeof(List<Guid>))
                {
                    Literal brl = new Literal();
                    brl.EnableViewState = false;
                    brl.Text = "<br />";
                    controlsForProperty.Add(brl);
                    GuidListBox guidListBox = new GuidListBox();
                    guidListBox.SelectionMode = ListSelectionMode.Multiple;
                    guidListBox.Width = 400;
                    guidListBox.Rows = 10;
                    guidListBox.ID = string.Format("glParam{0}_op{2}_guid_{1}", index, parameterProperty.Name,
                                                  parametersDescriptors.ShortCommandName);
                    guidListBox.LookedUpType = parameter.ComponentType;
                    guidListBox.AllowNullInput = parameter.AllowNullInput;
                    guidListBox.ProjectVersion = projectVersion;
                    guidListBox.InitControl();
                    controlsForProperty.Add(guidListBox);
                    propertyEditors[parameterProperty.Name] = guidListBox;
                }
                else
                {
                    Control _editor;
                    if (parameterProperty.PropertyType == typeof (string))
                    {
                        StringParameterEditor editor = new StringParameterEditor();
                        _editor = editor;
                    }
                    else if (parameterProperty.PropertyType == typeof(int) || parameterProperty.PropertyType == typeof(uint))
                    {
                        IntParameterEditor editor = new IntParameterEditor();
                        _editor = editor;
                    }
                    else if (parameterProperty.PropertyType == typeof (UnlimitedInt))
                    {
                        UnlimitedintParameterEditor editor = new UnlimitedintParameterEditor();
                        _editor = editor;
                    }
                    else if (parameterProperty.PropertyType == typeof (bool))
                    {
                        BoolParameterEditor editor = new BoolParameterEditor();
                        _editor = editor;
                    } 
                    else if (parameterProperty.PropertyType.IsEnum)
                    {
                        EnumParameterEditor editor = new EnumParameterEditor();
                        editor.EnumType = parameterProperty.PropertyType;
                        _editor = editor;
                    }
                    else
                    {
                        throw new NotImplementedException(String.Format("Not implemented for type {0}.",
                                                                        parameterProperty.PropertyType.Name));
                    }

                    if (_editor is IOperationParameterControl)
                    {
                        ((IOperationParameterControl) _editor).InitControl();
                        if (parameter.SuggestedValue != null)
                        {
                            ((IOperationParameterControl) _editor).SetSuggestedValue(parameter.SuggestedValue);
                        }
                    }
                    _editor.ID = string.Format("glParam{0}_op{2}_guid_{1}", index, parameterProperty.Name,
                                               parametersDescriptors.ShortCommandName);
                    controlsForProperty.Add(_editor);
                    propertyEditors[parameterProperty.Name] = _editor;
                }

                if (propertyEditors[parameterProperty.Name] != null)
                {
                    Control controlForProperty = propertyEditors[parameterProperty.Name];
                    if (!parameter.AllowNullInput && parameterProperty.PropertyType != typeof(bool))
                    {
                        Literal star = new Literal();
                        star.EnableViewState = false;
                        star.Text = " *";
                        controlsForProperty.Add(star);
                        RequiredFieldValidator rfv = new RequiredFieldValidator();
                        rfv.ControlToValidate = controlForProperty.ID;
                        rfv.Display = ValidatorDisplay.Dynamic;
                        rfv.ErrorMessage = "Required parameter. ";
                        rfv.CssClass = "missingRequiredParameter";
                        controlsForProperty.Add(rfv);
                    }
                }


                Literal br = new Literal();
                br.EnableViewState = false;
                br.Text = "<br />";
                controlsForProperty.Add(br);
            }

            List<PropertyInfo> dependentProperties = new List<PropertyInfo>();

            foreach (ParameterDescriptor parameter in parametersDescriptors.Parameters)
            {
                ConsistentWithAttribute consistentWithAttribute;
                if (parameter.ParameterPropertyInfo.TryGetAttribute(out consistentWithAttribute))
                {
                    IOperationParameterControlWithConsistencyCheck dependentPropertyEditor =
                        (IOperationParameterControlWithConsistencyCheck) propertyEditors[parameter.ParameterPropertyName];

                    DropDownList superiorPropertyEditor = (DropDownList) propertyEditors[consistentWithAttribute.PropertyName];
                    superiorPropertyEditor.AutoPostBack = true;

                    dependentPropertyEditor.SuperiorPropertyEditor = (IOperationParameterControl) superiorPropertyEditor;
                    dependentPropertyEditor.ConsistencyChecker = consistentWithAttribute.GetConsistencyInstance();
                    superiorPropertyEditor.SelectedIndexChanged += dependentPropertyEditor.OnSuperiorPropertyChanged;

                    if (superiorPropertyEditor.SelectedValue != null)
                    {
                        dependentPropertyEditor.OnSuperiorPropertyChanged(null, null);
                    }

                    dependentProperties.Add(parameter.ParameterPropertyInfo);
                }
            }

            for (int index = 0; index < parametersDescriptors.Parameters.Count; index++)
            {
                ParameterDescriptor parameter = parametersDescriptors.Parameters[index];
                PropertyInfo parameterProperty = parameter.ParameterPropertyInfo;
                if (dependentProperties.Contains(parameterProperty))
                {
                    continue;
                }
                if (parameterProperty.PropertyType.IsAmong(typeof (Guid), typeof(List<Guid>)))
                {
                    List<Control> controlsForProperty = createdControls[parameterProperty];
                    Type componentType = parameter.ComponentType;

                    int supCount = 0;
                    IOperationParameterControlWithConsistencyCheck dependentPropertyEditor = 
                        (IOperationParameterControlWithConsistencyCheck) propertyEditors[parameterProperty.Name];
                    GuidLookup superiorPropertyEditor = null;
                    IOperationParameterControlWithConsistencyCheck oneBelowSuperiorPropertyEditor = null;
                    Type topLevelType = null;
                    while (InputHierarchy.ContainsKey(componentType))
                    {
                        InputHierarchyStruct inputHierarchyStruct = InputHierarchy[componentType];
                        topLevelType = inputHierarchyStruct.SuperiorType;

                        // label
                        Label label = new Label();
                        label.ID = string.Format("lParam{0}_op{2}_guid_{1}sup{3}", index, parameterProperty.Name, parametersDescriptors.ShortCommandName, supCount);
                        label.Text = string.Format("{0} (for {1}): ", inputHierarchyStruct.Label, parameter.ParameterName);
                        controlsForProperty.Insert(0, label);

                        oneBelowSuperiorPropertyEditor = superiorPropertyEditor ?? dependentPropertyEditor;
                        // drop down list
                        superiorPropertyEditor = new GuidLookup();
                        superiorPropertyEditor.ID = string.Format("glParam{0}_op{2}_guid_{1}sup{3}", index, parameterProperty.Name, parametersDescriptors.ShortCommandName, supCount);
                        superiorPropertyEditor.LookedUpType = inputHierarchyStruct.SuperiorType;
                        superiorPropertyEditor.ProjectVersion = projectVersion;
                        superiorPropertyEditor.InitControl();
                        superiorPropertyEditor.AutoPostBack = true;
                        controlsForProperty.Insert(1, superiorPropertyEditor);
                        
                        dependentPropertyEditor.SuperiorPropertyEditor = superiorPropertyEditor;
                        dependentPropertyEditor.ConsistencyChecker = inputHierarchyStruct.Consistency;
                        superiorPropertyEditor.SelectedIndexChanged += dependentPropertyEditor.OnSuperiorPropertyChanged;

                        if (superiorPropertyEditor.SelectedValue != null)
                        {
                            dependentPropertyEditor.OnSuperiorPropertyChanged(null, null);
                        }

                        // literal
                        Literal br = new Literal();
                        br.EnableViewState = false;
                        br.Text = "<br />";
                        controlsForProperty.Insert(2, br);


                        // prepare for next cycle
                        supCount++;
                        dependentPropertyEditor = superiorPropertyEditor;
                        componentType = inputHierarchyStruct.SuperiorType;

                    }

                    if (superiorPropertyEditor != null)
                    {
                        superiorPropertyEditor.LoadAllPossibleValues(topLevelType);
                        if (superiorPropertyEditor.SelectedValue != null && oneBelowSuperiorPropertyEditor != null)
                        {
                            oneBelowSuperiorPropertyEditor.OnSuperiorPropertyChanged(null, null);
                        }
                    }
                    else
                    {
                        dependentPropertyEditor.LoadAllPossibleValues(componentType);
                    }
                }
            }

            return createdControls.Flatten();
        }

        public static void ReadParameterValues(CommandDescriptor parametersDescriptors, ControlCollection controls)
        {
            foreach (ParameterDescriptor t in parametersDescriptors.Parameters)
            {
                t.ParameterValue = null;
            }
            foreach (Control control in controls)
            {
                if (control is IOperationParameterControl)
                {
                    string propertyName = control.ID.Substring(control.ID.LastIndexOf("_") + 1);
                    ParameterDescriptor parameter = parametersDescriptors.GetParameterByPropertyName(propertyName);
                    if (parameter != null)
                    {
                        PropertyInfo propertyInfo = parameter.ParameterPropertyInfo;
                        if (propertyInfo != null)
                        {
                            if (propertyInfo.PropertyType.IsAmong(typeof (Guid), typeof(List<Guid>)) &&
                                control is IOperationParameterControl<Guid>)
                            {
                                if (control is GuidListBox)
                                {
                                    parameter.ParameterValue = ((GuidListBox)control).Values;
                                }
                                else
                                {
                                    parameter.ParameterValue = ((IOperationParameterControl<Guid>)control).Value;
                                }
                            }
                            else
                            {
                                parameter.ParameterValue = ((IOperationParameterControl) control).Value;
                            }
                        }
                    }
                }
            }
        }


        public static bool VerifyAttributeClassPair(params object[] verifiedObjects)
        {
            PIMAttribute attribute = (PIMAttribute) verifiedObjects[1];
            PIMClass @class = (PIMClass) verifiedObjects[0]; 

            return @class.PIMAttributes.Contains(attribute);
        }
    }

    
}