﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exolutio.Model;
using Exolutio.Model.PSM;
using Exolutio.Controller.Commands.Atomic;
using Exolutio.Controller.Commands.Atomic.PSM;

namespace Exolutio.Controller.Commands.Complex.PSM
{
    [PublicCommand("Delete PSM schema (complex)", PublicCommandAttribute.EPulicCommandCategory.PSM_complex)]
    public class cmdDeletePSMSchema : ComposedCommand
    {
        [PublicArgument("Deleted PSM schema", typeof(PSMSchema))]
        [Scope(ScopeAttribute.EScope.PSMSchema)]
        public Guid SchemaGuid { get; set; }
        
        public cmdDeletePSMSchema()
        {
            CheckFirstOnlyInCanExecute = true;
        }

        public cmdDeletePSMSchema(Controller c)
            : base(c)
        {
            CheckFirstOnlyInCanExecute = true;
        }

        public void Set(Guid schemaGuid)
        {
            SchemaGuid = schemaGuid;
            
        }

        internal override void GenerateSubCommands()
        {
            PSMSchema schema = Project.TranslateComponent<PSMSchema>(SchemaGuid);
            foreach (PSMAssociationMember m in schema.Roots)
            {
                Commands.Add(new cmdDeletePSMAssociationMemberRecursive(Controller) { AssociationMemberGuid = m });
            }

            PSMDiagram psmDiagram = schema.ProjectVersion.PSMDiagrams.FirstOrDefault(d => d.PSMSchema == schema);
            if (psmDiagram != null)
            {
                Commands.Add(new acmdDeletePSMDiagram(Controller) { DiagramGuid = psmDiagram.ID, SchemaGuid = SchemaGuid });
            }

            Commands.Add(new acmdDeletePSMSchema(Controller, schema));
        }

        public override bool CanExecute()
        {
            if (SchemaGuid == Guid.Empty) return false;
            return base.CanExecute();
        }

        internal override void CommandOperation()
        {
            base.CommandOperation();
            Report = new CommandReport(CommandReports.COMPLEX_DELETE_PSM_SCHEMA);
        }
    }
}
