﻿using System;
using System.Collections.Specialized;
using Xunit;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Redmine.Net.Api.Exceptions;

namespace xUnitTestredminenet45api
{
	[Collection("RedmineCollection")]
	public class VersionTests
	{
		private const string PROJECT_ID = "redmine-net-api";
		private const int NUMBER_OF_VERSIONS = 5;

		//version data - used for create
		private const string NEW_VERSION_NAME = "VersionTesting";
		private const VersionStatus NEW_VERSION_STATUS = VersionStatus.locked;
		private const VersionSharing NEW_VERSION_SHARING = VersionSharing.hierarchy;
		private DateTime NEW_VERSION_DUE_DATE = DateTime.Now.AddDays(7);
		private const string NEW_VERSION_DESCRIPTION = "Version description";

		private const string VERSION_ID = "6";

		//version data - used for update 
		private const string UPDATED_VERSION_ID = "15";
		private const string UPDATED_VERSION_NAME = "Updated version";
		private const VersionStatus UPDATED_VERSION_STATUS = VersionStatus.closed;
		private const VersionSharing UPDATED_VERSION_SHARING = VersionSharing.system;
		private DateTime UPDATED_VERSION_DUE_DATE = DateTime.Now.AddMonths(1);
		private const string UPDATED_VERSION_DESCRIPTION = "Updated description";

		private const string DELETED_VERSION_ID = "22";

		RedmineFixture fixture;
		public VersionTests  (RedmineFixture fixture)
		{
			this.fixture = fixture;
		}

		[Fact]
		public void Should_Get_Versions_By_Project_Id()
		{
			var versions = fixture.redmineManager.GetObjects<Redmine.Net.Api.Types.Version>(new NameValueCollection { { RedmineKeys.PROJECT_ID, PROJECT_ID } });

			Assert.NotNull(versions);
			Assert.NotEmpty (versions);
			Assert.True(versions.Count == NUMBER_OF_VERSIONS, "Versions count != " + NUMBER_OF_VERSIONS);
			Assert.All(versions, v => Assert.IsType<Redmine.Net.Api.Types.Version>(v));
		}

		[Fact]
		public void Should_Create_Version()
		{
			Redmine.Net.Api.Types.Version version = new Redmine.Net.Api.Types.Version();
			version.Name = NEW_VERSION_NAME;
			version.Status = NEW_VERSION_STATUS;
			version.Sharing = NEW_VERSION_SHARING;
			version.DueDate = NEW_VERSION_DUE_DATE;
			version.Description = NEW_VERSION_DESCRIPTION;

			Redmine.Net.Api.Types.Version savedVersion = fixture.redmineManager.CreateObject<Redmine.Net.Api.Types.Version>(version, PROJECT_ID);

			Assert.NotNull(savedVersion);
			Assert.NotNull(savedVersion.Project);
			Assert.True(savedVersion.Name.Equals(NEW_VERSION_NAME), "Version name is invalid.");
			Assert.True(savedVersion.Status.Equals(NEW_VERSION_STATUS), "Version status is invalid.");
			Assert.True(savedVersion.Sharing.Equals(NEW_VERSION_SHARING), "Version sharing is invalid.");
			Assert.NotNull(savedVersion.DueDate);
			Assert.True(savedVersion.DueDate.Value.Date.Equals(NEW_VERSION_DUE_DATE.Date), "Version due date is invalid.");
			Assert.True(savedVersion.Description.Equals(NEW_VERSION_DESCRIPTION), "Version description is invalid.");
		}

		[Fact]
		public void Should_Get_Version_By_Id()
		{
			Redmine.Net.Api.Types.Version version = fixture.redmineManager.GetObject<Redmine.Net.Api.Types.Version>(VERSION_ID, null);

			Assert.NotNull(version);
		}

		[Fact]
		public void Should_Compare_Versions()
		{
			Redmine.Net.Api.Types.Version version = fixture.redmineManager.GetObject<Redmine.Net.Api.Types.Version>(VERSION_ID, null);
			Redmine.Net.Api.Types.Version versionToCompare = fixture.redmineManager.GetObject<Redmine.Net.Api.Types.Version>(VERSION_ID, null);

			Assert.NotNull(version);
			Assert.True(version.Equals(versionToCompare), "Versions are not equal.");

		}

		[Fact]
		public void Should_Update_Version()
		{
			Redmine.Net.Api.Types.Version version = fixture.redmineManager.GetObject<Redmine.Net.Api.Types.Version>(UPDATED_VERSION_ID, null);
			version.Name = UPDATED_VERSION_NAME;
			version.Status = UPDATED_VERSION_STATUS;
			version.Sharing = UPDATED_VERSION_SHARING;
			version.DueDate = UPDATED_VERSION_DUE_DATE;
			version.Description = UPDATED_VERSION_DESCRIPTION;

			fixture.redmineManager.UpdateObject<Redmine.Net.Api.Types.Version>(UPDATED_VERSION_ID, version);

			Redmine.Net.Api.Types.Version updatedVersion = fixture.redmineManager.GetObject<Redmine.Net.Api.Types.Version>(UPDATED_VERSION_ID, null);

			Assert.NotNull(version);
			Assert.True (updatedVersion.Name.Equals (version.Name), "Version name not updated.");
			Assert.True (updatedVersion.Status.Equals (version.Status), "Status not updated");
			Assert.True (updatedVersion.Sharing.Equals (version.Sharing), "Sharing not updated");
			Assert.True (DateTime.Compare(updatedVersion.DueDate.Value.Date, version.DueDate.Value.Date) == 0, "DueDate not updated");
			Assert.True (updatedVersion.Description.Equals (version.Description), "Description not updated");
		}

		[Fact]
		public void Should_Delete_Version()
		{
			RedmineException exception = (RedmineException)Record.Exception(() => fixture.redmineManager.DeleteObject<Redmine.Net.Api.Types.Version>(DELETED_VERSION_ID, null));
			Assert.Null (exception);
			Assert.Throws<NotFoundException>(() => fixture.redmineManager.GetObject<Redmine.Net.Api.Types.Version>(DELETED_VERSION_ID, null));
		}
	}
}

