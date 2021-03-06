﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exolutio.Model.OCL.Types;

namespace Exolutio.Model.OCL {

    /// <summary>
    /// 
    /// </summary>
    public abstract class Environment {

        public Exolutio.Model.OCL.Environment Parent {
            get;
            private set;
        }

        /// <summary>
        /// Find a named element in the current environment, not in its parents, based on a single name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="acceptClassifiersOnly"> </param>
        /// <returns></returns>
        public abstract IModelElement LookupLocal(string name, bool acceptClassifiersOnly = false);

        public abstract IModelElement LookupNamespaceLocal(string name);

        /// <summary>
        /// Find a named element in the current environment or recursively in its parent environment, based on a single name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract IModelElement Lookup(string name);

        /// <summary>
        /// Find a named element in the current environment or recursively in its parent environment, based on a path name.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="acceptClassifiersOnly"> </param>
        /// <returns></returns>
        public abstract IModelElement LookupPathName(IEnumerable<string> path, bool acceptClassifiersOnly = false);

        /// <summary>
        /// Lookup a given association end name of an implicitly named element in the current environment, including its parents.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract ImplicitPropertyData LookupImplicitAttribute(string name);
        
        /// <summary>
        /// Lookup an operation of an implicitly named element with given name and parameter types in the current environment,
        /// including its parents.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public abstract ImplicitOperationData LookupImplicitOperation(string name, IEnumerable<Classifier> parameters);

        /// <summary>
        /// Add all elements in the namespace to the environment.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        public Environment CreateFromNestedNamespace(Namespace ns) {
            NamespaceEnvironment nsEnv = new NamespaceEnvironment(ns);
            nsEnv.Parent = this;
            return nsEnv;
        }

        public Environment CreateFromClassifier(Classifier cl,VariableDeclaration varDecl) {
            ClassifierEnvironment clEnv = new ClassifierEnvironment(cl,varDecl);
            clEnv.Parent = this;
            return clEnv;
        }
        
        /// <summary>
        /// Add a new named element to the environment.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="impl"></param>
        /// <returns></returns>
        public Environment AddElement(string name, IModelElement type,VariableDeclaration source, bool impl) {
            if (LookupLocal(name) != null) {
                throw new Exception(string.Format(CompilerErrors.VariableAlreadyExisted, name));
            }

            Environment env = new AddElementEnvironment(this,name, type,source, impl);
            env.Parent = this;
            return env;
        }
    }

    public class ErrorEnvironment : Environment
    {
        private ErrorEnvironment()
        {
        }

        public static ErrorEnvironment Instance = new ErrorEnvironment();

        public override IModelElement LookupLocal(string name, bool acceptClassifiersOnly = false)
        {
            return null;
        }

        public override IModelElement LookupNamespaceLocal(string name)
        {
            return null;
        }

        public override IModelElement Lookup(string name)
        {
            return null;
        }

        public override IModelElement LookupPathName(IEnumerable<string> path, bool acceptClassifiersOnly = false)
        {
            return null;
        }

        public override ImplicitPropertyData LookupImplicitAttribute(string name)
        {
            return null;
        }

        public override ImplicitOperationData LookupImplicitOperation(string name, IEnumerable<Classifier> parameters)
        {
            return null;
        }
    }
    
    public class NamespaceEnvironment : Environment {
        private Namespace internalNamespace;

        public NamespaceEnvironment(Namespace ns) {
            internalNamespace = ns;
        }

        public override IModelElement LookupLocal(string name, bool acceptClassifiersOnly = false) {
            if (internalNamespace.NestedClassifier.ContainsKey(name)) {
                return internalNamespace.NestedClassifier[name];
            }
            if (!acceptClassifiersOnly && internalNamespace.NestedNamespace.ContainsKey(name)) {
                return internalNamespace.NestedNamespace[name];
            }
            return null;
        }

        public override IModelElement Lookup(string name) {
            IModelElement lookupLocalResult = LookupLocal(name);
            if (lookupLocalResult != null) {
                return lookupLocalResult;
            }

            if (Parent != null) {
                return Parent.Lookup(name);
            }
            return null;

        }

        public override IModelElement LookupNamespaceLocal(string name) {
            if (internalNamespace.NestedNamespace.ContainsKey(name)) {
                return internalNamespace.NestedNamespace[name];
            }
            return null;
        }

        public override IModelElement LookupPathName(IEnumerable<string> path, bool acceptClassifiersOnly = false) {
            int pathLength = path.Count();
            if (pathLength == 0) {
                return null;
            }

            string first = path.First();

            if (pathLength == 1) {
                // musi byt local
                return LookupLocal(first, acceptClassifiersOnly);
            }
            else {
                var forwardLookup = ForwardLookupPathName(path, internalNamespace);
                if (forwardLookup != null) {
                    return forwardLookup;
                }


                if (Parent != null) {
                    return Parent.LookupPathName(path, acceptClassifiersOnly);
                }
                else {
                    return null;
                }

            }
        }

        private IModelElement ForwardLookupPathName(IEnumerable<string> path, Namespace ns) {
            int pathLength = path.Count();
            if (pathLength == 0) {
                return null;
            }

            string first = path.First();
            if (pathLength == 1) {
                return ns.NestedClassifier.ContainsKey(first) ? ns.NestedNamespace[first] : null;
            }
            else {

                if (ns.NestedNamespace.ContainsKey(first)) {
                    return ForwardLookupPathName(path.Skip(1), ns.NestedNamespace[first]);
                }
                else {
                    return null;
                }
            }
        }

        public override ImplicitPropertyData LookupImplicitAttribute(string name) {
            if (Parent != null) {
                return Parent.LookupImplicitAttribute(name);
            }

            return null;
        }

        public override ImplicitOperationData LookupImplicitOperation(string name, IEnumerable<Classifier> parameters) {
            if (Parent != null) {
                return Parent.LookupImplicitOperation(name,parameters);
            }

            return null;
        }
    }

    public class ClassifierEnvironment : Environment {
        private Classifier internalClassifier;
        private VariableDeclaration internalVariableDec;

        public ClassifierEnvironment(Classifier classifier,VariableDeclaration varDecl) {
            this.internalClassifier = classifier;
            this.internalVariableDec = varDecl;
        }

        public override IModelElement LookupLocal(string name, bool acceptClassifiersOnly = false) {
            // nekontroluje se vnitrni tridy,??operace??
            if (acceptClassifiersOnly)
                return null;
            return internalClassifier.LookupProperty(name);
        }

        public override IModelElement LookupNamespaceLocal(string name) {
            throw new NotImplementedException();
        }

        public override IModelElement Lookup(string name) {
            IModelElement el = LookupLocal(name);
            if (el != null) {
                return el;
            }
            if (Parent != null) {
                return this.Parent.Lookup(name);
            }
            return null;
        }

        public override IModelElement LookupPathName(IEnumerable<string> path, bool acceptClassifiersOnly = false) {
            // Neuplna implementace !!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (path.Count() == 1) {
                var el = LookupLocal(path.First(), acceptClassifiersOnly);
                if (el != null) {
                    return el;
                }
            }
            if (Parent != null) {
                return Parent.LookupPathName(path, acceptClassifiersOnly);
            }
            return null;
        }

        public override ImplicitPropertyData LookupImplicitAttribute(string name) {
            Property prop = internalClassifier.LookupProperty(name);
            if (prop != null) {
                return new ImplicitPropertyData(prop,internalVariableDec);
            }
            if (Parent != null) {
                return Parent.LookupImplicitAttribute(name);
            }
            return null;
        }

        public override ImplicitOperationData LookupImplicitOperation(string name, IEnumerable<Classifier> parameters) {
            Operation op = internalClassifier.LookupOperation(name, parameters);
            if (op != null) {
                return new ImplicitOperationData(op, internalVariableDec);
            }
            if (Parent != null) {
                return Parent.LookupImplicitOperation(name, parameters);
            }
            return null;
        }
    }

    public class AddElementEnvironment : Environment {
        private string Name;
        private IModelElement Type;
        private VariableDeclaration Source;
        private bool IsImplicit;
        private Environment RootEnviroment;

        public AddElementEnvironment(Environment rootEnv, string name, IModelElement type, VariableDeclaration source, bool impl) {
            this.RootEnviroment = rootEnv;
            this.Name = name;
            this.Type = type;
            this.Source = source;
            this.IsImplicit = impl;
        }

        public override IModelElement LookupLocal(string name, bool acceptClassifiersOnly = false) {
            if (name == this.Name && !acceptClassifiersOnly) {
                return Type;
            }
            else {
                return RootEnviroment.LookupLocal(name, acceptClassifiersOnly);
            }
        }

        public override IModelElement LookupNamespaceLocal(string name) {
            if (name == this.Name && Type is Namespace) {
                return Type;
            }
            else {
                return RootEnviroment.LookupNamespaceLocal(name);
            }
        }

        public override IModelElement Lookup(string name) {
            if (name == this.Name) {
                return Source;
            }
            else {
                return RootEnviroment.Lookup(name);
            }
        }

        public override IModelElement LookupPathName(IEnumerable<string> path, bool acceptClassifiersOnly = false) {
            // AddElemnt neni soucasti konkretniho namespace => nebudeme hledat path v name
            return RootEnviroment.LookupPathName(path, acceptClassifiersOnly);
        }

        public override ImplicitPropertyData LookupImplicitAttribute(string name) {

            if (this.Name == name && IsImplicit && Type is Property) {
                return new ImplicitPropertyData( Type as Property, Source);
            }

            if (RootEnviroment != null) {
                return RootEnviroment.LookupImplicitAttribute(name);
            }

            if (Parent != null) {
                return Parent.LookupImplicitAttribute(name);
            }

            return null;
        }

        public override ImplicitOperationData LookupImplicitOperation(string name, IEnumerable<Classifier> parameters) {
            if (this.Name == name && IsImplicit && Type is Operation) {
                Operation thisOp = Type as Operation;
                if (thisOp.Parametrs.HasMatchingSignature(parameters)) {
                    return new ImplicitOperationData( thisOp, Source);
                }
            }

            if (RootEnviroment != null) {
                return RootEnviroment.LookupImplicitOperation(name,parameters);
            }

            if (Parent != null) {
                return Parent.LookupImplicitOperation(name,parameters);
            }

            return null;
        }
    }

    public class ImplicitOperationData {
        public ImplicitOperationData(Operation oper, VariableDeclaration source) {
            this.Operation = oper;
            this.Source = source;
        }

        public Operation Operation {
            get;
            protected set;
        }

        public VariableDeclaration Source {
            get;
            protected set;
        }
    }

    public class ImplicitPropertyData {
        public ImplicitPropertyData(Property property, VariableDeclaration source) {
            this.Property = property;
            this.Source = source;
        }

        public Property Property {
            get;
            protected set;
        }

        public VariableDeclaration Source {
            get;
            protected set;
        }
    }
}
