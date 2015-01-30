//-------------------------------------------------------------------------------------------------
// <copyright file="InconclusiveTestCase.cs" company="Quantum">
//     Copyright (c) Quantum Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Test case that is unstable enough to have its result considered inconclusive.
// </summary>
//
//-------------------------------------------------------------------------------------------------

namespace Quantum.NUnitExtension
{
    using System;
    using System.Reflection;

    using NUnit.Core;

    /// <summary>
    /// Test case that is unstable enough to have its result considered inconclusive.
    /// </summary>
    public class InconclusiveTestCase : NUnitTestMethod
    {
        /// <summary>
        /// Initializes an instance of <see cref="InconclusiveTestCase"/>.
        /// </summary>
        /// <param name="testCase">Original test case.</param>
        public InconclusiveTestCase(NUnitTestMethod testCase)
            : base(testCase.Method)
        {
            NUnitFramework.ApplyCommonAttributes(testCase.Method, this);
            NUnitFramework.ApplyExpectedExceptionAttribute(testCase.Method, this);

            // Copy all the attributes of the original test
            this.BuilderException = testCase.BuilderException;
            this.Categories = testCase.Categories;
            this.Description = testCase.Description;
            this.ExceptionProcessor = testCase.ExceptionProcessor;
            this.Fixture = testCase.Fixture;
            this.IgnoreReason = testCase.IgnoreReason;
            this.Parent = testCase.Parent;
            this.Properties = testCase.Properties;
            this.RunState = testCase.RunState;
            this.TestName.Name = testCase.TestName.Name;
            this.TestName.FullName = testCase.TestName.FullName;
            this.TestName.RunnerID = testCase.TestName.RunnerID;
            this.TestName.TestID = testCase.TestName.TestID;

            this.setUpMethods = GetFieldValue<MethodInfo[]>(typeof(TestMethod), testCase, "setUpMethods");
            this.tearDownMethods = GetFieldValue<MethodInfo[]>(typeof(TestMethod), testCase, "tearDownMethods");
            this.actions = GetFieldValue<TestAction[]>(typeof(TestMethod), testCase, "actions");
            this.suiteActions = GetFieldValue<TestAction[]>(typeof(TestMethod), testCase, "suiteActions");

            this.SetFieldValue("arguments", GetFieldValue<object[]>(typeof(TestMethod), testCase, "arguments"));
            this.SetFieldValue("expectedResult", GetFieldValue<object>(typeof(TestMethod), testCase, "expectedResult"));
            this.SetFieldValue("hasExpectedResult", GetFieldValue<bool>(typeof(TestMethod), testCase, "hasExpectedResult"));
        }

        /// <summary>
        /// Runs a test.
        /// </summary>
        /// <param name="listener">An event listener.</param>
        /// <param name="filter">A test filter.</param>
        /// <returns>Test result.</returns>
        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            var result = base.Run(listener, filter);
            if (result.ResultState == ResultState.Failure || result.ResultState == ResultState.Error)
            {
                result.SetResult(ResultState.Inconclusive, result.Message, result.StackTrace, result.FailureSite);
            }

            return result;
        }

        /// <summary>
        /// Runs test.
        /// </summary>
        /// <returns>Test result.</returns>
        public override TestResult RunTest()
        {
            var result = base.RunTest();
            if (result.ResultState == ResultState.Failure || result.ResultState == ResultState.Error)
            {
                result.SetResult(ResultState.Inconclusive, result.Message, result.StackTrace, result.FailureSite);
            }

            return result;
        }

        /// <summary>
        /// Gets a field value.
        /// </summary>
        /// <typeparam name="T">Type of value to get.</typeparam>
        /// <param name="type">Type of the source object.</param>
        /// <param name="obj">Source object.</param>
        /// <param name="name">Field name.</param>
        /// <returns>Field value.</returns>
        private static T GetFieldValue<T>(Type type, object obj, string name)
        {
            var field = type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            return (T)field.GetValue(obj);
        }

        /// <summary>
        /// Sets a field value.
        /// </summary>
        /// <param name="name">Field name.</param>
        /// <param name="value">Value to set.</param>
        private void SetFieldValue(string name, object value)
        {
            var field = this.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            field.SetValue(this, value);
        }
    }
}
