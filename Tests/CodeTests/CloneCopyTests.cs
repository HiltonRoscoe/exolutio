using System;
using System.Reflection;
using Exolutio.Model;
using Exolutio.Model.Versioning;
using NUnit.Framework;

namespace Exolutio.Tests.CodeTests
{
    [TestFixture]
	public class CloneCopyTests : CodeTestBase
	{
		private static bool isNotCloneable(Type t)
		{
		    return false;
		}

		[Test]
		public void TestCloneOverride()
		{
			Type elementInterfaceType = typeof(Component);
			foreach (Type type in ModelAssembly.GetTypes())
			{
				if (type.IsClass && elementInterfaceType.IsAssignableFrom(type))
				{
                    MethodInfo method = type.GetMethod("Clone", new Type[] { typeof(ProjectVersion), typeof(ElementCopiesMap) });
					if (!type.IsAbstract && !isNotCloneable(type))
					{
						Assert.IsNotNull(method, string.Format("Type {0} should override Clone method. ", type.Name));
						Assert.AreEqual(method.DeclaringType, type, string.Format("Type {0} should override Clone method. ", type.Name));
					}
					else if (type.Name != "Component")
					{
						Assert.IsFalse(method != null && type.IsAbstract && method.DeclaringType == type, string.Format("Type {0} should not override Clone method because it is abstract. ", type.Name));
					}
				}

			}
		}

	}
}