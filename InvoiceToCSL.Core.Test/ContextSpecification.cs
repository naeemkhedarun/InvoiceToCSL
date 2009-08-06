// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextSpecification.cs" company="EMC Consulting">
//   Marks and Spencer
// </copyright>
// <summary>
//   Defines the ContextSpecification type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MarksAndSpencer.Stores.UserManager.CommonUnitTest.BDD
{
    using MbUnit.Framework;
    using Rhino.Mocks;

    public abstract class StaticContextSpecification
    {
        [SetUp]
        public void setup()
        {
            establish_context();
            because();
        }

        [TearDown]
        public void tear_down()
        {
            after_each_specification();
        }

        protected abstract void because();
        protected abstract void establish_context();
        protected virtual void after_each_specification()
        {
        }

        protected InterfaceType dependency<InterfaceType>() where InterfaceType : class
        {
            return MockRepository.GenerateMock<InterfaceType>();
        }
    }

    /// <summary>
    /// ContextSpecification
    /// </summary>
    /// <typeparam name="SystemUnderTest"></typeparam>
    /// <remarks>Category=Unit test by default to allow for positive filtering in Gallio (seperate running of unit and integration tests)</remarks>
    [MbUnit.Framework.Category("Unit")]
    public abstract class ContextSpecification<SystemUnderTest> : StaticContextSpecification
    {
        protected SystemUnderTest sut;
        protected abstract SystemUnderTest create_sut();
    }
}