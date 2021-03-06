﻿using System;
using System.IO;
using Xunit;

namespace Tests.Unit.Support
{
	public class TemporaryFileTests
	{
		[Fact]
		public void Test_File()
		{
			// Arrange.
			var temp = new TemporaryFile();

			// Act.
			var file = temp.File;

			// Assert.
			Assert.NotNull(file);
			Assert.Equal(Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar), file.DirectoryName.TrimEnd(Path.DirectorySeparatorChar));
		}

		[Fact]
		public void Test_Touch()
		{
			// Arrange.
			using (var temp = new TemporaryFile())
			{
				// Act.
				var returned = temp.Touch();

				// Assert.
				Assert.True(temp.File.Exists);
				Assert.Same(temp, returned);
			}
		}

		[Fact]
		public void Test_WithContent()
		{
			// Act.
			using (var temp = new TemporaryFile("stuff"))
			{
				// Assert.
				Assert.True(temp.File.Exists);
				Assert.Equal("stuff", File.ReadAllText(temp.File.FullName));
			}
		}

		[Fact]
		public void Test_Dispose()
		{
			// Arrange.
			var temp = new TemporaryFile().Touch();

			// Act.
			temp.Dispose();
			temp.File.Refresh();

			// Assert.
			Assert.False(temp.File.Exists);
		}

		[Fact]
		public void Test_Destructor()
		{
			// Act.
			FileInfo file = GetTempFile();

			GC.Collect();
			GC.WaitForPendingFinalizers();
			file.Refresh();

			// Assert.
			Assert.False(file.Exists);
		}

		private static FileInfo GetTempFile()
		{
			var temp = new TemporaryFile().Touch();
			return temp.File;
		}
	}
}