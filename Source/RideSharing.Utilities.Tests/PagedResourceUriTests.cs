using System;
using Xunit;

namespace RideSharing.Utilities.Tests
{
    public class PagedResourceUriTests
    {
        public class ParsePageIndecesFromTests
        {
            [Fact]
            public void ReturnsZeroToZero_UriHasNoPath()
            {
                var uri = new Uri("http://localhost");
                var result = PagedResourceUri.ParseRecordRangeFrom(uri);
                Assert.Equal(0, result.Start);
                Assert.Equal(0, result.End);
            }
            
            [Fact]
            public void ReturnsZeroToZero_RangeSectionIsNotCommaSeparated()
            {
                var uri = new Uri("http://localhost/999");
                var result = PagedResourceUri.ParseRecordRangeFrom(uri);
                Assert.Equal(0, result.Start);
                Assert.Equal(0, result.End);
            }
            
            [Fact]
            public void ReturnsZeroToZero_RangeSectionIsCommaSeparatedButFirstValueIsNotANumber()
            {
                var uri = new Uri("http://localhost/abc,8");
                var result = PagedResourceUri.ParseRecordRangeFrom(uri);
                Assert.Equal(0, result.Start);
                Assert.Equal(0, result.End);
            }
            
            [Fact]
            public void ReturnsZeroToZero_RangeSectionIsCommaSeparatedButLastValueIsNotANumber()
            {
                var uri = new Uri("http://localhost/2,abc");
                var result = PagedResourceUri.ParseRecordRangeFrom(uri);
                Assert.Equal(0, result.Start);
                Assert.Equal(0, result.End);
            }
            
            [Fact]
            public void ReturnsPageStartAndEndFromUri()
            {
                var uri = new Uri("http://localhost/1,9");
                var result = PagedResourceUri.ParseRecordRangeFrom(uri);
                Assert.Equal(1, result.Start);
                Assert.Equal(9, result.End);
            }
        }
    }
}