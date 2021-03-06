﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Exolutio.Model.OCL.Types
//{


//    public abstract class NonCompositeType : Classifier
//    {
//        protected NonCompositeType(TypesTable.TypesTable tt) : base(tt,"NoConformsToType") { }
//        protected NonCompositeType(TypesTable.TypesTable tt,string name) : base(tt,name) { }

//        public abstract Type CompositeType
//        {
//            get;
//        }

//        public override bool IsAbstract
//        {
//            get
//            {
//                return true;
//            }
//        }
//    }

//    /// <summary>
//    /// NoConformsToType is infrastructure type. Is is used by CompositeType.
//    /// </summary>
//    public sealed class NonCompositeType<T> :NonCompositeType where T : ICompositeType
//    {
//        private NonCompositeType(TypesTable.TypesTable tt) : base(tt,"NonConformsToType<" + typeof(T).Name + ">") { }

//        public override Type CompositeType
//        {
//            get
//            {
//                return typeof(T);
//            }
//        }

       
//        public override bool Equals(object obj)
//        {
//            return obj.GetType() == this.GetType();
//        }

//        public override int GetHashCode()
//        {
//            return base.GetHashCode();
//        }

//        public override bool ConformsTo(Classifier other)
//        {
//            return base.ConformsTo(other);
//        }

//        public override bool ConformsToRegister(Classifier other)
//        {
//            if (other is NonCompositeType == false)
//                return base.ConformsToRegister(other);
//            if (other is NonCompositeType)
//            {
//                NonCompositeType otherNonComposite = other as NonCompositeType;
//                if (CompositeType == otherNonComposite.CompositeType || CompositeType.BaseType == otherNonComposite.CompositeType)
//                    return true;
//            }
//            // is root of leaf of IComposit type
//            if(CompositeType.BaseType.GetInterfaces().Contains(typeof(ICompositeType)) == false)
//                if (other is NonCompositeType == false)
//                    return base.ConformsToRegister(other);
//            return false;
//        }

        
//    }
//}
