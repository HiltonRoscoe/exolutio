﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Exolutio.Model.OCL.Types {
    public class IteratorOperation {

        public IteratorOperation(string name, Func<int, bool> iteratorCount, Func<CollectionType,Classifier, TypesTable.TypesTable, Classifier> expressionType, Func<CollectionType,Classifier, TypesTable.TypesTable, Classifier> bodyType) {
            this.Name = name;
            this.iteratorCount = iteratorCount;
            this.expressionType = expressionType;
            this.bodyType = bodyType;
        }

        public string Name {
            get;
            protected set;
        }

        Func<int, bool> iteratorCount;
        public bool IsIteratorCountValid(int count) {
            return iteratorCount(count);
        }

        Func<CollectionType,Classifier, TypesTable.TypesTable, Classifier> expressionType;
        /// <summary>
        /// Call after validate body type
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="_bodyType"></param>
        /// <param name="tt"></param>
        /// <returns></returns>
        public Classifier ExpressionType(CollectionType sourceType,Classifier _bodyType, TypesTable.TypesTable tt) {
            return expressionType(sourceType,_bodyType,tt);
        }

        Func<CollectionType, Classifier, TypesTable.TypesTable, Classifier> bodyType;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="_bodyType"></param>
        /// <param name="tt"></param>
        /// <returns></returns>
        public Classifier BodyType(CollectionType sourceType, Classifier _bodyType, TypesTable.TypesTable tt) {
            return bodyType(sourceType,_bodyType,tt);
        }
    }
}
