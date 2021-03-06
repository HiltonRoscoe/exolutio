﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Schema;
using Exolutio.Model.PIM;
using Exolutio.Model.Resources;
using Exolutio.Model.Serialization;
using Exolutio.Model.Versioning;
using Version = Exolutio.Model.Versioning.Version;
using System.Collections.ObjectModel;
using System.Linq;

namespace Exolutio.Model
{
    /// <summary>
    /// Exolutio project, encapsulates the respective versions. If versioning is not
    /// applied in the project, property SingleVersion contains the only
    /// oldVersion of the project.
    /// </summary>
    public class Project : IExolutioSerializable, INotifyPropertyChanged
    {
        public Project()
        {
            InitializeCollections();
            LoadBuiltInTypes();
	        LoadBuiltInStereotypes(); 
        }

        private void InitializeCollections()
        {
            projectVersions = new UndirectCollection<ProjectVersion>(this);
            projectVersions.MemberAdded += delegate { NotifyPropertyChanged("ProjectVersions"); };
            projectVersions.MemberRemoved += delegate { NotifyPropertyChanged("ProjectVersions"); };
            pimBuiltInTypes = new UndirectCollection<AttributeType>(this);
            psmBuiltInTypes = new UndirectCollection<AttributeType>(this);
			builtInStereotypes = new UndirectCollection<Stereotype>(this);
        }

        private void LoadBuiltInTypes()
        {
            XNamespace pimBuiltInTypesNS = @"http://eXolutio.eu/Project/PIM/built-in-types/";
            XNamespace psmBuiltInTypesNS = @"http://eXolutio.eu/Project/PSM/built-in-types/";
            using (StringReader reader = new StringReader(Properties.Resources.PIM_built_in_types))
            {
                XDocument xbuiltInTypes = XDocument.Load(reader);
                IEnumerable<XElement> types =
                    xbuiltInTypes.Element(pimBuiltInTypesNS + "Project").Element(pimBuiltInTypesNS + "AttributeTypes").
                        Elements(pimBuiltInTypesNS + "AttributeType");

                SerializationContext context = new SerializationContext();

                foreach (XElement type in types)
                {
                    AttributeType attributeType = new AttributeType(this, Guid.Empty);
                    attributeType.Deserialize(type, context);
                    foreach (ModelOperation modelOperation in attributeType.Operations)
                    {
                        modelOperation.DeclaringType = attributeType;
                    }
                    pimBuiltInTypes.Add(attributeType);
                }
            }

            using (StringReader reader = new StringReader(Properties.Resources.PSM_built_in_types))
            {
                XDocument xbuiltInTypes = XDocument.Load(reader);
                IEnumerable<XElement> types =
                    xbuiltInTypes.Element(psmBuiltInTypesNS + "Project").Element(psmBuiltInTypesNS + "AttributeTypes").
                        Elements(psmBuiltInTypesNS + "AttributeType");

                Dictionary<string, Guid> typeDict = new Dictionary<string, Guid>();
                foreach (XElement typeElement in types)
                {
                    typeDict[typeElement.Attribute("Name").Value] = Guid.Parse(typeElement.Attribute("ID").Value);
                }

                SerializationContext context = new SerializationContext();
                context.TypeDict = typeDict;

                foreach (XElement type in types)
                {
                    if (type.Element(context.ExolutioNS + "Operations") != null)
                    {
                        foreach (XElement operationElement in type.Element(context.ExolutioNS + "Operations").Elements())
                        {
                            string resultTypeID = operationElement.Attribute("ResultType").Value;
                            if (resultTypeID.StartsWith("{"))
                            {
                                operationElement.Attribute("ResultType").Value = typeDict[resultTypeID.Substring(1, resultTypeID.Length - 2)].ToString();
                            }
                            foreach (XElement parameterElement in operationElement.Element(context.ExolutioNS + "Parameters").Elements())
                            {
                                string parType = parameterElement.Attribute("Type").Value;
                                if (parType.StartsWith("{"))
                                {
                                    parameterElement.Attribute("Type").Value = typeDict[parType.Substring(1, parType.Length - 2)].ToString();
                                }
                            }
                        }
                    }
                    AttributeType attributeType = new AttributeType(this, Guid.Empty);
                    attributeType.Deserialize(type, context);
                    foreach (ModelOperation modelOperation in attributeType.Operations)
                    {
                        modelOperation.DeclaringType = attributeType;
                    }
                    psmBuiltInTypes.Add(attributeType);
                }
            }
        }

		private void LoadBuiltInStereotypes()
		{
			Stereotype key = new Stereotype(this, Guid.Parse(@"48311640-1bc1-488c-ac40-31e1c0b47d2e"));
			key.Name = "Key";
			builtInStereotypes.Add(key);

			Stereotype collection = new Stereotype(this, Guid.Parse(@"b10292c6-430c-4b78-abac-9b5800790bd8"));
			collection.Name = "Collection";
			builtInStereotypes.Add(collection);

			Stereotype collectionRef = new Stereotype(this, Guid.Parse(@"81da98bc-3a31-439a-bd7d-760f677cb330"));
			collectionRef.Name = "CollectionRef";
			builtInStereotypes.Add(collectionRef);

			Stereotype largeText = new Stereotype(this, Guid.Parse(@"d45f347c-4721-489a-84bf-1e8ca831ef8f"));
			largeText.Name = "LargeText";
			builtInStereotypes.Add(largeText);

			Stereotype htmlText = new Stereotype(this, Guid.Parse(@"a68fa1d6-2c99-4ca0-839b-04caee013afb"));
			htmlText.Name = "HTMLText";
			builtInStereotypes.Add(htmlText);

			Stereotype ocl = new Stereotype(this, Guid.Parse(@"b96618e7-b948-4bc5-8a92-4669382184ff"));
			ocl.Name = "OCL";
			builtInStereotypes.Add(ocl);
		}

        Project IExolutioSerializable.Project
        {
            get { return this; }
        }

        #region Project versions

        private UndirectCollection<ProjectVersion> projectVersions;

        public UndirectCollection<ProjectVersion> ProjectVersions
        {
            get
            {
                if (!UsesVersioning)
                {
                    throw new ExolutioModelException(Exceptions.Project_ProjectVersions_Can_not_access_ProjectVersions_when_UsesVersioning_is_set_to_false);
                }
                return projectVersions;
            }
            private set { projectVersions = value; }
        }

        public ProjectVersion GetProjectVersion(Version version)
        {
            return ProjectVersions.FirstOrDefault(pv => pv.Version == version);
        }

        #endregion

        private bool usesVersioning;
        public bool UsesVersioning
        {
            get { return usesVersioning; }
            private set { usesVersioning = value; NotifyPropertyChanged("UsesVersioning"); }
        }

        //private string name;
        public string Name
        {
            get
            {
                return /*!String.IsNullOrEmpty(name) ? name : */(ProjectFile != null ? Path.GetFileNameWithoutExtension(ProjectFile.FullName) : "Untitled");
            }
            //set
            //{
            //    //name = value; 
            //    NotifyPropertyChanged("Name");
            //}
        }


        private Guid singleVersionGuid = Guid.Empty;
        //private ProjectVersion singleVersion;

        /// <summary>
        /// Returns the only oldVersion present in the project when versioning is not used
        /// </summary>
        public ProjectVersion SingleVersion
        {
            get
            {
                if (UsesVersioning)
                {
                    throw new ExolutioModelException(Exceptions.Project_SingleVersion_Can_not_access_SingleVersion_when_UsesVersioning_is_set_to_true);
                }
                return singleVersionGuid != Guid.Empty ? TranslateComponent<ProjectVersion>(singleVersionGuid) : null;
            }
            set
            {
                singleVersionGuid = value != null ? value : Guid.Empty;
                NotifyPropertyChanged("SingleVersion");
            }
        }

        private FileInfo projectFile;
        public FileInfo ProjectFile
        {
            get { return projectFile; }
            set
            {
                projectFile = value;
                NotifyPropertyChanged("ProjectFile");
                NotifyPropertyChanged("Name");
            }
        }

        private bool hasUnsavedChanges;
        public bool HasUnsavedChanges
        {
            get { return hasUnsavedChanges; }
            set
            {
                hasUnsavedChanges = value;
                NotifyPropertyChanged("HasUnsavedChanges");
            }
        }

        public ProjectVersion LatestVersion
        {
            get { return UsesVersioning ? ProjectVersions.Last() : SingleVersion; }
        }

        public void InitNewEmptyProject(bool createPIMSchemaInSingleVersion = true)
        {
            UsesVersioning = false;
            HasUnsavedChanges = false;

            ProjectFile = null;

            SingleVersion = new ProjectVersion(this);
            if (createPIMSchemaInSingleVersion)
            {
                SingleVersion.PIMSchema = new PIMSchema(this);
            }
        }

        /// <summary>
        /// Creates the version manager and delcares the existing content as the virst version of the project. 
        /// </summary>
        public void StartVersioning(int firstVersionNumber = 1, string firstVersionLabel = "v1")
        {
            VersionManager = new VersionManager(this);

            Version firstVersion = new Version(this) { Label = firstVersionLabel, Number = firstVersionNumber };
            VersionManager.Versions.Add(firstVersion);
            ProjectVersion firstProjectVersion = this.SingleVersion;
            firstProjectVersion.Version = firstVersion;
            this.SingleVersion = null;
            /* direct property base assignment is used to prevent 
             * methods reacting to the change of "UsesVersioning" 
             * property during an incoherent state */
            usesVersioning = true;
            ProjectVersions.Add(firstProjectVersion);
            
            foreach (IVersionedItem versionedItem in ModelIterator.GetAllModelItems(firstProjectVersion).OfType<IVersionedItem>())
            {
                firstVersion.Items.Add(versionedItem);
                VersionManager.AddVersionedItem(versionedItem);
            }

            /* Now listeners to the change of "UsesVersioning" can be notified */
            UsesVersioning = true;
        }

        public VersionManager VersionManager { get; set; }

        private UndirectCollection<AttributeType> pimBuiltInTypes;

        public UndirectCollection<AttributeType> PIMBuiltInTypes
        {
            get
            {
                
                return pimBuiltInTypes;
            }
        }

        private UndirectCollection<AttributeType> psmBuiltInTypes;

        public UndirectCollection<AttributeType> PSMBuiltInTypes
        {
            get
            {
                return psmBuiltInTypes;
            }
        }

	    private UndirectCollection<Stereotype> builtInStereotypes;
	    public UndirectCollection<Stereotype> BuiltInStereotypes
	    {
		    get
		    {
				return builtInStereotypes;
		    }
	    }

        #region IExolutioSerializable Members

        public void Deserialize(XElement parentNode, SerializationContext context)
        {
            parentNode = (XElement)context.Document.Element(context.ExolutioNS + "Project");
            UsesVersioning = SerializationContext.DecodeBool(parentNode.Attribute("UsesVersioning").Value);
            if (UsesVersioning)
            {
                VersionManager = new VersionManager(this);
                VersionManager.Loading = true; 
                this.DeserializeWrappedCollection("Versions", VersionManager.Versions, Version.CreateInstance, parentNode, context);
                this.DeserializeWrappedCollection("ProjectVersions", ProjectVersions, ProjectVersion.CreateInstance, parentNode, context);
                XElement versionLinksElement = parentNode.Element(context.ExolutioNS + "VersionLinks");
                if (versionLinksElement != null)
                {
                    VersionManager.DeserializeVersionLinks(versionLinksElement, context);
                }
                VersionManager.Loading = false; 
            }
            else
            {
                ProjectVersion objSingleVersion = new ProjectVersion(this, Guid.Empty);
                objSingleVersion.DeserializeFromChildElement("SingleVersion", parentNode, context);
                SingleVersion = objSingleVersion;
            }

            //XAttribute nameAttribute = parentNode.Attribute("Name");
            //if (nameAttribute != null)
            //{
            //    this.Name = nameAttribute.Value;
            //}
        }

        public void Serialize(XElement parentNode, SerializationContext context)
        {
            XElement projectElement = new XElement(context.ExolutioNS + "Project");
            projectElement.Add(new XAttribute(XNamespace.Xmlns + SerializationContext.ExolutioPrefix, context.ExolutioNS.NamespaceName));

            // version attribute
            XAttribute versionAttribute = new XAttribute("Version", "3.0");
            projectElement.Add(versionAttribute);

            // uses versioning attribute
            XAttribute usesVersioningAttribute = new XAttribute("UsesVersioning", SerializationContext.EncodeValue(UsesVersioning));
            projectElement.Add(usesVersioningAttribute);

            if (!String.IsNullOrEmpty(Name))
            {
                XAttribute nameAttribute = new XAttribute("Name", SerializationContext.EncodeValue(Name));
                projectElement.Add(nameAttribute);
            }

            if (UsesVersioning)
            {
                // versions
                this.WrapAndSerializeCollection("Versions", "Version", VersionManager.Versions, projectElement, context);
                // project versions
                this.WrapAndSerializeCollection("ProjectVersions", "ProjectVersion", ProjectVersions, projectElement, context);
                XElement versionLinksElement = new XElement(context.ExolutioNS + "VersionLinks");
                VersionManager.SerializeVersionLinks(versionLinksElement, context);
                projectElement.Add(versionLinksElement);
            }
            else
            {
                this.SerializeToChildElement("SingleVersion", SingleVersion, projectElement, context);
            }

            context.Document.Add(projectElement);
        }
        #endregion

        public readonly ComponentMappingDictionary mappingDictionary = new ComponentMappingDictionary();

        public T TranslateComponent<T>(Guid componentId)
            where T : ExolutioObject
        {
            return mappingDictionary.FindComponent<T>(componentId);
        }

        public ExolutioObject TranslateComponent(Guid componentId)
        {
            return mappingDictionary.FindComponent(componentId);
        }

        public bool TryTranslateObject(Guid componentId, out ExolutioObject exolutioObject)
        {
            return mappingDictionary.TryGetValue(componentId, out exolutioObject);
        }

        public bool VerifyComponentType<T>(Guid componentId)
            where T : ExolutioObject
        {
            return mappingDictionary.VerifyComponentType<T>(componentId);
        }

        public ReadOnlyCollection<T> TranslateComponentCollection<T>(IEnumerable<Guid> guids) where T : ExolutioObject
        {
            return guids.Select(TranslateComponent<T>).ToList().AsReadOnly();
        }

        #region Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler tmp = PropertyChanged;
            if (tmp != null)
            {
                tmp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}