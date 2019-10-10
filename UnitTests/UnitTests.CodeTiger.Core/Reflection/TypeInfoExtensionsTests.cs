using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using CodeTiger.Reflection;
using Xunit;

namespace UnitTests.CodeTiger.Reflection
{
    /// <summary>
    /// Contains unit tests for the <see cref="TypeExtensions"/> class.
    /// </summary>
    public static class TypeInfoExtensionsTests
    {
        public class IsCompilerGenerated_TypeInfo
        {
            [Fact]
            public void ThrowsArgumentNullExceptionForNullType()
            {
                TypeInfo target = null;

                Assert.Throws<ArgumentNullException>("typeInfo", () => target.IsCompilerGenerated());
            }

            [Theory]
            [InlineData(typeof(object))]
            [InlineData(typeof(bool))]
            [InlineData(typeof(sbyte))]
            [InlineData(typeof(byte))]
            [InlineData(typeof(short))]
            [InlineData(typeof(ushort))]
            [InlineData(typeof(int))]
            [InlineData(typeof(uint))]
            [InlineData(typeof(long))]
            [InlineData(typeof(ulong))]
            [InlineData(typeof(float))]
            [InlineData(typeof(double))]
            [InlineData(typeof(decimal))]
            [InlineData(typeof(char))]
            [InlineData(typeof(string))]
            [InlineData(typeof(DateTime))]
            [InlineData(typeof(Action))]
            [InlineData(typeof(Action<object>))]
            [InlineData(typeof(Func<object>))]
            [InlineData(typeof(Func<object, object>))]
            [InlineData(typeof(TimeSpan))]
            [InlineData(typeof(Type))]
            public void ReturnsFalseForFrameworkTypes(Type type)
            {
                Assert.False(type.GetTypeInfo().IsCompilerGenerated());
            }

            [Theory]
            [InlineData(typeof(EnumWithNoAttributes))]
            [InlineData(typeof(EnumWithNonCompilerGeneratedAttributes))]
            [InlineData(typeof(ClassWithNoAttributes))]
            [InlineData(typeof(ClassWithNonCompilerGeneratedAttributes))]
            [InlineData(typeof(StructWithNoAttributes))]
            [InlineData(typeof(StructWithNonCompilerGeneratedAttributes))]
            public void ReturnsFalseForDeclaredTypes(Type type)
            {
                Assert.False(type.GetTypeInfo().IsCompilerGenerated());
            }

            [Fact]
            public void ReturnsTrueForAnonymousTypeWithNoProperties()
            {
                var target = new { };

                Assert.True(target.GetType().GetTypeInfo().IsCompilerGenerated());
            }

            [Fact]
            public void ReturnsTrueForAnonymousTypeWithOneProperty()
            {
                var target = new
                {
                    Name = "Test Name",
                };

                Assert.True(target.GetType().GetTypeInfo().IsCompilerGenerated());
            }

            [Fact]
            public void ReturnsTrueForAnonymousTypeWithMultipleProperties()
            {
                var target = new
                {
                    Name = "Test Name",
                    Description = "Test Description",
                };

                Assert.True(target.GetType().GetTypeInfo().IsCompilerGenerated());
            }

            [Fact]
            public void ReturnsTrueForTypeCreatedByLambdaInMethod()
            {
                var targets = typeof(ClassWithLambdaInMethod).GetTypeInfo().Assembly.DefinedTypes
                    .Where(x => x.DeclaringType == typeof(ClassWithLambdaInMethod))
                    .ToList();

                Assert.Single(targets);
                Assert.True(targets.All(x => x.IsCompilerGenerated()));
            }

            [Fact]
            public void ReturnsTrueForTypeCreatedByExpressionBodiedProperty()
            {
                var targets = typeof(ClassWithExpressionBodiedProperty).GetTypeInfo().Assembly.DefinedTypes
                    .Where(x => x.DeclaringType == typeof(ClassWithExpressionBodiedProperty))
                    .ToList();

                Assert.Single(targets);
                Assert.True(targets.All(x => x.IsCompilerGenerated()));
            }

            private enum EnumWithNoAttributes
            {
                Unknown,
            }

            [Flags]
            private enum EnumWithNonCompilerGeneratedAttributes
            {
                Unknown,
            }

            private class ClassWithNoAttributes
            {
                public string Name { get; set; }
            }

            private class ClassWithLambdaInMethod
            {
                public Func<int> GetFunc()
                {
                    int i = 1;
                    return () => i;
                }
            }

            private class ClassWithExpressionBodiedProperty
            {
                public string Name { get; set; }
                public bool IsNameEmpty => string.IsNullOrEmpty(Name) || Name.Any(c => c != ' ');
            }

            [DataContract]
            private class ClassWithNonCompilerGeneratedAttributes
            {
                public string Name { get; set; }
            }

            private struct StructWithNoAttributes
            {
                public string Name { get; set; }
            }

            [DataContract]
            private struct StructWithNonCompilerGeneratedAttributes
            {
                public string Name { get; set; }
            }
        }

        public class IsStatic_TypeInfo
        {
            [Fact]
            public void ThrowsArgumentNullExceptionForNullType()
            {
                TypeInfo target = null;

                Assert.Throws<ArgumentNullException>("typeInfo", () => target.IsStatic());
            }

            [Theory]
            [InlineData(typeof(object))]
            [InlineData(typeof(bool))]
            [InlineData(typeof(sbyte))]
            [InlineData(typeof(byte))]
            [InlineData(typeof(short))]
            [InlineData(typeof(ushort))]
            [InlineData(typeof(int))]
            [InlineData(typeof(uint))]
            [InlineData(typeof(long))]
            [InlineData(typeof(ulong))]
            [InlineData(typeof(float))]
            [InlineData(typeof(double))]
            [InlineData(typeof(decimal))]
            [InlineData(typeof(char))]
            [InlineData(typeof(string))]
            [InlineData(typeof(DateTime))]
            [InlineData(typeof(Action))]
            [InlineData(typeof(Action<object>))]
            [InlineData(typeof(Enum))]
            [InlineData(typeof(Func<object>))]
            [InlineData(typeof(Func<object, object>))]
            [InlineData(typeof(TimeSpan))]
            [InlineData(typeof(Type))]
            public void ReturnsFalseForNonStaticFrameworkTypes(Type type)
            {
                Assert.False(type.GetTypeInfo().IsStatic());
            }

            [Theory]
            [InlineData(typeof(Convert))]
            [InlineData(typeof(GC))]
            [InlineData(typeof(Environment))]
            public void ReturnsTrueForStaticFrameworkTypes(Type type)
            {
                Assert.True(type.GetTypeInfo().IsStatic());
            }

            [Fact]
            public void ReturnsFalseForNonStaticDeclaredType()
            {
                Assert.False(typeof(NonStaticClass).GetTypeInfo().IsStatic());
            }

            [Fact]
            public void ReturnsFalseForEmptyAnonymousType()
            {
                var target = new { };

                Assert.False(target.GetType().GetTypeInfo().IsStatic());
            }

            [Fact]
            public void ReturnsFalseForAnonymousTypeWithProperties()
            {
                var target = new
                {
                    Name = "Test Name",
                    Description = "Test Description",
                };

                Assert.False(target.GetType().GetTypeInfo().IsStatic());
            }

            [Fact]
            public void ReturnsTrueForStaticDeclaredType()
            {
                Assert.True(typeof(StaticClass).GetTypeInfo().IsStatic());
            }

            private static class StaticClass
            {
            }

            private class NonStaticClass
            {
            }
        }
    }
}
