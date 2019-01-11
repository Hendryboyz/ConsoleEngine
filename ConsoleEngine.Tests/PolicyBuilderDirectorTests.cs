using ConsoleEngine.Business;
using ConsoleEngine.Domain.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ConsoleEngine.Tests
{
    [TestFixture]
    public class PolicyBuilderDirectorTests
    {
        private IPolicyBuilder policyBuilder;
        private PolicyBuilderDirector policyDirector;

        [Test]
        public void CanCreate()
        {
            FakeDependency();
            PolicyBuilderDirector policyDirector = new PolicyBuilderDirector(policyBuilder);
            Assert.NotNull(policyDirector);
        }

        private void FakeDependency()
        {
            policyBuilder = Substitute.For<IPolicyBuilder>();
        }

        [SetUp]
        public void SetUp()
        {
            FakeDependency();
            policyDirector = new PolicyBuilderDirector(policyBuilder);
            Assert.NotNull(policyDirector);
        }

        [Test]
        public void GivenNothing_WhenConstruct_ThenReceivedSetRetryAndTimeOut()
        {
            policyDirector.Construct();

            policyBuilder.Received().SetRetryPolicy(Arg.Any<int>(), Arg.Any<int>(),
                Arg.Any<Action<Exception, TimeSpan, int>>());
            policyBuilder.Received().SetTimeoutPolicy(Arg.Any<int>(),
                Arg.Any<Action<TimeSpan, Task, Exception>>());
            policyBuilder.Received().Build();
        }
    }
}
