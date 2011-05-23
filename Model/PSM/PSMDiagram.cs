using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using EvoX.Model.Serialization;
using EvoX.Model.Versioning;
using EvoX.Model.ViewHelper;

namespace EvoX.Model.PSM
{
    public class PSMDiagram : Diagram
    {
        public PSMSchema PSMSchema
        {
            get { return (PSMSchema)Schema; }
            set { Schema = value; }
        }

        public PSMDiagram(Project p)
            : base(p)
        {
        }

        public PSMDiagram(Project p, Guid g)
            : base(p, g)
        {
        }

        public override void AddFactoryMethods()
        {
            viewHelperFactoryMethods[typeof(PSMClass)] = delegate { return new PSMClassViewHelper(this); };
            viewHelperFactoryMethods[typeof(PSMSchemaClass)] = delegate { return new PSMSchemaClassViewHelper(this); };
            viewHelperFactoryMethods[typeof(PSMAssociation)] = delegate { return new PSMAssociationViewHelper(this); };
            viewHelperFactoryMethods[typeof(PSMContentModel)] = delegate { return new PSMContentModelViewHelper(this); };
        }

        public IEnumerable<PSMComponent> PSMComponents
        {
            get { return Components.Cast<PSMComponent>(); }
        }

        /// <summary>
        /// Associates schema with diagram. When <paramref name="bindingOnly"/> is set to false (default), 
        /// adds all components of the schema to the diagram. Binds diagram to the <see cref="PSM.PSMSchema.ComponentAdded"/>
        /// and <see cref="PSM.PSMSchema.ComponentRemoved"/> so that every component added or removed to/from the schema
        /// in the future is correspondingly added/removed from the diagram. 
        /// </summary>
        /// <param name="psmSchema"></param>
        /// <param name="bindingOnly"></param>
        public void LoadSchemaToDiagram(PSMSchema psmSchema, bool bindingOnly = false)
        {
            PSMSchema = psmSchema;

            if (!bindingOnly)
            {
                Caption = psmSchema.Caption;

                foreach (PSMComponent psmComponent in ModelIterator.GetPSMComponents(psmSchema))
                {
                    if (psmComponent.IsOfType(typeof (PSMClass), typeof (PSMAssociation), typeof (PSMContentModel),
                                              typeof (PSMSchemaClass)))
                    {
                        Components.Add(psmComponent);
                    }
                }
            }

            PSMSchema.ComponentAdded += Components_ComponentAdded;
            PSMSchema.ComponentRemoved += Components_ComponentRemoved;
        }

        private void Components_ComponentAdded(Schema psmschema, Component component)
        {
            if (viewHelperFactoryMethods.ContainsKey(component.GetType()))
                Components.Add(component);
        }

        private void Components_ComponentRemoved(Schema psmschema, Component component)
        {
            if (viewHelperFactoryMethods.ContainsKey(component.GetType()))
            {
                Components.Remove(component);
                ViewHelpers.Remove(component);
            }
        }

        #region Implementation of IEvoXSerializable

        public override void Serialize(XElement parentNode, SerializationContext context)
        {
            base.Serialize(parentNode, context);

        }

        public override void Deserialize(XElement parentNode, SerializationContext context)
        {
            base.Deserialize(parentNode, context);
            LoadSchemaToDiagram(PSMSchema, true);

        }
        #endregion

        #region Implementation of IEvoXCloneable

        public override IEvoXCloneable Clone(ProjectVersion projectVersion, ElementCopiesMap createdCopies)
        {
            return new PSMDiagram(projectVersion.Project, createdCopies.SuggestGuid(this));
        }

        public override void FillCopy(IEvoXCloneable copyComponent, ProjectVersion projectVersion,
                                      ElementCopiesMap createdCopies)
        {
            base.FillCopy(copyComponent, projectVersion, createdCopies);

            PSMDiagram copyPSMDiagram = (PSMDiagram)copyComponent;
            copyPSMDiagram.SetProjectVersion(projectVersion);
            copyPSMDiagram.LoadSchemaToDiagram(copyPSMDiagram.PSMSchema, true);

        }

        #endregion

        public static PSMDiagram CreateInstance(Project project)
        {
            return new PSMDiagram(project, Guid.Empty);
        }
    }
}