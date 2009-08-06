// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BDDExtensions.cs" company="Khedan">
//   Naeem Khedarun
// </copyright>
// <summary>
//   Defines the BDDExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace InvoiceToCSL.Core.Test.BDD
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;
    using Action=Gallio.Action;

    /// <summary>
    /// Extension methods for BSS style testing 
    /// </summary>
    public static class BDDExtensions
    {
        /// <summary>
        /// Should_be_an_instance_ofs the specified item.
        /// </summary>
        /// <typeparam name="Type">The type of the ype.</typeparam>
        /// <param name="item">The item to compare.</param>
        public static void should_be_an_instance_of<Type>(this object item)
        {
            Assert.IsInstanceOfType(typeof(Type), item);
        }

        /// <summary>
        /// Should_be_an_instance_ofs the specified item.
        /// </summary>
        /// <param name="item">The item to compare.</param>
        public static void should_be_an_instance_of(this object item, Type type)
        {
            Assert.IsInstanceOfType(type, item);
        }

        /// <summary>
        /// Should_not_be_an_instance_ofs the specified item.
        /// </summary>
        /// <typeparam name="Type">The type of the ype.</typeparam>
        /// <param name="item">The item to compare.</param>
        public static void should_not_be_an_instance_of<Type>(this object item)
        {
            Assert.IsNotInstanceOfType(typeof(Type), item);
        }

        /// <summary>
        /// Should_be_equal_ignoring_cases the specified result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="expected">The expected.</param>
        public static void should_be_equal_ignoring_case(this string result, string expected)
        {
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Should_be_equal_toes the specified actual.
        /// </summary>
        /// <typeparam name="T">The paramter type</typeparam>
        /// <param name="actual">The actual.</param>
        /// <param name="expected">The expected.</param>
        public static void should_be_equal_to<T>(this T actual, T expected)
        {
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Should_be_falses the specified item.
        /// </summary>
        /// <param name="item">if set to <c>true</c> [item].</param>
        public static void should_be_false(this bool item)
        {
            Assert.IsFalse(item);
        }

        /// <summary>
        /// Should_be_greater_thans the specified item.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="item">The item to compare.</param>
        /// <param name="other">The other item to compare.</param>
        public static void should_be_greater_than<T>(this T item, T other) where T : IComparable<T>
        {
            (item.CompareTo(other) >= 0).should_be_true();
        }

        /// <summary>
        /// Should_be_greater_than_or_equal_toes the specified actual.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="actual">The actual.</param>
        /// <param name="expected">The expected.</param>
        public static void should_be_greater_than_or_equal_to<T>(this T actual, T expected) where T : IComparable<T>
        {
            (actual.CompareTo(expected) >= 0).should_be_true();
        }

        /// <summary>
        /// Should_be_the_same_ases the specified actual.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="actual">The actual.</param>
        /// <param name="expected">The expected.</param>
        public static void should_be_the_same_as<T>(this T actual, T expected) where T : class
        {
            Assert.AreSame(expected, actual);
        }

        /// <summary>
        /// Should_be_trues the specified item.
        /// </summary>
        /// <param name="item">if set to <c>true</c> [item].</param>
        public static void should_be_true(this bool item)
        {
            Assert.IsTrue(item);
        }

        /// <summary>
        /// Should_contain_number_of_itemses the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="number_of_items">The number_of_items.</param>
        public static void should_contain_number_of_items<T>(this IEnumerable<T> items, int number_of_items)
        {
            var results = new List<T>(items);

            Assert.AreEqual(number_of_items, results.Count);
        }

        public static void should_not_be_empty<T>(this IEnumerable<T> items)
        {
            should_not_contain_number_of_items(items, 0);
        }
        public static void should_not_contain_number_of_items<T>(this IEnumerable<T> items, int number_of_items)
        {
            var results = new List<T>(items);

            Assert.AreNotEqual(number_of_items, results.Count);
        }

        /// <summary>
        /// Should_matches the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="items_to_find">The items_to_find.</param>
        public static void should_match<T>(this IEnumerable<T> items, IEnumerable<T> items_to_find)
        {
            var results = new List<T>(items);
            new List<T>(items_to_find).Count.should_be_equal_to(new List<T>(items).Count);

            foreach (T itemToFind in items_to_find)
            {
                results.Contains(itemToFind).should_be_true();
            }
        }

        /// <summary>
        /// Should_match_number_of_itemses the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="items_to_find">The items_to_find.</param>
        public static void should_match_number_of_items<T>(this IEnumerable<T> items, IEnumerable<T> items_to_find)
        {
            var results = new List<T>(items); // Create new list in case of null
            new List<T>(items_to_find).Count.should_be_equal_to(new List<T>(results).Count);
        }

        /// <summary>
        /// Should_not_be_equal_toes the specified actual.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="actual">The actual.</param>
        /// <param name="not_equal">The not_equal.</param>
        public static void should_not_be_equal_to<T>(this T actual, T not_equal)
        {
            Assert.AreNotEqual(not_equal, actual);
        }

        /// <summary>
        /// Should_not_be_nulls the specified item.
        /// </summary>
        /// <param name="item">The item to compare.</param>
        public static void should_not_be_null(this object item)
        {
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Should_be_nulls the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public static void should_be_null(this object item)
        {
            Assert.IsNull(item);
        }

        /// <summary>
        /// Should_not_contains the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="items_that_should_not_be_in_list">The items_that_should_not_be_in_list.</param>
        public static void should_not_contain<T>(this IEnumerable<T> items, params T[] items_that_should_not_be_in_list)
        {
            var results = new List<T>(items); // Create new list in case of null
            results.should_not_contain(items_that_should_not_be_in_list as IEnumerable<T>);
        }

        /// <summary>
        /// Should_not_contains the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="items_that_should_not_be_in_list">The items_that_should_not_be_in_list.</param>
        public static void should_not_contain<T>(this IEnumerable<T> items, IEnumerable<T> items_that_should_not_be_in_list)
        {
            foreach (T item in items_that_should_not_be_in_list)
            {
                new List<T>(items).Contains(item).should_be_false();
            }
        }

        /// <summary>
        /// Should_only_contains the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="items_to_find">The items_to_find.</param>
        public static void should_only_contain<T>(this IEnumerable<T> items, params T[] items_to_find)
        {
            var results = new List<T>(items); // Create new list in case of null
            results.should_only_contain(items_to_find as IEnumerable<T>);
        }

        /// <summary>
        /// Should_only_contains the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="items_to_find">The items_to_find.</param>
        public static void should_only_contain<T>(this IEnumerable<T> items, IEnumerable<T> items_to_find)
        {
            foreach (T item in items_to_find)
            {
                new List<T>(items).Contains(item).should_be_true();
            }
        }

        /// <summary>
        /// Should_contains the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="item_to_find">The item_to_find.</param>
        public static void should_contain<T>(this IEnumerable<T> items, T item_to_find)
        {
            new List<T>(items).Contains(item_to_find).should_be_true();
        }

        /// <summary>
        /// Should_only_contain_in_orders the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="items_to_find">The items_to_find.</param>
        public static void should_only_contain_in_order<T>(this IEnumerable<T> items, params T[] items_to_find)
        {
            items.should_only_contain_in_order(items_to_find as IEnumerable<T>);
        }

        /// <summary>
        /// Should_only_contain_in_orders the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="items_to_find">The items_to_find.</param>
        public static void should_only_contain_in_order<T>(this IEnumerable<T> items, IEnumerable<T> items_to_find)
        {
            var results = new List<T>(items);
            var items_to_find_list = new List<T>(items_to_find);
            new List<T>(items_to_find).Count.should_be_equal_to(new List<T>(items).Count);

            for (int i = 0; i < new List<T>(items_to_find).Count; i++)
            {
                items_to_find_list[i].should_be_equal_to(results[i]);
            }
        }

        /// <summary>
        /// Should_throw_an the specified work_to_perform.
        /// </summary>
        /// <typeparam name="ExceptionType">The type of the xception type.</typeparam>
        /// <param name="work_to_perform">The work_to_perform.</param>
        public static void should_throw_an<ExceptionType>(this Action work_to_perform) where ExceptionType : Exception
        {
            Exception resultingException = get_exception_from_performing(work_to_perform);
            resultingException.should_not_be_null();
            resultingException.should_be_an_instance_of<ExceptionType>();
        }

        /// <summary>
        /// string should start with the specified prefix
        /// </summary>
        /// <param name="test">
        /// The string to test against.
        /// </param>
        /// <param name="prefix">
        /// The prefix to test.
        /// </param>
        public static void should_start_with(this string test, string prefix)
        {
            test.StartsWith(prefix).should_be_true();  
        }

        /// <summary>
        /// Get_exception_from_performings the specified work.
        /// </summary>
        /// <param name="work">The action.</param>
        /// <returns>The exception</returns>
        private static Exception get_exception_from_performing(Action work)
        {
            try
            {
                work();
                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}