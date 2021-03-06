﻿using System;
using System.Xml;
using System.Xml.Linq;
using Exolutio.Controller.Commands;
using Exolutio.DataGenerator;
using Exolutio.Dialogs;
using Exolutio.Model.PSM;
using Exolutio.ResourceLibrary;
using Exolutio.SupportingClasses;
using Exolutio.View.Commands.Grammar;

namespace Exolutio.View.Commands
{
	[Scope(ScopeAttribute.EScope.PSMAssociation | ScopeAttribute.EScope.PSMAttribute | ScopeAttribute.EScope.PSMClass)]
	public class guiSampleDocumentCommand : guiScopeCommand
    {
        public override bool CanExecute(object parameter)
        {
            return Current.ActiveDiagram != null && Current.ActiveDiagram is PSMDiagram;
        }

        public override void Execute(object parameter)
        {
            SampleDataGenerator g = new SampleDataGenerator();
	        g.MinimalTree = true;
	        g.EmptyValues = true;
	        g.UseAttributesDefaultValues = false;
	        if (ScopeObject != null)
	        {
		        g.RootForGeneration = (PSMAssociationMember) ScopeObject;
	        }
            XDocument xmlDocument = g.Translate((PSMSchema) Current.ActiveDiagram.Schema);
            FilePresenterButtonInfo[] additionalButtonsInfo = new [] { new FilePresenterButtonInfo() { Text = "Generate another file", Icon = ExolutioResourceNames.GetResourceImageSource(ExolutioResourceNames.xmlIcon), UpdateFileContentAction = GenerateAnotherFile} };
            Current.MainWindow.FilePresenter.DisplayFile(xmlDocument, EDisplayedFileType.XML, Current.ActiveDiagram.Caption + "_sample.xml", g.Log, (PSMSchema) Current.ActiveDiagram.Schema, null, additionalButtonsInfo);


        }

        private void GenerateAnotherFile(IFilePresenterTab filetab)
        {
            SampleDataGenerator g = new SampleDataGenerator();
			if (ScopeObject != null)
			{
				g.RootForGeneration = (PSMAssociationMember)ScopeObject;
			}
            XDocument xmlDocument = g.Translate(filetab.ValidationSchema);
            filetab.SetDocumentText(xmlDocument.ToString());
        }

        public override string Text
        {
            get { return "Sample XML"; }
            set
            {
                base.Text = value;
            }
        }

        public override string ScreenTipText
        {
            get { return "Create sample XML file for the PSM schema "; }
        }

        public override System.Windows.Media.ImageSource Icon
        {
            get { return ExolutioResourceNames.GetResourceImageSource(ExolutioResourceNames.xmlIcon); }
        }
    }
}