﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Exolutio.Model.OCL.AST;
using Exolutio.Model.OCL.Bridge;
using Exolutio.Model.OCL.Types;
using Exolutio.Model.PSM;
using Exolutio.SupportingClasses;

namespace Exolutio.Model.OCL.ConstraintConversion
{
    public class PSMPath : ICloneable
    {
        public class PathContext
        {
            private readonly List<LoopExp> loopStack = new List<LoopExp>();
            public List<LoopExp> LoopStack
            {
                get { return loopStack; }
            }

            public PSMBridge Bridge { get; set; }

            public IConstraintsContext ConstraintContext { get; set; }

            public VariableNamer VariableNamer { get; set; }
        }

        public PathContext Context { get; private set; }

        private readonly List<OclExpression> subExpressions = new List<OclExpression>();
        public List<OclExpression> SubExpressions
        {
            get { return subExpressions; }
        }

        public TupleLiteralToXPathCallbackHandler TupleLiteralToXPathCallback { get; set; }

        public ClassLiteralToXPathCallbackHandler ClassLiteralToXPathCallback { get; set; }

        public GenericExpressionToXPathCallbackHandler GenericExpressionToXPathCallback { get; set; }

        public GetRelativeXPathEvolutionCallback GetRelativeXPathEvolutionCallback { get; set; }

        public PSMPath()
        {
            Context = new PathContext();
        }

        private readonly List<PSMPathStep> steps = new List<PSMPathStep>();

        public VariableExp StartingVariableExp
        {
            get { return steps[0] is PSMPathVariableStep ? ((PSMPathVariableStep)steps[0]).VariableExp : null; }
        }

        public PSMClass StartingClass
        {
            get { return (PSMClass)StartingVariableExp.Type.Tag; }
        }

        public bool StartsInContext
        {
            get { return StartingVariableExp.referredVariable.IsContextVariable; }
        }

        public string StartingVariableName
        {
            get { return StartingVariableExp.referredVariable.Name; }
        }

        public List<PSMPathStep> Steps
        {
            get { return steps; }
        }

        public PSMClass LastClass
        {
            get
            {
                PSMPathStep step = Steps.Last(s => s is PSMPathVariableStep || s is PSMPathAssociationStep);
                if (step is PSMPathVariableStep)
                    return ((PSMPathVariableStep)step).VariableType;
                else
                    return (PSMClass) ((PSMPathAssociationStep) step).To;
            }
        }

        /// <summary>
        /// Returns true when the path contains only association and attribute steps 
        /// and all associations are traversed from the top to the bottom. 
        /// </summary>
        public bool IsDownwards
        {
            get 
            {
                foreach (PSMPathStep psmPathStep in Steps.Skip(1))
                {
                    if (psmPathStep is PSMPathAssociationStep)
                    {
                        if (((PSMPathAssociationStep)psmPathStep).IsUp)
                        {
                            return false; 
                        }
                    }
                    else if (psmPathStep is PSMPathAttributeStep)
                    {
                        continue;
                    }
                    else
                    {
                        return false; 
                    }
                }
                return true; 
            }
        }

        public override string ToString()
        {
            return Steps.ConcatWithSeparator(@".");
        }

        public object Clone()
        {
            PSMPath clone = new PSMPath();
            clone.Context = Context;
            foreach (PSMPathStep navigationStep in Steps)
            {
                PSMPathStep stepClone = (PSMPathStep)navigationStep.Clone(clone);
                clone.Steps.Add(stepClone);
            }
            return clone;
        }

        /// <summary>
        /// Return PSMPath as XPath
        /// </summary>
        /// <param name="withoutFirstStep">The first step is ommited</param>
        /// <param name="delayFirstVariableStep">The first step is not converted to XPath, it is only 
        /// returned as a format string placeholeder '{0}'. </param>
        /// <returns></returns>
        public string ToXPath(bool withoutFirstStep = false, bool delayFirstVariableStep = false)
        {
            List<PSMPathStep> noEvolutionSteps;
            if (Steps.Any(s => s is PSMEvolutionPrevStep))
            {
                noEvolutionSteps = Steps.SkipWhile(s => !(s is PSMEvolutionPrevStep)).ToList();
            }
            else
            {
                noEvolutionSteps = Steps;
            }

            if (withoutFirstStep)
            {
                return noEvolutionSteps.SkipWhile(s => s == noEvolutionSteps[0]).Select(s => s.ToXPath()).ConcatWithSeparator(String.Empty);
            }

            if (delayFirstVariableStep && StartingVariableExp != null && !(noEvolutionSteps.First() is PSMEvolutionPrevStep))
            {
                return noEvolutionSteps.Select(s => s == noEvolutionSteps.First() ? @"{0}" : s.ToXPath()).ConcatWithSeparator(String.Empty);
            }

            return noEvolutionSteps.Select(s => s.ToXPath()).ConcatWithSeparator(String.Empty);
        }
    }
    
    public delegate string TupleLiteralToXPathCallbackHandler(TupleLiteralExp tupleLiteral, List<OclExpression> subExpressions);

    public delegate string ClassLiteralToXPathCallbackHandler(ClassLiteralExp tupleLiteral, List<OclExpression> subExpressions);

    public delegate string GenericExpressionToXPathCallbackHandler(OclExpression oclExpression, List<OclExpression> oclExpressions);
    
    public delegate string GetRelativeXPathEvolutionCallback(PSMComponent node);

    public abstract class PSMPathStep
    {
        public abstract object Clone(PSMPath newContainingPath);

        public abstract string ToXPath();

        public PSMPath Path { get; private set; }

        protected PSMPathStep(PSMPath path)
        {
            Path = path;
        }
    }

    public interface IPSMPathStepWithCardinality
    {
        uint Lower { get; }
        UnlimitedInt Upper { get; }
        string ToXPath();
    }

    class PSMPathVariableStep : PSMPathStep
    {
        public PSMPathVariableStep(PSMPath path) : base(path)
        {
        }

        public PSMClass VariableType
        {
            get { return (PSMClass)Variable.PropertyType.Tag; }
        }
        public VariableExp VariableExp { get; set; }
        public VariableDeclaration Variable
        {
            get { return VariableExp.referredVariable; }
        }

        public override string ToString()
        {
            return Variable.Name;
        }

        public override object Clone(PSMPath newContainingPath)
        {
            return new PSMPathVariableStep(newContainingPath) { VariableExp = this.VariableExp };
        }

        public override string ToXPath()
        {
            return string.Format(@"${0}", Variable.Name);
        }
    }

    class PSMPathAttributeStep : PSMPathStep, IPSMPathStepWithCardinality
    {
        public PSMPathAttributeStep(PSMPath path) : base(path)
        {
        }

        public PSMAttribute Attribute { get; set; }
        public PSMClass Class { get { return Attribute.PSMClass; } }
        public override string ToString()
        {
            return Attribute.Name;
        }

        public uint Lower
        {
            get { return Attribute.Lower; }
        }

        public UnlimitedInt Upper
        {
            get { return Attribute.Upper; }
        }

        public override object Clone(PSMPath newContainingPath)
        {
            return new PSMPathAttributeStep(newContainingPath) { Attribute = this.Attribute };
        }

        public override string ToXPath()
        {
            if (Attribute.Element)
            {
                return string.Format(@"/{0}", Attribute.Name);
            }
            else
            {
                return string.Format(@"/@{0}", Attribute.Name);
            }
        }
    }

    class TupleLiteralStep: PSMPathStep
    {
        public TupleLiteralStep(PSMPath path) : base(path)
        {
        }

        public TupleLiteralExp TupleExpresion { get; set; }

        public override object Clone(PSMPath newContainingPath)
        {
            return new TupleLiteralStep(newContainingPath) { TupleExpresion = this.TupleExpresion };
        }

        public override string ToXPath()
        {
            string tuple = Path.TupleLiteralToXPathCallback(this.TupleExpresion, Path.SubExpressions);
            return tuple;
        }
    }

    class ClassLiteralStep : PSMPathStep
    {
        public ClassLiteralStep(PSMPath path)
            : base(path)
        {
        }

        public ClassLiteralExp ClassLiteralExpression { get; set; }

        public override object Clone(PSMPath newContainingPath)
        {
            return new ClassLiteralStep(newContainingPath) { ClassLiteralExpression = this.ClassLiteralExpression };
        }

        public override string ToXPath()
        {
            string callbackResult = Path.ClassLiteralToXPathCallback(this.ClassLiteralExpression, Path.SubExpressions);
            return callbackResult;
        }
    }

    class GeneralSubexpressionStep : PSMPathStep
    {
        public GeneralSubexpressionStep(PSMPath path)
            : base(path)
        {
        }

        public OclExpression OclExpression { get; set; }

        public override object Clone(PSMPath newContainingPath)
        {
            return new GeneralSubexpressionStep(newContainingPath) { OclExpression = this.OclExpression };
        }

        public override string ToXPath()
        {
            string expression = Path.GenericExpressionToXPathCallback(this.OclExpression, Path.SubExpressions);
            return expression;
        }
    }

    class TuplePartStep : PSMPathStep
    {
        public TuplePartStep(PSMPath path) : base(path)
        {
        }

        public TupleLiteralExp TupleExpresion { get; set; }

        public TupleLiteralPart TuplePart { get; set; }

        public override object Clone(PSMPath newContainingPath)
        {
            return new TuplePartStep(newContainingPath) {TupleExpresion = this.TupleExpresion, TuplePart = this.TuplePart };
        }

        public override string ToXPath()
        {
            return string.Format(@"('{0}')", TuplePart.Attribute.Name);
        }
    }

    class PSMPathAssociationStep : PSMPathStep, IPSMPathStepWithCardinality
    {
        public PSMPathAssociationStep(PSMPath path) : base(path)
        {
        }

        public PSMAssociationMember From { get; set; }
        public PSMAssociation Association { get; set; }
        public PSMAssociationMember To { get; set; }

        public uint Lower
        {
            get { return Association.Lower; }
        }

        public UnlimitedInt Upper
        {
            get { return Association.Upper; }
        }

        public bool IsUp { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Association.Name))
                return Association.Name;
            if (!string.IsNullOrEmpty(To.Name))
                return To.Name;
            if (From.ParentAssociation == Association)
                return @"parent";
            else 
                return string.Format(@"child_{0}", Association.Parent.ChildPSMAssociations.IndexOf(Association));
        }

        public override object Clone(PSMPath newContainingPath)
        {
            return new PSMPathAssociationStep(newContainingPath) { Association = this.Association, From = this.From, To = this.To, IsUp = this.IsUp};
        }

        public override string ToXPath()
        {
            if (Association.IsNamed)
            {
                if (IsUp)
                {
                    bool useFullParentStep = false; 
                    if (From is PSMClass)
                    {
                        useFullParentStep = ((PSMClass) From).GetIncomingNonTreeAssociations().Count() > 0;
                    }
                    if (useFullParentStep)
                    {
                        //HACK : this works for tree-like schemas, could cause problems where non-tree features appear (inheritance, SR, non-tree associations)
                        IEnumerable<PSMAssociation> associations = ModelIterator.GetAncestorAssociations(To).Where(a => a.IsNamed);
                        if (associations.Count() > 0)
                            return string.Format(@"/parent::{0}", associations.First().Name);
                    }
                    
                    return string.Format(@"/..");                    
                }
                else
                {
                    return string.Format(@"/{0}", Association.Name);
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }

    class PSMEvolutionPrevStep : PSMPathStep
    {
        public PSMAssociationMember PSMAssociationMemberTargetVersion { get; set; }

        public PSMAssociationMember PSMAssociationMemberSourceVersion { get; set; }

        public PSMAttribute StepAttributeNewVersion { get; set; }

        public PSMAssociation StepAssociationNewVersion { get; set; }

        public PSMEvolutionPrevStep(PSMPath path) : base(path)
        {
        }

        public override object Clone(PSMPath newContainingPath)
        {
            return new PSMEvolutionPrevStep(newContainingPath)
                {
                    PSMAssociationMemberTargetVersion = this.PSMAssociationMemberTargetVersion,
                    PSMAssociationMemberSourceVersion = this.PSMAssociationMemberSourceVersion,
                    StepAssociationNewVersion = this.StepAssociationNewVersion,
                    StepAttributeNewVersion = this.StepAttributeNewVersion
                };
        }

        public override string ToXPath()
        {
            if (StepAttributeNewVersion != null)
            {
                string relativeXPath = Path.GetRelativeXPathEvolutionCallback(StepAttributeNewVersion);
                return relativeXPath; 
                //return string.Format(@"$ci[name()='{0}']", StepAttributeNewVersion.Name);
            }
            else 
            {
                string relativeXPath = Path.GetRelativeXPathEvolutionCallback(StepAssociationNewVersion.Child);
                return relativeXPath;    
            }
        }
    }

    public static class PSMPathBuilder
    {
        private static bool IsEvolutionPrevStep(PropertyCallExp node)
        {
            OperationCallExp operationCallExp = node.Source as OperationCallExp;
            if (operationCallExp != null)
            {
                if (operationCallExp.ReferredOperation.Tag is PrevOperationTag)
                {
                    return true;
                }
            }
            return false;
        }

        public static PSMPath BuildPSMPath(PropertyCallExp node, IConstraintsContext constraintsContext, VariableNamer variableNamer, BuildPSMPathParams buildPsmPathParams)
        {
            PSMPath path = new PSMPath();
            path.TupleLiteralToXPathCallback = buildPsmPathParams.TupleLiteralToXPathCallback;
            path.GenericExpressionToXPathCallback = buildPsmPathParams.GenericExpressionToXPathCallback;
            path.GetRelativeXPathEvolutionCallback = buildPsmPathParams.GetRelativeXPathEvolutionCallback;
            path.Context.ConstraintContext = constraintsContext;
            path.Context.VariableNamer = variableNamer;

            OclExpression s;

            if (buildPsmPathParams.Evolution && IsEvolutionPrevStep(node))
            {
                PSMEvolutionPrevStep prevStep = new PSMEvolutionPrevStep(path);
                if (node.ReferredProperty is PSMBridgeAttribute)
                {
                    prevStep.StepAttributeNewVersion = ((PSMBridgeAttribute) node.ReferredProperty).SourceAttribute;
                }
                else if (node.ReferredProperty is PSMBridgeAssociation)
                {
                    prevStep.StepAssociationNewVersion = ((PSMBridgeAssociation)node.ReferredProperty).SourceAsscociation;
                }
                else
                {
                    throw new InvalidOperationException();
                }
                PrevOperationTag tag = (PrevOperationTag) ((OperationCallExp) node.Source).ReferredOperation.Tag;
                prevStep.PSMAssociationMemberTargetVersion = (PSMAssociationMember) tag.TargetVersionClassifier.Tag;
                prevStep.PSMAssociationMemberSourceVersion = (PSMAssociationMember) tag.SourceVersionClassifier.Tag;
                path.Steps.Add(prevStep);
                OclExpression prevSource = ((OperationCallExp) node.Source).Source;
                if (prevSource is VariableExp && ((VariableExp)prevSource).referredVariable == constraintsContext.Self)
                {
                    s = null;
                }
                else
                {
                    s = prevSource;
                }
            }
            else if (node.ReferredProperty is PSMBridgeAssociation)
            {
                s = node;
            }
            else if (node.Source is TupleLiteralExp)
            {
                TuplePartStep tuplePartStep = new TuplePartStep(path) { TupleExpresion = (TupleLiteralExp) node.Source };
                tuplePartStep.TuplePart = tuplePartStep.TupleExpresion.Parts[node.ReferredProperty.Name];
                path.Steps.Add(tuplePartStep);
                s = node.Source;
            }
            else 
            {
                PSMAttribute a = (PSMAttribute)node.ReferredProperty.Tag;
                PSMPathAttributeStep pathAttributeStep = new PSMPathAttributeStep(path) { Attribute = a };
                path.Steps.Add(pathAttributeStep);
                s = node.Source;
            }

            while (s is PropertyCallExp)
            {
                PSMPathAssociationStep step = new PSMPathAssociationStep(path);
                var sp = ((PropertyCallExp)s);
                // HACK: WFJT turn class tag into association tag
                PSMBridgeAssociation bridgeAssociation = (PSMBridgeAssociation) sp.ReferredProperty;
                step.Association = bridgeAssociation.SourceAsscociation;
                if (bridgeAssociation.Direction == PSMBridgeAssociation.AssociationDirection.Down)
                {
                    step.From = bridgeAssociation.SourceAsscociation.Parent;
                    step.To = bridgeAssociation.SourceAsscociation.Child;
                    step.IsUp = false;
                }
                else
                {
                    step.From = bridgeAssociation.SourceAsscociation.Child;
                    step.To = bridgeAssociation.SourceAsscociation.Parent;
                    step.IsUp = true;
                }
                path.Steps.Insert(0, step);
                s = sp.Source;
            }

            if (s is VariableExp)
            {
                VariableExp variableExp = (VariableExp) s;
                PSMPathVariableStep pathVariableStep = new PSMPathVariableStep(path) {VariableExp = variableExp};
                if (string.IsNullOrEmpty(variableExp.referredVariable.Name))
                {
                    variableExp.referredVariable.Name =
                        path.Context.VariableNamer.GetName(variableExp.referredVariable.PropertyType);
                }
                path.Steps.Insert(0, pathVariableStep);
            }
            else if (s is TupleLiteralExp)
            {
                TupleLiteralExp tupleLiteralExp = (TupleLiteralExp)s;
                TupleLiteralStep tupleLiteralStep = new TupleLiteralStep(path);
                tupleLiteralStep.TupleExpresion = tupleLiteralExp;
                path.Steps.Insert(0, tupleLiteralStep);
            }
            else if (node.Source is ClassLiteralExp)
            {
                ClassLiteralExp classLiteralExp = (ClassLiteralExp)s;
                ClassLiteralStep tupleLiteralStep = new ClassLiteralStep(path);
                tupleLiteralStep.ClassLiteralExpression = classLiteralExp;
                path.Steps.Insert(0, tupleLiteralStep);
            }
            else if (s == null)
            {
                // do nothing
            }
            else
            {
                GeneralSubexpressionStep generalSubexpressionStep = new GeneralSubexpressionStep(path);
                generalSubexpressionStep.OclExpression = s;
                path.Steps.Insert(0, generalSubexpressionStep);
            }

            return path;
        }

        public static PSMPath BuildPSMPath(VariableExp variableExp, IConstraintsContext constraintsContext, VariableNamer variableNamer, BuildPSMPathParams buildPsmPathParams)
        {
            PSMPath path = new PSMPath();
            path.Context.ConstraintContext = constraintsContext;
            path.Context.VariableNamer = variableNamer;
            path.TupleLiteralToXPathCallback = buildPsmPathParams.TupleLiteralToXPathCallback;
            path.GenericExpressionToXPathCallback = buildPsmPathParams.GenericExpressionToXPathCallback;
            path.ClassLiteralToXPathCallback = buildPsmPathParams.ClassLiteralToXPathCallback;

            PSMPathVariableStep pathVariableStep = new PSMPathVariableStep(path) { VariableExp = variableExp };
            if (string.IsNullOrEmpty(variableExp.referredVariable.Name))
            {
                variableExp.referredVariable.Name = path.Context.VariableNamer.GetName(variableExp.referredVariable.PropertyType);
            }
            path.Steps.Insert(0, pathVariableStep);
            return path;
        }

        public static PSMPath BuildPSMPath(TupleLiteralExp tupleLiteral, IConstraintsContext constraintsContext, VariableNamer variableNamer, BuildPSMPathParams buildPsmPathParams)
        {
            PSMPath path = new PSMPath();
            path.Context.ConstraintContext = constraintsContext;
            path.Context.VariableNamer = variableNamer;
            path.TupleLiteralToXPathCallback = buildPsmPathParams.TupleLiteralToXPathCallback;
            path.ClassLiteralToXPathCallback = buildPsmPathParams.ClassLiteralToXPathCallback;
            path.GenericExpressionToXPathCallback = buildPsmPathParams.GenericExpressionToXPathCallback;

            TupleLiteralStep tupleLiteralStep = new TupleLiteralStep(path) { TupleExpresion = tupleLiteral };
            path.Steps.Insert(0, tupleLiteralStep);
            return path;
        }

        public static PSMPath BuildPSMPath(ClassLiteralExp classLiteral, IConstraintsContext constraintsContext, VariableNamer variableNamer, BuildPSMPathParams buildPsmPathParams)
        {
            PSMPath path = new PSMPath();
            path.Context.ConstraintContext = constraintsContext;
            path.Context.VariableNamer = variableNamer;
            path.TupleLiteralToXPathCallback = buildPsmPathParams.TupleLiteralToXPathCallback;
            path.ClassLiteralToXPathCallback = buildPsmPathParams.ClassLiteralToXPathCallback;
            path.GenericExpressionToXPathCallback = buildPsmPathParams.GenericExpressionToXPathCallback;

            ClassLiteralStep classLiteralStep = new ClassLiteralStep(path) { ClassLiteralExpression = classLiteral };
            path.Steps.Insert(0, classLiteralStep);
            return path;
        }
    }

    public struct BuildPSMPathParams
    {
        private TupleLiteralToXPathCallbackHandler tupleLiteralToXPathCallback;
        private ClassLiteralToXPathCallbackHandler classLiteralToXPathCallback;
        private GenericExpressionToXPathCallbackHandler genericExpressionToXPathCallback;
        private GetRelativeXPathEvolutionCallback getRelativeXPathEvolutionCallback;
        private bool evolution;

        public BuildPSMPathParams(TupleLiteralToXPathCallbackHandler tupleLiteralToXPathCallback, ClassLiteralToXPathCallbackHandler classLiteralToXPathCallback, GenericExpressionToXPathCallbackHandler genericExpressionToXPathCallback, GetRelativeXPathEvolutionCallback getRelativeXPathEvolutionCallback)
        {
            this.tupleLiteralToXPathCallback = tupleLiteralToXPathCallback;
            this.classLiteralToXPathCallback = classLiteralToXPathCallback;
            this.genericExpressionToXPathCallback = genericExpressionToXPathCallback;
            this.getRelativeXPathEvolutionCallback = getRelativeXPathEvolutionCallback;
            evolution = false;
        }

        public TupleLiteralToXPathCallbackHandler TupleLiteralToXPathCallback
        {
            get { return tupleLiteralToXPathCallback; }
        }

        public ClassLiteralToXPathCallbackHandler ClassLiteralToXPathCallback
        {
            get { return classLiteralToXPathCallback; }
        }

        public GenericExpressionToXPathCallbackHandler GenericExpressionToXPathCallback
        {
            get { return genericExpressionToXPathCallback; }
        }

        public GetRelativeXPathEvolutionCallback GetRelativeXPathEvolutionCallback
        {
            get { return getRelativeXPathEvolutionCallback; }
        }

        public bool Evolution
        {
            get { return evolution; }
            set { evolution = value; }
        }
    }
}