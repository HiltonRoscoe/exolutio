﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exolutio.Model.OCL.Types;
using Exolutio.Model.PIM;
using Exolutio.Model.OCL.TypesTable;

namespace Exolutio.Model.OCL.Bridge {
    /// <summary>
    /// Represents class from PIM in OCL type system.
    /// </summary>
    public class PIMBridgeClass : Classifier {
        /// <summary>
        /// Containts source class from PIM.
        /// </summary>
        public PIMClass SourceClass {
            get;
            private set;
        }

        private Dictionary<PIMAttribute, PIMBridgeAttribute> PIMAttribute {
            get;
            set;
        }

        private Dictionary<PIMAssociationEnd, PIMBridgeAssociation> PIMAssociations {
            get;
            set;
        }

        private Dictionary<ModelOperation, Operation> PIMOperations {
            get;
            set;
        }

        public bool IsFromOldVersion
        {
            get; set;
        }


        /// <summary>
        /// Creates instance which represents <paramref name="sourceClass"/> in OCL type system.
        /// </summary>
        /// <param name="tt">Destination OCL type system.</param>
        /// <param name="sourceClass">Source class</param>
        /// <param name="parent">Parent of <paramref name="sourceClass"/> in inheritance hierarchy (if it has any)</param>
        /// <param name="nameOverride">Use this parameter when the class in the OCL should be registered under a 
        /// different name than <paramref name="sourceClass"/>.<see cref="PIMClass.Name"/></param>
        public PIMBridgeClass(TypesTable.TypesTable tt, Namespace ns, PIMClass sourceClass, PIMBridgeClass parent = null,
            string nameOverride = null)
            : base(tt, ns, nameOverride ?? sourceClass.Name, parent ?? tt.Library.Any) {
            this.SourceClass = sourceClass;
            PIMAttribute = new Dictionary<PIMAttribute, PIMBridgeAttribute>();
            PIMAssociations = new Dictionary<PIMAssociationEnd, PIMBridgeAssociation>();
            PIMOperations = new Dictionary<ModelOperation, Operation>();
        }

        /// <summary>
        /// Tries find instance of PIMBridgeAttribute associated with <paramref name="att"/> from PIM.
        /// </summary>
        /// <exception cref="KeyNotFoundException"><paramref name="att"/> not exists in this class.</exception>
        public PIMBridgeAttribute FindAttribute(PIMAttribute att) {
            return PIMAttribute[att];
        }

        /// <summary>
        /// Tries find instance of PIMBridgeAssociation associated with <paramref name="ass"/> from PIM.
        /// </summary>
        /// <exception cref="KeyNotFoundException"><paramref name="ass"/> not exists in this class.</exception>
        public PIMBridgeAssociation FindAssociation(PIMAssociationEnd assEnd) {
            return PIMAssociations[assEnd];
        }

        /// <summary>
        /// Tries find instance of Operation associated with <paramref name="op"/> from PIM.
        /// </summary>
        /// <exception cref="KeyNotFoundException"><paramref name="op"/> not exists in this class.</exception>
        public Operation FindOperation(ModelOperation op) {
            return PIMOperations[op];
        }


        internal void TranslateMembers() {

            //Attributy
            foreach (var pr in SourceClass.PIMAttributes) {

                Classifier baseType = pr.AttributeType != null ? TypeTable.Library.RootNamespace.NestedClassifier[pr.AttributeType.Name] : TypeTable.Library.Any;
                Classifier propType = BridgeHelpers.GetTypeByCardinality(TypeTable, pr, baseType);
                PIMBridgeAttribute newProp = new PIMBridgeAttribute(pr, PropertyType.One, propType);
                Properties.Add(newProp);
                //Hack
                newProp.Tag = pr;
                //Registration to find
                PIMAttribute.Add(pr, newProp);
            }


            //Associace

            foreach (var ass in SourceClass.PIMAssociationEnds) {
                var end = ass.PIMAssociation.PIMAssociationEnds.Where(a => a.ID != ass.ID).First();
                Classifier assType = TypeTable.Library.RootNamespace.NestedClassifier[end.PIMClass.Name];
                string name;
                if (string.IsNullOrEmpty(end.Name)) {
                    name = assType.Name;
                }
                else {
                    name = end.Name;
                }
                Classifier propType = BridgeHelpers.GetTypeByCardinality(TypeTable, end, assType);
                TypeTable.RegisterType(propType);
                PIMBridgeAssociation newass = new PIMBridgeAssociation(name, ass.PIMAssociation, end, PropertyType.One, propType);
                Properties.Add(newass);

                //hack
                newass.Tag = end;
                //Registration to find
                PIMAssociations.Add(end, newass);
            }


            //Operation

            foreach (var op in SourceClass.PIMOperations) {
                Operation newOp = new Operation(op.Name, true,
                    op.ResultType != null ? TypeTable.Library.RootNamespace.NestedClassifier[op.ResultType.Name] : TypeTable.Library.Void,
                    op.Parameters.Select(p => new Parameter(p.Name, TypeTable.Library.RootNamespace.NestedClassifier[p.Type.Name])));
                Operations.Add(newOp);

                //hack
                newOp.Tag = op;
                // Registration to find
                PIMOperations.Add(op, newOp);
            }

            // allInstances
            {
                Operation allInstancesOp = new Operation(@"allInstances", true, this);
                Operations.Add(allInstancesOp);
            }
        }

        

    }

}
