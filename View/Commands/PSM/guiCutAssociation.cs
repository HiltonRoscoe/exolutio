using System;
using Exolutio.Dialogs;
using Exolutio.ResourceLibrary;
using Exolutio.Model.PSM;
using System.Linq;
using System.Collections.Generic;
using Exolutio.Controller.Commands.Complex.PSM;
using Exolutio.Controller.Commands;
using Exolutio.Controller.Commands.Atomic.PSM;
using System.Diagnostics;

namespace Exolutio.View.Commands.PSM
{
    public class guiCutAssociation : guiSelectionDependentCommand
    {
        public override bool CanExecute(object parameter)
        {
            if (!(Current.ActiveDiagram is PSMDiagram)) return false;

            IEnumerable<PSMAssociation> selectedAssociations = Current.ActiveDiagramView.GetSelectedComponents()
                .Where(c => c is PSMAssociation).Cast<PSMAssociation>();

            return selectedAssociations.Count() > 0;

        }

        public override void Execute(object parameter)
        {
            IEnumerable<PSMAssociation> selectedAssociations = Current.ActiveDiagramView.GetSelectedComponents()
                .Where(c => c is PSMAssociation).Cast<PSMAssociation>();

            MacroCommand command = new MacroCommand(Current.Controller);
            foreach (PSMAssociation a in selectedAssociations)
            {
                command.Commands.Add(new cmdDeletePSMAssociation(Current.Controller) { AssociationGuid = a });
            }
            command.Execute();
        }

        public override string Text
        {
            get
            {
                return "Cut associations";
            }
        }

        public override string ScreenTipText
        {
            get { return "Cut PSM associations"; }
        }

        public override System.Windows.Media.ImageSource Icon
        {
            get
            {
                return ExolutioResourceNames.GetResourceImageSource(ExolutioResourceNames.delete);
            }
        }
    }
}