﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exolutio.Controller.Commands;
using Exolutio.Model.PSM;
using Exolutio.Model;

namespace Exolutio.Controller.Commands.Atomic
{
    internal class acmdUpdateAttributeType : AtomicCommand
    {
        public Guid AttributeTypeGuid { get; set; }

        public string NewName { get; set; }

        public string NewXSDDefinition { get; set; }

        public bool NewIsSealed { get; set; }

        public Guid NewBaseType { get; set; }

        private string oldname;
        private string oldXSDDefinition;
        private bool oldIsSealed;
        private Guid oldBaseType { get; set; }

        public acmdUpdateAttributeType()
        {
            
        }

        public acmdUpdateAttributeType(Controller c, Guid attributeTypeGuid, string newName, string newXSDDefinition, bool isSealed, Guid newBaseType)
            : base(c)
        {
            AttributeTypeGuid = attributeTypeGuid;
            NewName = newName;
            NewXSDDefinition = newXSDDefinition;
            NewIsSealed = isSealed;
            NewBaseType = newBaseType;
        }

        public override bool CanExecute()
        {
            return AttributeTypeGuid != Guid.Empty;
        }
        
        internal override void CommandOperation()
        {
            AttributeType attributeType = Project.TranslateComponent<AttributeType>(AttributeTypeGuid);
            oldname = attributeType.Name;
            oldBaseType = attributeType.BaseType;
            oldXSDDefinition = attributeType.XSDDefinition;
            oldIsSealed = attributeType.IsSealed;
            
            attributeType.Name = NewName;
            attributeType.BaseType = NewBaseType != Guid.Empty ? 
                Project.TranslateComponent<AttributeType>(NewBaseType) : null;
            attributeType.XSDDefinition = NewXSDDefinition;
            attributeType.IsSealed = NewIsSealed;
            
            Report = new CommandReport("'{0}' updated. ");
        }

        internal override CommandBase.OperationResult UndoOperation()
        {
            AttributeType attributeType = Project.TranslateComponent<AttributeType>(AttributeTypeGuid);
            attributeType.Name = oldname;
            attributeType.XSDDefinition = oldXSDDefinition;
            attributeType.IsSealed = oldIsSealed;
            attributeType.BaseType = oldBaseType != Guid.Empty ? 
                Project.TranslateComponent<AttributeType>(oldBaseType) : null;
            return OperationResult.OK;
        }
    }
}
