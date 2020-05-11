using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFluent;

using PygmentSharp.Core.Extensions;

using Xunit;

namespace PygmentSharp.UnitTests.Extensions
{
    public class TypeExtensionTests
    {
        private class Base { }

        private sealed class Derived : Base { }

        private sealed class HasConstructor
        {
            public int Value { get; }
            public HasConstructor()
            {
                Value = 42;
            }
        }

        [Fact]
        public void InstantiateAs_BaseClassGetsDefault()
        {
            var type = typeof(Derived);
            var result = type.InstantiateAs<Base>();

            Check.That(result).IsNotNull();
            Check.That(result).IsInstanceOf<Derived>();
        }

        [Fact]
        public void InstantiateAs_RunsDefaultConstructor()
        {
            var type = typeof(HasConstructor);
            var result = type.InstantiateAs<HasConstructor>();

            Check.That(result.Value).IsEqualTo(42);
        }

        private class SampleAttribute : Attribute
        {
            public string Name { get; }

            public SampleAttribute(string name)
            {
                Name = name;
            }
        }

        [Sample("foo")]
        public class SampleFoo { }

        [Sample("bar")]
        public class SampleBar { }

        public class SampleNone { }

        [Fact]
        public void HasAttribute_MatchesPredicate()
        {
            Check.That(typeof(SampleFoo).HasAttribute<SampleAttribute>(sa => sa.Name == "foo")).IsTrue();
        }

        [Fact]
        public void HasAttribute_DoesntMatchPredicate()
        {
            Check.That(typeof(SampleBar).HasAttribute<SampleAttribute>(sa => sa.Name == "foo")).IsFalse();
        }

        [Fact]
        public void HasAttribute_DoesntHaveAttribute()
        {
            Check.That(typeof(SampleNone).HasAttribute<SampleAttribute>(sa => sa.Name == "foo")).IsFalse();
        }
    }

}
