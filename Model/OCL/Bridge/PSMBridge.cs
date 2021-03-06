﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exolutio.Model.OCL.Types;
using Exolutio.Model.PSM;
using Exolutio.Model.OCL.TypesTable;

namespace Exolutio.Model.OCL.Bridge
{
    public class PSMBridge : IBridgeToOCL
    {
        public PSMSchema Schema
        {
            get;
            private set;
        }

        Schema IBridgeToOCL.Schema { get { return Schema; } }

        public TypesTable.TypesTable TypesTable
        {
            get;
            private set;
        }

        public TypesTable.Library Library
        {
            get
            {
                return this.TypesTable.Library;
            }
        }

        internal Dictionary<PSM.PSMAssociationMember, PSMBridgeClass> PSMAssociationMembers
        {
            get;
            private set;
        }

        internal Dictionary<AttributeType, Types.Classifier> PSMAttributeType
        {
            get;
            private set;
        }


        public PSMBridge(PSMSchema schema)
        {
            PSMAssociationMembers = new Dictionary<PSMAssociationMember, PSMBridgeClass>();
            PSMAttributeType = new Dictionary<AttributeType, Classifier>();
            this.Schema = schema;
            CreateTypesTable();
        }

        /// <summary>
        /// Gets the class from type associated with the PSM class.
        /// </summary>
        /// <exception cref="KeyNotFoundException">PIM class does not exist in collection.</exception>
        public PSMBridgeClass Find(PSMAssociationMember psmMember)
        {
            return PSMAssociationMembers[psmMember];
        }

        /// <summary>
        /// Gets the class from type associated with the attribute type.
        /// </summary>
        /// <exception cref="KeyNotFoundException">Attribute type does not exist in collection.</exception>
        public Types.Classifier Find(AttributeType psmAttType)
        {
            return PSMAttributeType[psmAttType];
        }

        public Classifier Find(Component component)
        {
            if (component is PSMAssociationMember)
            {
                return Find((PSMAssociationMember)component);
            }
            else
                throw new ExolutioModelException(
                    string.Format(
                        "PSMBridge can locate only components of type `PSMAssociationMember`. Type of component `{0}` is `{1}`.",
                        component, component.GetType().Name));
        }

        private void CreateTypesTable()
        {
            Library.StandardTypeName naming = new OCL.TypesTable.Library.StandardTypeName();
            naming.Any = "any";
            naming.Boolean = "boolean";
            naming.Integer = "integer";
            naming.Invalid = "invalid";
            naming.Message = "message";
            naming.Real = "double";
            naming.String = "string";
            naming.Type = "type";
            naming.UnlimitedNatural = "unlimitedNatural";
            naming.Void = "void";

            TypesTable = new TypesTable.TypesTable(naming);
            TypesTable.StandardLibraryCreator sLC = new TypesTable.StandardLibraryCreator();
            sLC.CreateStandardLibrary(TypesTable);

			#region XML schema additional primitive types 

			var @decimal = new Classifier(TypesTable, TypesTable.Library.RootNamespace, "decimal", TypesTable.Library.Integer);
			TypesTable.RegisterType(@decimal);
			var date = new Classifier(TypesTable, TypesTable.Library.RootNamespace, "date");
			TypesTable.RegisterType(date);


			#endregion 

			// Docasna podpora pro typy v Tournaments.eXo
            
            date.Operations.Add(new Operation("after", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("time", date) }));
            date.Operations.Add(new Operation("before", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("time", date) }));
            date.Operations.Add(new Operation("equals", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("time", date) }));
            date.Operations.Add(new Operation("trunc", true, date, new Parameter[] { }));
            date.Operations.Add(new Operation("=", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("other", date) }));
            date.Operations.Add(new Operation("<=", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("other", date) }));
            date.Operations.Add(new Operation("<", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("other", date) }));
            date.Operations.Add(new Operation(">", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("other", date) }));
            date.Operations.Add(new Operation(">=", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("other", date) }));

            TypesTable.RegisterType(date);
            //  TypesTable.Library.RootNamespace.NestedClassifier.Add(date);


            Class dateTime = new Class(TypesTable, TypesTable.Library.RootNamespace, "dateTime");
            dateTime.Operations.Add(new Operation("after", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("time", dateTime) }));
            dateTime.Operations.Add(new Operation("before", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("time", dateTime) }));
            dateTime.Operations.Add(new Operation("equals", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("time", dateTime) }));
            dateTime.Operations.Add(new Operation("trunc", true, dateTime, new Parameter[] { }));
            dateTime.Operations.Add(new Operation("getDate", true, dateTime, new Parameter[] { }));
            dateTime.Operations.Add(new Operation("=", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("other", dateTime) }));
            dateTime.Operations.Add(new Operation("<=", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("other", dateTime) }));
            dateTime.Operations.Add(new Operation("<", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("other", dateTime) }));
            dateTime.Operations.Add(new Operation(">", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("other", dateTime) }));
            dateTime.Operations.Add(new Operation(">=", true, TypesTable.Library.Boolean, new Parameter[] { new Parameter("other", dateTime) }));
            TypesTable.RegisterType(dateTime);
            //  TypesTable.Library.RootNamespace.NestedClassifier.Add(dateTime);


            Class matchesStatus = new Class(TypesTable, TypesTable.Library.RootNamespace, "MatchStatus");
            // TypesTable.Library.RootNamespace.NestedClassifier.Add(matchesStatus);
            TypesTable.RegisterType(matchesStatus);

            TranslateSchema(Schema, false);
	        TypesTable.PerformDeferredInitializations();
        }


        public void TranslateSchema(Schema schema, bool translateAsOldVersion = false)
        {
            PSMSchema psmSchema = (PSMSchema)schema;
            IEnumerable<AttributeType> attTypes = psmSchema.GetAvailablePSMTypes();

            foreach (AttributeType type in attTypes)
            {
                if (translateAsOldVersion && schema.Project.PSMBuiltInTypes.Contains(type))
                    continue;
                Classifier existsCl;
                if (Library.RootNamespace.NestedClassifier.TryGetValue(type.Name, out existsCl))
                {
                    PSMAttributeType.Add(type, existsCl);
                    continue;
                }
                Classifier parent;
                if (type.BaseType == null)
                {
                    parent = Library.Any;
                }
                else
                {
                    if (Library.RootNamespace.NestedClassifier.TryGetValue(type.BaseType.Name, out parent) == false)
                    {
                        parent = Library.Any;
                    }
                }
                Classifier newClassifier = new Classifier(Library.TypeTable, Library.TypeTable.Library.RootNamespace, type.Name, parent);
                Library.TypeTable.RegisterType(newClassifier);
                //    Library.RootNamespace.NestedClassifier.Add(newClassifier);
                PSMAttributeType.Add(type, newClassifier);
            }

            List<PSMBridgeClass> classToProcess = new List<PSMBridgeClass>();
            // JM: usporadani trid tak, aby predkove byli zalozeni pred potomky
            List<PSMClass> psmClassesHierarchy = ModelIterator.GetPSMClassesInheritanceBFS(psmSchema).ToList();

            foreach (PSM.PSMClass psmClass in psmClassesHierarchy)
            {
                // JM: parent - predek v PSM modelu
                PSMBridgeClass parent = psmClass.GeneralizationAsSpecific != null ? PSMAssociationMembers[psmClass.GeneralizationAsSpecific.General] : null;
                string nameOverride = translateAsOldVersion ? psmClass.Name + @"_old" : null;
                PSMBridgeClass newClass = new PSMBridgeClass(Library.TypeTable, Library.TypeTable.Library.RootNamespace, psmClass, parent, nameOverride);
                //  tt.Library.RootNamespace.NestedClassifier.Add(newClass);
                Library.TypeTable.RegisterType(newClass);
                classToProcess.Add(newClass);
                //Hack
                newClass.Tag = psmClass;
                //Registred to find
                PSMAssociationMembers.Add(psmClass, newClass);
            }

            foreach (var cM in psmSchema.PSMContentModels)
            {
                string cMName = PSMBridgeClass.GetContentModelOCLName(cM);
                if (translateAsOldVersion)
                {
                    cMName += @"_old";
                }
                PSMBridgeClass newClass = new PSMBridgeClass(Library.TypeTable, Library.TypeTable.Library.RootNamespace, cM, cMName);
                // tt.Library.RootNamespace.NestedClassifier.Add(newClass);
                Library.TypeTable.RegisterType(newClass);
                classToProcess.Add(newClass);
                newClass.Tag = cM;
                //Registred to find
                PSMAssociationMembers.Add(cM, newClass);
            }

            classToProcess.ForEach(cl => cl.TranslateMembers(this, translateAsOldVersion));
            List<PSMBridgeClass> orderedSRs = ModelIterator.GetPSMClassesStructuralRepresentativeRelationBFS(psmSchema).Where(c => c.IsStructuralRepresentative).Select(psmc => PSMAssociationMembers[psmc]).ToList();
            orderedSRs.ForEach(cl => cl.IncludeSRMembers(PSMAssociationMembers[((PSMClass)cl.PSMSource).RepresentedClass]));
        }
    }
}
