﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exolutio.Model.OCL.Types
{

    //TODO TemplateParameterType

    /// <summary>
    /// A TemplateParameterType is used to refer to generic types in parameterized definitions.
    /// </summary>
    public class TemplateParameterType:Classifier
    {
        public TemplateParameterType(TypesTable.TypesTable tt,Namespace ns)
            : base(tt,ns,"")
        {
            throw new NotImplementedException();
        }
    }
}
