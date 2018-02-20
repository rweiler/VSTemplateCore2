using System;
using Xunit;

namespace VSTemplateCore2.Tests {
	public class ProofTests {
		[Fact]
		public void TestForTrue() {
			Assert.True(true);
		}

		[Fact]
		public void TestForFalse() {
			Assert.True(false);
		}
	}
}
