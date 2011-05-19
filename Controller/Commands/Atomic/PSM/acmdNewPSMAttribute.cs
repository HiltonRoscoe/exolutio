﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EvoX.Controller.Commands;
using EvoX.Model.PSM;
using EvoX.Model;

namespace EvoX.Controller.Commands.Atomic.PSM
{
    public class acmdNewPSMAttribute : StackedCommand
    {
        private Guid schemaGuid;

        private Guid attributeGuid = Guid.Empty;

        private Guid classGuid = Guid.Empty;

        /// <summary>
        /// If set before execution, creates a new attribute with this GUID.
        /// After execution contains GUID of the created attribute.
        /// </summary>
        public Guid AttributeGuid
        {
            get { return attributeGuid; }
            set
            {
                if (!Executed) attributeGuid = value;
                else throw new EvoXCommandException("Cannot set AttributeGuid after command execution.", this);
            }
        }

        public acmdNewPSMAttribute(Controller c, Guid psmClassGuid, Guid psmSchemaGuid)
            : base(c)
        {
            schemaGuid = psmSchemaGuid;
            classGuid = psmClassGuid;
        }

        public override bool CanExecute()
        {
            return classGuid != Guid.Empty && Project.VerifyComponentType<PSMClass>(classGuid)
                && schemaGuid != Guid.Empty && Project.VerifyComponentType<PSMSchema>(schemaGuid);
        }
        
        internal override void CommandOperation()
        {
            if (AttributeGuid == Guid.Empty) AttributeGuid = Guid.NewGuid();
            PSMAttribute psmAttribute = new PSMAttribute(Project, AttributeGuid, Project.TranslateComponent<PSMClass>(classGuid), Project.TranslateComponent<PSMSchema>(schemaGuid));
            Report = new CommandReport(CommandReports.PSM_component_added, psmAttribute);
        }

        internal override CommandBase.OperationResult UndoOperation()
        {
            PSMAttribute a = Project.TranslateComponent<PSMAttribute>(AttributeGuid);
            Project.TranslateComponent<PSMSchema>(schemaGuid).PSMAttributes.Remove(a);
            Project.TranslateComponent<PSMClass>(classGuid).PSMAttributes.Remove(a);
            Project.mappingDictionary.Remove(AttributeGuid);
            return OperationResult.OK;
        }
    }
}
