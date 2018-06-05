using System;
using System.Collections.Generic;
using System.Linq;
using CodeTiger.Collections.Generic;
using Xunit;

namespace UnitTests.CodeTiger.Collections.Generic
{
    public class EnumerableExtensionsTests
    {
        public class AnyNot_IEnumerableOfTSource_FuncOfTSourceAndBoolean
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceIsNull()
            {
                List<int> target = null;
                Func<int, bool> predicate = element => true;

                Assert.Throws<ArgumentNullException>("source",
                    () => EnumerableExtensions.AnyNot(target, predicate));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenPredicateIsNull()
            {
                var target = new List<int>();
                Func<int, bool> predicate = null;

                Assert.Throws<ArgumentNullException>("predicate",
                    () => EnumerableExtensions.AnyNot(target, predicate));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ReturnsFalseWhenSourceContainsNoElements(bool predicateResult)
            {
                var target = new List<int>();
                Func<int, bool> predicate = element => predicateResult;

                Assert.False(EnumerableExtensions.AnyNot(target, predicate));
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsFalseWhenSourceContainsElementsAndPredicateAlwaysReturnsTrue(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => true;

                Assert.False(EnumerableExtensions.AnyNot(target, predicate));
            }

            [Theory]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsTrueWhenSourceContainsElementsAndPredicateReturnsTrueForFirstItem(
                int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => element == 0;

                Assert.True(EnumerableExtensions.AnyNot(target, predicate));
            }

            [Theory]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsTrueWhenSourceContainsElementsAndPredicateReturnsTrueForLastItem(
                int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => element == (numberOfElements - 1) * 2;

                Assert.True(EnumerableExtensions.AnyNot(target, predicate));
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsTrueWhenSourceContainsElementsAndPredicateAlwaysReturnsFalse(int numberOfElements)
            {
                var target = new List<int>();
                Func<int, bool> predicate = element => false;

                for (int i = 0; i < numberOfElements; i++)
                {
                    target.Add(i);
                }

                Assert.True(EnumerableExtensions.AnyNot(target, predicate));
            }
        }

        public class None_IEnumerableOfTSource
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceIsNull()
            {
                List<int> target = null;

                Assert.Throws<ArgumentNullException>("source",
                    () => EnumerableExtensions.None(target));
            }

            [Fact]
            public void ReturnsTrueWhenSourceContainsNoElements()
            {
                var target = new List<int>();

                Assert.True(EnumerableExtensions.None(target));
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsFalseWhenSourceContainsElements(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Assert.False(EnumerableExtensions.None(target));
            }
        }

        public class None_IEnumerableOfTSource_FuncOfTSourceAndBoolean
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceIsNull()
            {
                List<int> target = null;
                Func<int, bool> predicate = element => true;

                Assert.Throws<ArgumentNullException>("source",
                    () => EnumerableExtensions.None(target, predicate));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenPredicateIsNull()
            {
                var target = new List<int>();
                Func<int, bool> predicate = null;

                Assert.Throws<ArgumentNullException>("predicate",
                    () => EnumerableExtensions.None(target, predicate));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ReturnsTrueWhenSourceContainsNoElements(bool predicateResult)
            {
                var target = new List<int>();
                Func<int, bool> predicate = element => predicateResult;

                Assert.True(EnumerableExtensions.None(target, predicate));
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsFalseWhenSourceContainsElementsAndPredicateAlwaysReturnsTrue(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => true;

                Assert.False(EnumerableExtensions.None(target, predicate));
            }

            [Theory]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsFalseWhenSourceContainsElementsAndPredicateReturnsTrueForFirstItem(
                int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => element == 0;

                Assert.False(EnumerableExtensions.None(target, predicate));
            }

            [Theory]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsFalseWhenSourceContainsElementsAndPredicateReturnsTrueForLastItem(
                int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => element == (numberOfElements - 1) * 2;

                Assert.False(EnumerableExtensions.None(target, predicate));
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsTrueWhenSourceContainsElementsAndPredicateAlwaysReturnsFalse(int numberOfElements)
            {
                var target = new List<int>();
                Func<int, bool> predicate = element => false;

                for (int i = 0; i < numberOfElements; i++)
                {
                    target.Add(i);
                }

                Assert.True(EnumerableExtensions.None(target, predicate));
            }
        }

        public class SkipUntil_IEnumerableOfTSource_FuncOfTSourceAndBoolean
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceIsNull()
            {
                List<int> target = null;
                Func<int, bool> predicate = element => true;

                Assert.Throws<ArgumentNullException>("source",
                    () => EnumerableExtensions.SkipUntil(target, predicate));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenPredicateIsNull()
            {
                var target = new List<int>();
                Func<int, bool> predicate = null;

                Assert.Throws<ArgumentNullException>("predicate",
                    () => EnumerableExtensions.SkipUntil(target, predicate));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ReturnsEmptySequenceWhenSourceContainsNoElements(bool predicateResult)
            {
                var target = new List<int>();
                Func<int, bool> predicate = element => predicateResult;

                var actual = EnumerableExtensions.SkipUntil(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllElementsWhenPredicateAlwaysReturnsTrue(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => true;

                var actual = EnumerableExtensions.SkipUntil(target, predicate);

                Assert.Equal(numberOfElements, actual.Count());

                for (int i = 0; i < numberOfElements; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllElementsWhenPredicateReturnsTrueOnlyForFirstElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => element == 0;

                var actual = EnumerableExtensions.SkipUntil(target, predicate);

                Assert.Equal(numberOfElements, actual.Count());

                for (int i = 0; i < numberOfElements; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsLastElementWhenPredicateReturnsTrueForLastElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => element == (numberOfElements - 1) * 2;

                var actual = EnumerableExtensions.SkipUntil(target, predicate);

                Assert.Single(actual);
                Assert.Equal((numberOfElements - 1) * 2, actual.ElementAt(0));
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsEmptySequenceWhenPredicateAlwaysReturnsFalse(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => false;

                var actual = EnumerableExtensions.SkipUntil(target, predicate);

                Assert.Empty(actual);
            }
        }

        public class SkipUntil_IEnumerableOfTSource_FuncOfTSourceAndInt32AndBoolean
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceIsNull()
            {
                List<int> target = null;
                Func<int, int, bool> predicate = (element, index) => true;

                Assert.Throws<ArgumentNullException>("source",
                    () => EnumerableExtensions.SkipUntil(target, predicate));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenPredicateIsNull()
            {
                var target = new List<int>();
                Func<int, int, bool> predicate = null;

                Assert.Throws<ArgumentNullException>("predicate",
                    () => EnumerableExtensions.SkipUntil(target, predicate));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ReturnsEmptySequenceWhenSourceContainsNoElements(bool predicateResult)
            {
                var target = new List<int>();
                Func<int, int, bool> predicate = (element, index) => predicateResult;

                var actual = EnumerableExtensions.SkipUntil(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllElementsWhenPredicateAlwaysReturnsTrue(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => true;

                var actual = EnumerableExtensions.SkipUntil(target, predicate);

                Assert.Equal(numberOfElements, actual.Count());

                for (int i = 0; i < numberOfElements; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllElementsWhenPredicateReturnsTrueOnlyForFirstElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => index == 0;

                var actual = EnumerableExtensions.SkipUntil(target, predicate);

                Assert.Equal(numberOfElements, actual.Count());

                for (int i = 0; i < numberOfElements; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsLastElementWhenPredicateReturnsTrueForLastElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => index == numberOfElements - 1;

                var actual = EnumerableExtensions.SkipUntil(target, predicate);

                Assert.Single(actual);
                Assert.Equal((numberOfElements - 1) * 2, actual.ElementAt(0));
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsEmptySequenceWhenPredicateAlwaysReturnsFalse(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => false;

                var actual = EnumerableExtensions.SkipUntil(target, predicate);

                Assert.Empty(actual);
            }
        }

        public class TakeUntil_IEnumerableOfTSource_FuncOfTSourceAndBoolean
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceIsNull()
            {
                List<int> target = null;
                Func<int, bool> predicate = element => true;

                Assert.Throws<ArgumentNullException>("source",
                    () => EnumerableExtensions.TakeUntil(target, predicate));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenPredicateIsNull()
            {
                var target = new List<int>();
                Func<int, bool> predicate = null;

                Assert.Throws<ArgumentNullException>("predicate",
                    () => EnumerableExtensions.TakeUntil(target, predicate));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ReturnsEmptySequenceWhenSourceContainsNoElements(bool predicateResult)
            {
                var target = new List<int>();
                Func<int, bool> predicate = element => predicateResult;

                var actual = EnumerableExtensions.TakeUntil(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsEmptySequenceWhenPredicateAlwaysReturnsTrue(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => true;

                var actual = EnumerableExtensions.TakeUntil(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsEmptySequenceWhenPredicateReturnsTrueOnlyForFirstElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => element == 0;

                var actual = EnumerableExtensions.TakeUntil(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllExceptLastElementWhenPredicateReturnsTrueOnlyForLastElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => element == (numberOfElements - 1) * 2;

                var actual = EnumerableExtensions.TakeUntil(target, predicate);

                Assert.Equal(numberOfElements - 1, actual.Count());

                for (int i = 0; i < numberOfElements - 1; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllElementsWhenPredicateAlwaysReturnsFalse(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => false;

                var actual = EnumerableExtensions.TakeUntil(target, predicate);

                Assert.Equal(numberOfElements, actual.Count());

                for (int i = 0; i < numberOfElements; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }
        }

        public class TakeUntil_IEnumerableOfTSource_FuncOfTSourceAndInt32AndBoolean
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceIsNull()
            {
                List<int> target = null;
                Func<int, int, bool> predicate = (element, index) => true;

                Assert.Throws<ArgumentNullException>("source",
                    () => EnumerableExtensions.TakeUntil(target, predicate));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenPredicateIsNull()
            {
                var target = new List<int>();
                Func<int, int, bool> predicate = null;

                Assert.Throws<ArgumentNullException>("predicate",
                    () => EnumerableExtensions.TakeUntil(target, predicate));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ReturnsEmptySequenceWhenSourceContainsNoElements(bool predicateResult)
            {
                var target = new List<int>();
                Func<int, int, bool> predicate = (element, index) => predicateResult;

                var actual = EnumerableExtensions.TakeUntil(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsEmptySequenceWhenPredicateAlwaysReturnsTrue(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => true;

                var actual = EnumerableExtensions.TakeUntil(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsEmptySequenceWhenPredicateReturnsTrueOnlyForFirstElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => index == 0;

                var actual = EnumerableExtensions.TakeUntil(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllExceptLastElementWhenPredicateReturnsTrueOnlyForLastElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => index == numberOfElements - 1;

                var actual = EnumerableExtensions.TakeUntil(target, predicate);

                Assert.Equal(numberOfElements - 1, actual.Count());

                for (int i = 0; i < numberOfElements - 1; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllElementsWhenPredicateAlwaysReturnsFalse(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => false;

                var actual = EnumerableExtensions.TakeUntil(target, predicate);

                Assert.Equal(numberOfElements, actual.Count());

                for (int i = 0; i < numberOfElements; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }
        }

        public class WhereNot_IEnumerableOfTSource_FuncOfTSourceAndBoolean
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceIsNull()
            {
                List<int> target = null;
                Func<int, bool> predicate = element => true;

                Assert.Throws<ArgumentNullException>("source",
                    () => EnumerableExtensions.WhereNot(target, predicate));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenPredicateIsNull()
            {
                var target = new List<int>();
                Func<int, bool> predicate = null;

                Assert.Throws<ArgumentNullException>("predicate",
                    () => EnumerableExtensions.WhereNot(target, predicate));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ReturnsEmptySequenceWhenSourceContainsNoElements(bool predicateResult)
            {
                var target = new List<int>();
                Func<int, bool> predicate = element => predicateResult;

                var actual = EnumerableExtensions.WhereNot(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsEmptySequenceWhenPredicateAlwaysReturnsTrue(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => true;

                var actual = EnumerableExtensions.WhereNot(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllExceptFirstElementWhenPredicateReturnsTrueOnlyForFirstElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => element == 0;

                var actual = EnumerableExtensions.WhereNot(target, predicate);

                Assert.Equal(numberOfElements - 1, actual.Count());

                for (int i = 0; i < numberOfElements - 1; i++)
                {
                    Assert.Equal((i + 1) * 2, actual.ElementAt(i));
                }
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllExceptLastElementWhenPredicateReturnsTrueOnlyForLastElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => element == (numberOfElements - 1) * 2;

                var actual = EnumerableExtensions.WhereNot(target, predicate);

                Assert.Equal(numberOfElements - 1, actual.Count());

                for (int i = 0; i < numberOfElements - 1; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllElementsWhenPredicateAlwaysReturnsFalse(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, bool> predicate = element => false;

                var actual = EnumerableExtensions.WhereNot(target, predicate);

                Assert.Equal(numberOfElements, actual.Count());

                for (int i = 0; i < numberOfElements; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }
        }

        public class WhereNot_IEnumerableOfTSource_FuncOfTSourceAndInt32AndBoolean
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceIsNull()
            {
                List<int> target = null;
                Func<int, int, bool> predicate = (element, index) => true;

                Assert.Throws<ArgumentNullException>("source",
                    () => EnumerableExtensions.WhereNot(target, predicate));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenPredicateIsNull()
            {
                var target = new List<int>();
                Func<int, int, bool> predicate = null;

                Assert.Throws<ArgumentNullException>("predicate",
                    () => EnumerableExtensions.WhereNot(target, predicate));
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ReturnsEmptySequenceWhenSourceContainsNoElements(bool predicateResult)
            {
                var target = new List<int>();
                Func<int, int, bool> predicate = (element, index) => predicateResult;

                var actual = EnumerableExtensions.WhereNot(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsEmptySequenceWhenPredicateAlwaysReturnsTrue(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => true;

                var actual = EnumerableExtensions.WhereNot(target, predicate);

                Assert.Empty(actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllExceptFirstElementWhenPredicateReturnsTrueOnlyForFirstElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => index == 0;

                var actual = EnumerableExtensions.WhereNot(target, predicate);

                Assert.Equal(numberOfElements - 1, actual.Count());

                for (int i = 0; i < numberOfElements - 1; i++)
                {
                    Assert.Equal((i + 1) * 2, actual.ElementAt(i));
                }
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllExceptLastElementWhenPredicateReturnsTrueOnlyForLastElement(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => index == numberOfElements - 1;

                var actual = EnumerableExtensions.WhereNot(target, predicate);

                Assert.Equal(numberOfElements - 1, actual.Count());

                for (int i = 0; i < numberOfElements - 1; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void ReturnsAllElementsWhenPredicateAlwaysReturnsFalse(int numberOfElements)
            {
                var target = Enumerable.Range(0, numberOfElements)
                    .Select(x => x * 2);

                Func<int, int, bool> predicate = (element, index) => false;

                var actual = EnumerableExtensions.WhereNot(target, predicate);

                Assert.Equal(numberOfElements, actual.Count());

                for (int i = 0; i < numberOfElements; i++)
                {
                    Assert.Equal(i * 2, actual.ElementAt(i));
                }
            }
        }
    }
}
