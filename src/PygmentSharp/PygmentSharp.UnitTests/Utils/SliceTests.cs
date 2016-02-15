using System;
using System.Collections.Generic;
using System.Linq;

using NFluent;

using PygmentSharp.Core.Utils;

using Xunit;

namespace PygmentSharp.UnitTests.Utils
{
    public class SliceTests
    {
        [Fact]
        public void Constructor()
        {
            var inner = new [] { 1, 2, 3, 4 };
            var subject = new Slice<int>(inner, 0, 2);

            Check.That(subject.Start).IsEqualTo(0);
            Check.That(subject.Length).IsEqualTo(2);
            Check.That((IEnumerable<int>)subject).ContainsExactly(1, 2);
        }

        [Fact]
        public void TooLongOnlySetsLengthToEndOfArray()
        {
            var inner = new [] { 1, 2, 3, 4 };
            var subject = new Slice<int>(inner, 2, 8);

            Check.That(subject.Start).IsEqualTo(2);
            Check.That(subject.Length).IsEqualTo(2);
        }

        [Fact]
        public void NegativeLengthGoesToZero()
        {
            var inner = new[] { 1, 2, 3, 4, 5 };
            var subject = new Slice<int>(inner, 1, -1);

            Check.That(subject.Length).IsEqualTo(0);
        }

        [Fact]
        public void IsEnumerable()
        {
            var inner = new[] { 1, 2, 3, 4 };
            var subject = new Slice<int>(inner, 0, 2);

            Check.That((IEnumerable<int>)subject).ContainsExactly(1, 2);
        }

        [Fact]
        public void Indexing_GetsUnderlyingValue()
        {
            var inner = new[] { 1, 2, 3, 4, 5 };
            var subject = new Slice<int>(inner, 1, 2);

            Check.That(subject[0]).IsEqualTo(2);
            Check.That(subject[1]).IsEqualTo(3);
        }

        [Fact]
        public void Indexing_ThrowsIfOutOfRange()
        {
            var inner = new[] { 1, 2, 3, 4, 5 };
            var subject = new Slice<int>(inner, 1, 2);

            Check.ThatCode(() => subject[3]).Throws<IndexOutOfRangeException>();
        }

        [Fact]
        public void ImplicitConversionFromArray()
        {
            Slice<int> subject = new int[] { 1, 2, 3, 4, 5 };

            Check.That(subject.Start).IsEqualTo(0);
            Check.That(subject.Length).IsEqualTo(5);
            Check.That((IEnumerable<int>)subject).ContainsExactly(1, 2, 3, 4, 5);

        }

        [Fact]
        public void ArrayExtensions_Slice()
        {
            var inner = new[] { 1, 2, 3, 4, 5 };

            var subject = inner.Slice(0, 5);
            Check.That(subject.Start).IsEqualTo(0);
            Check.That(subject.Length).IsEqualTo(5);
        }

        [Fact]
        public void ArrayExtensions_SliceStart()
        {
            var inner = new[] { 1, 2, 3, 4, 5 };

            var subject = inner.Slice(2);
            Check.That(subject.Start).IsEqualTo(2);
            Check.That(subject.Length).IsEqualTo(3);
        }

        [Fact]
        public void ArrayExtensions_SliceEnd()
        {
            var inner = new[] { 1, 2, 3, 4, 5 };

            var subject = inner.Slice(-2);
            Check.That(subject.Start).IsEqualTo(3);
            Check.That(subject.Length).IsEqualTo(2);
        }
    }
}
