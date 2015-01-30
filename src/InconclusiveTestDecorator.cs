//-------------------------------------------------------------------------------------------------
// <copyright file="InconclusiveTestDecorator.cs" company="Quantum">
//     Copyright (c) Quantum Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Decorates test case that is unstable enough to have its result considered inconclusive.
// </summary>
//
//-------------------------------------------------------------------------------------------------

namespace Quantum.NUnitExtension
{
    using System.Reflection;

    using NUnit.Core;
    using NUnit.Core.Extensibility;
    
    /// <summary>
    /// Decorates test case that is unstable enough to have its result considered inconclusive.
    /// </summary>
    [NUnitAddin(Name = "BaseExtension", Description = "Adding inconclusive tests.", Type = ExtensionType.Core)]
    public class InconclusiveTestDecorator : IAddin, ITestDecorator
    {
        /// <summary>
        /// When called, the add-in installs itself into
        /// the host, if possible. Because NUnit uses separate
        /// hosts for the client and test domain environments,
        /// an add-in may be invited to istall itself more than
        /// once. The add-in is responsible for checking which
        /// extension points are supported by the host that is
        /// passed to it and taking the appropriate action.
        /// </summary>
        /// <param name="host">The host in which to install the add-in</param>
        /// <returns>True if the add-in was installed, otehrwise false</returns>
        public bool Install(IExtensionHost host)
        {
            IExtensionPoint decorators = host.GetExtensionPoint("TestDecorators");
            if (decorators == null)
            {
                return false;
            }

            decorators.Install(this);
            return true;
        }

        /// <summary>
        /// Examine the a Test and either return it as is, modify it
        /// or return a different TestCase.
        /// </summary>
        /// <param name="test">The Test to be decorated</param>
        /// <param name="member">The MethodInfo used to construct the test</param>
        /// <returns>The resulting Test</returns>
        public Test Decorate(Test test, System.Reflection.MemberInfo member)
        {
            var attr = Reflect.GetAttribute(member, "SymformNUnitExtension.InconclusiveTestAttribute", false);
            if (attr == null)
            {
                return test;
            }

            var nunitTest = test as NUnitTestMethod;
            if (nunitTest != null)
            {
                return new InconclusiveTestCase(nunitTest);
            }

            var paramTests = test as ParameterizedMethodSuite;
            if (paramTests != null)
            {
                Decorate(paramTests);
            }

            return test;
        }

        /// <summary>
        /// Decorates a parameterized test suite.
        /// </summary>
        /// <param name="paramTests">Test suite.</param>
        private static void Decorate(ParameterizedMethodSuite paramTests)
        {
            for (int i = 0; i < paramTests.Tests.Count; i++)
            {
                var test = paramTests.Tests[i];
                var nunitTest = test as NUnitTestMethod;
                if (nunitTest != null)
                {
                    paramTests.Tests[i] = new InconclusiveTestCase(nunitTest);
                }
            }
        }
    }
}
