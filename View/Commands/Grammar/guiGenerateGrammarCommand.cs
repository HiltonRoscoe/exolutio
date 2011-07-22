﻿using Exolutio.Model.PSM;
using Exolutio.Model.PSM.Grammar;
using Exolutio.ResourceLibrary;

namespace Exolutio.View.Commands.Grammar
{
    public class guiGenerateGrammarCommand : guiActiveDiagramCommand
    {   
        public override void Execute(object parameter = null)
        {
            if (Current.ActiveDiagram != null && Current.ActiveDiagram is PSMDiagram)
            {
                GrammarGenerator generator = new GrammarGenerator();
                Exolutio.Model.PSM.Grammar.Grammar grammar = generator.GenerateGrammar((PSMSchema) Current.ActiveDiagram.Schema);
                
#if SILVERLIGHT
                GrammarWindow w = new GrammarWindow();
                w.DisplayedGrammar = grammar; 
                w.ShowDialog();
#else 
                GrammarWindow w = new GrammarWindow();
                w.DisplayedGrammar = grammar;
                w.ShowDialog();
#endif
            }
        }

        public override string Text
        {
            get { return "Generate grammar"; }
        }

        public override string ScreenTipText
        {
            get { return "Generate regular tree grammar from the PSM schema"; }
        }

        public override bool CanExecute(object parameter)
        {
            return Current.ActiveDiagram != null && Current.ActiveDiagram is PSMDiagram;
        }

        public override System.Windows.Media.ImageSource Icon
        {
            get { return ExolutioResourceNames.GetResourceImageSource(ExolutioResourceNames.component_new); }
        }
    }
}