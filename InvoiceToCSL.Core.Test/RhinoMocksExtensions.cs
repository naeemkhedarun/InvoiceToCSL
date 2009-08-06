//------------------------------------------------------------------------------------------------- 
// <copyright file="RhinoMocksExtensions.cs" company="EMC Consulting">
// Copyright (c) EMC Consulting.  All rights reserved.
// </copyright>
// <summary>Defines the RhinoMocksExtensions type.</summary>
//-------------------------------------------------------------------------------------------------

namespace MarksAndSpencer.Stores.UserManager.CommonUnitTest.BDD
{
    using System;
    using Rhino.Mocks;
    using Rhino.Mocks.Interfaces;

    /// <summary>
    /// Rhino Mocks extension methods class.
    /// </summary>
    public static class RhinoMocksExtensions
    {
        /// <summary>
        /// Setups the specified mock.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="mock">The mock item.</param>
        /// <param name="action">The action.</param>
        /// <returns>The method options</returns>
        public static IMethodOptions<Rhino.Mocks.RhinoMocksExtensions.VoidType> setup<T>(this T mock, Action<T> action) where T : class
        {
            return mock.Expect(action);
        }

        /// <summary>
        /// Setup_results the specified mock.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <typeparam name="R">The result type</typeparam>
        /// <param name="mock">The mock object.</param>
        /// <param name="func">The function.</param>
        /// <returns>The IMethod options</returns>
        public static IMethodOptions<R> setup_result<T, R>(this T mock, Function<T, R> func) where T : class
        {
            return mock.Expect(func).Repeat.AtLeastOnce();
        }

        /// <summary>
        /// Setup_results the specified mock.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <typeparam name="R">The result type</typeparam>
        /// <param name="mock">The mock object.</param>
        /// <param name="func">The function.</param>
        /// <returns>The IMethod options</returns>
        public static IMethodOptions<R> setup_result_while_ignoring_arguments<T, R>(this T mock, Function<T, R> func) where T : class
        {
            return mock.Expect(func).IgnoreArguments().Repeat.AtLeastOnce();
        }

        /// <summary>
        /// Was_told_toes the specified mock.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="mock">The mock object.</param>
        /// <param name="item">The callback.</param>
        public static void setup_callback<T>(this T mock, Action<T> item, Delegate callback) where T : class
        {
            mock.Expect(item).IgnoreArguments().Do(callback);
        }

        /// <summary>
        /// Was_told_toes the specified mock.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="mock">The mock object.</param>
        /// <param name="item">The action.</param>
        public static void was_told_to<T>(this T mock, Action<T> item)
        {
            mock.AssertWasCalled(item);
        }

        /// <summary>
        /// Was_told_toes the specified mock.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="mock">The mock object.</param>
        /// <param name="item">The action.</param>
        public static void was_told_to_while_ignoring_arguments<T>(this T mock, Action<T> item)
        {
            mock.AssertWasCalled(item, options => options.IgnoreArguments());
        }

        /// <summary>
        /// Confirms that the specified mock object was not told to call a specific method.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="mock">The mock object.</param>
        /// <param name="item">The action.</param>
        public static void was_not_told_to<T>(this T mock, Action<T> item)
        {
            mock.AssertWasNotCalled(item);
        }

        /// <summary>
        /// Confirms that the specified mock object was not told to call a specific method.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="mock">The mock object.</param>
        /// <param name="item">The action.</param>
        public static void was_not_told_to_while_ignoring_arguments<T>(this T mock, Action<T> item)
        {
            mock.AssertWasNotCalled(item, options => options.IgnoreArguments());
        }
    }
}