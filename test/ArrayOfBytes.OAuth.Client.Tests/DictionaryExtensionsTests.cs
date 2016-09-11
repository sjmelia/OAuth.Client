using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Xunit;

namespace ArrayOfBytes.OAuth.Client.Tests
{
    public class DictionaryExtensionsTests
    {
        [Fact]
        public void MergeTest()
        {
            Dictionary<string, StringValues> a = new Dictionary<string, StringValues>();
            a.Add("1", new StringValues("One"));
            a.Add("2", new StringValues("Two"));

            Dictionary<string, StringValues> b = new Dictionary<string, StringValues>();
            b.Add("2", new StringValues("Two"));
            b.Add("3", new StringValues("Three"));

            a.Merge(b);

            Assert.Equal("One", a["1"].ToString());
            Assert.Equal("Two,Two", a["2"].ToString());
            Assert.Equal("Three", a["3"].ToString());
        }
    }
}
