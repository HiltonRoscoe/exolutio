﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exolutio.Model;
using Exolutio.Model.PIM;
using Exolutio.Model.PSM;
using Exolutio.Controller.Commands.Atomic;
using Exolutio.Controller.Commands.Atomic.PSM;

namespace Exolutio.Controller.Commands.Complex.PSM
{
    [PublicCommand("Add PSM class as interpreted child (complex)", PublicCommandAttribute.EPulicCommandCategory.PSM_complex)]
    public class cmdCreateNewPSMClassAsIntChild : MacroCommand
    {
        [PublicArgument("Parent PSM class", typeof(PSMClass))]
        [Scope(ScopeAttribute.EScope.PSMClass)]
        public Guid ParentPSMClassGuid { get; set; }

        [PublicArgument("PIM association end", typeof(PIMAssociationEnd))]
        public Guid PIMAssociationEndGuid { get; set; }

        /// <summary>
        /// Preffered Guid of the new PSM class
        /// </summary>
        public Guid ClassGuid { get; set; }

        /// <summary>
        /// Preffered Guid of the new PSM association
        /// </summary>
        public Guid AssociationGuid { get; set; }

        public cmdCreateNewPSMClassAsIntChild()
        {
            CheckFirstOnlyInCanExecute = true;
        }

        public cmdCreateNewPSMClassAsIntChild(Controller c)
            : base(c)
        {
            CheckFirstOnlyInCanExecute = true;
        }

        public void Set(Guid parentPSMClassGuid, Guid pimAssociationEndGuid, Guid newClassGuid, Guid newAssociationGuid)
        {
            ParentPSMClassGuid = parentPSMClassGuid;
            PIMAssociationEndGuid = pimAssociationEndGuid;
            ClassGuid = newClassGuid;
            AssociationGuid = newAssociationGuid;
        }

        protected override void GenerateSubCommands()
        {
            if (ClassGuid == Guid.Empty) ClassGuid = Guid.NewGuid();
            if (AssociationGuid == Guid.Empty) AssociationGuid = Guid.NewGuid();

            PSMClass parent = Project.TranslateComponent<PSMClass>(ParentPSMClassGuid);
            PIMAssociationEnd associationEnd = Project.TranslateComponent<PIMAssociationEnd>(PIMAssociationEndGuid);

            Commands.Add(new acmdNewPSMClass(Controller, parent.PSMSchema) { ClassGuid = ClassGuid });
            Commands.Add(new acmdRenameComponent(Controller, ClassGuid, associationEnd.PIMClass.Name));
            Commands.Add(new acmdSetPSMClassInterpretation(Controller, ClassGuid, associationEnd.PIMClass));
            Commands.Add(new acmdNewPSMAssociation(Controller, parent, ClassGuid, parent.PSMSchema) { AssociationGuid = AssociationGuid });
            Commands.Add(new acmdSetPSMAssociationInterpretation(Controller, AssociationGuid, associationEnd.PIMAssociation));
            foreach (PIMAttribute a in associationEnd.PIMClass.PIMAttributes)
            {
                Guid attrGuid = Guid.NewGuid();
                cmdCreateNewPSMAttribute c = new cmdCreateNewPSMAttribute(Controller) { AttributeGuid = attrGuid };
                c.Set(ClassGuid, a.AttributeType, a.Name, a.Lower, a.Upper, false);
                Commands.Add(c);
                Commands.Add(new acmdSetPSMAttributeInterpretation(Controller, attrGuid, a));
            }

        }

        public override bool CanExecute()
        {
            if (ParentPSMClassGuid == Guid.Empty || PIMAssociationEndGuid == Guid.Empty) return false;
            PSMClass parent = Project.TranslateComponent<PSMClass>(ParentPSMClassGuid);
            PIMAssociationEnd associationEnd = Project.TranslateComponent<PIMAssociationEnd>(PIMAssociationEndGuid);
            if (parent.Interpretation == null) return false;
            if (!(parent.Interpretation as PIMClass).GetAssociationsWith(associationEnd.PIMClass).Contains(associationEnd.PIMAssociation)) return false;
            return true;
        }

        internal override void CommandOperation()
        {
            Report = new CommandReport(CommandReports.COMPLEX_NEW_PSM_CLASS_AS_CHILD);
            base.CommandOperation();
        }
        
    }
}