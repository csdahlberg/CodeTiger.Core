using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using Xunit;
using CollectionExtensions = CodeTiger.Collections.ObjectModel.CollectionExtensions;

namespace UnitTests.CodeTiger.Collections.ObjectModel
{
    public static class CollectionExtensionsTests
    {
        public class AddRange_ICollectionOfT_IEnumerableOfT
        {
            [Fact]
            public void ThrowsArgumentNullExceptionWhenTargetCollectionIsNull()
            {
                Assert.Throws<ArgumentNullException>("collection",
                    () => CollectionExtensions.AddRange(null!, Enumerable.Empty<object>()));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenSourceCollectionIsNull()
            {
                ICollection<object> collection = new Collection<object>();

                Assert.Throws<ArgumentNullException>("values",
                    () => CollectionExtensions.AddRange(collection, null!));
            }

            [Fact]
            public void DoesNotAddAnythingToTargetCollectionWhenSourceCollectionIsEmpty()
            {
                var collection = new Mock<ICollection<object>>(MockBehavior.Strict);

                CollectionExtensions.AddRange(collection.Object, Enumerable.Empty<object>());

                collection.Verify();
            }

            [Fact]
            public void AddsSourceCollectionWithOneElementToTargetCollection()
            {
                var source = new List<Guid> { Guid.NewGuid() };

                var collection = new Mock<ICollection<Guid>>(MockBehavior.Strict);
                collection.Setup(x => x.Add(source[0]));

                CollectionExtensions.AddRange(collection.Object, source);

                collection.Verify();
            }

            [Fact]
            public void AddsSourceCollectionWithMultipleElementsToTargetCollection()
            {
                var source = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

                var collection = new Mock<ICollection<Guid>>(MockBehavior.Strict);
                collection.Setup(x => x.Add(source[2]));
                collection.Setup(x => x.Add(source[1]));
                collection.Setup(x => x.Add(source[0]));

                CollectionExtensions.AddRange(collection.Object, source);

                collection.Verify();
            }
        }
    }
}
