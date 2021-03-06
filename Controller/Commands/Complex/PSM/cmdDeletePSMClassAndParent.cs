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
    [PublicCommand("Delete PSM class and parent PSM association (complex)", PublicCommandAttribute.EPulicCommandCategory.PSM_complex)]
    public class cmdDeletePSMClassAndParent : ComposedCommand
    {
        [PublicArgument("Deleted PSM class", typeof(PSMClass))]
        [Scope(ScopeAttribute.EScope.PSMClass)]
        public Guid ClassGuid { get; set; }

        public cmdDeletePSMClassAndParent()
        {
            CheckFirstOnlyInCanExecute = true;
        }

        public cmdDeletePSMClassAndParent(Controller c)
            : base(c)
        {
            CheckFirstOnlyInCanExecute = true;
        }

        public void Set(Guid classGuid)
        {
            ClassGuid = classGuid;
            
        }

        internal override void GenerateSubCommands()
        {
            PSMClass psmClass = Project.TranslateComponent<PSMClass>(ClassGuid);
            if (psmClass.ParentAssociation != null)
            {
                cmdDeletePSMAssociation cmddelete = new cmdDeletePSMAssociation(Controller) { Propagate = Propagate };
                cmddelete.Set(psmClass.ParentAssociation);
                Commands.Add(cmddelete);
            }
            cmdDeleteRootPSMClass c = new cmdDeleteRootPSMClass(Controller) { Propagate = Propagate };
            c.Set(ClassGuid);
            Commands.Add(c);
        }

        public override bool CanExecute()
        {
            if (ClassGuid == Guid.Empty) return false;
            return base.CanExecute();
        }

        internal override void CommandOperation()
        {
            base.CommandOperation();
            Report = new CommandReport(CommandReports.COMPLEX_DELETE_PSM_CLASS_AND_PARENT);
        }

    }
}
