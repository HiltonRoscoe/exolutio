﻿using System;
using Exolutio.Model.PIM;

namespace Exolutio.Controller.Commands.Atomic.PIM
{
    internal class acmdUpdatePIMAttributeDefaultValue : AtomicCommand
    {
        Guid attributeGuid;
        string newDefaultValue, oldDefaultValue;

        public acmdUpdatePIMAttributeDefaultValue(Controller c, Guid PIMAttributeGuid, string defaultValue)
            : base(c)
        {
            attributeGuid = PIMAttributeGuid;
            newDefaultValue = defaultValue;
        }

        public override bool CanExecute()
        {
            return attributeGuid != Guid.Empty
                   && Project.VerifyComponentType<PIMAttribute>(attributeGuid);
        }

        internal override void CommandOperation()
        {
            PIMAttribute PIMAttribute = Project.TranslateComponent<PIMAttribute>(attributeGuid);
            oldDefaultValue = PIMAttribute.DefaultValue;
            PIMAttribute.DefaultValue = newDefaultValue;

            Report = new CommandReport("Default value of '{0}' changed from '{1}' to '{2}'.", PIMAttribute, oldDefaultValue, newDefaultValue);
        }

        internal override CommandBase.OperationResult UndoOperation()
        {
            PIMAttribute PIMAttribute = Project.TranslateComponent<PIMAttribute>(attributeGuid);
            PIMAttribute.DefaultValue = oldDefaultValue;
            return OperationResult.OK;
        }
    }
}