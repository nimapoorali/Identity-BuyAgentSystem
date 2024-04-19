using Identity.Domain.Models.Aggregates.Roles;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using NP.Shared.Domain.Models.SharedKernel;
using Identity.Resources;
using NP.Common;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Identity.Domain.Models.Test.Aggregates.Roles
{
    public class RoleUnitTest
    {
        [Fact]
        public void CreateValidRole()
        {
            var title = StringUtil.RandomAlphaNumeric(RoleTitle.MinLength);
            var groupTitle = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MinLength);
            var result = Role.Create(RoleTitle.Create(title), RoleGroupTitle.Create(groupTitle), ActivityState.Deactive);

            Assert.True(true);


            title = StringUtil.RandomAlphaNumeric(RoleTitle.MaxLength);
            groupTitle = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MaxLength);
            result = Role.Create(RoleTitle.Create(title), RoleGroupTitle.Create(groupTitle), ActivityState.Deactive);

            Assert.True(true);

            title = StringUtil.RandomAlphaNumeric(RoleTitle.MinLength, RoleGroupTitle.MaxLength);
            groupTitle = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MinLength, RoleGroupTitle.MaxLength);
            result = Role.Create(RoleTitle.Create(title), RoleGroupTitle.Create(groupTitle), ActivityState.Deactive);

            Assert.True(true);
        }

        [Fact]
        public void ChangeRoleTitle()
        {
            var title = StringUtil.RandomAlphaNumeric(RoleTitle.MinLength, RoleTitle.MaxLength);
            var groupTitle = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MinLength, RoleTitle.MaxLength);
            var role = Role.Create(RoleTitle.Create(title), RoleGroupTitle.Create(groupTitle), ActivityState.Deactive);
            var newTitle = StringUtil.RandomAlphaNumeric(RoleTitle.MinLength);
            role.Change(RoleTitle.Create(newTitle), role.GroupTitle, role.ActivityState);

            Assert.True(true);
            Assert.Equal(newTitle, role.Title?.Title);
            Assert.Equal(groupTitle, role.GroupTitle?.Title);

            newTitle = StringUtil.RandomAlphaNumeric(RoleTitle.MaxLength);
            role.Change(RoleTitle.Create(newTitle), role.GroupTitle, role.ActivityState);

            Assert.True(true);
            Assert.Equal(newTitle, role.Title?.Title);
            Assert.Equal(groupTitle, role.GroupTitle?.Title);

            newTitle = StringUtil.RandomAlphaNumeric(RoleTitle.MinLength, RoleTitle.MaxLength);
            role.Change(RoleTitle.Create(newTitle), role.GroupTitle, role.ActivityState);

            Assert.True(true);
            Assert.Equal(newTitle, role.Title?.Title);
            Assert.Equal(groupTitle, role.GroupTitle?.Title);
        }

        [Fact]
        public void ChangeRoleGroupTitle()
        {
            var title = StringUtil.RandomAlphaNumeric(RoleTitle.MinLength, RoleTitle.MaxLength);
            var groupTitle = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MinLength, RoleTitle.MaxLength);
            var role = Role.Create(RoleTitle.Create(title), RoleGroupTitle.Create(groupTitle), ActivityState.Deactive);
            var newGroupTitle = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MinLength);
            role.Change(role.Title, RoleGroupTitle.Create(newGroupTitle), role.ActivityState);

            Assert.True(true);
            Assert.Equal(newGroupTitle, role.GroupTitle?.Title);
            Assert.Equal(title, role.Title?.Title);

            newGroupTitle = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MaxLength);
            role.Change(role.Title, RoleGroupTitle.Create(newGroupTitle), role.ActivityState);

            Assert.True(true);
            Assert.Equal(newGroupTitle, role.GroupTitle?.Title);
            Assert.Equal(title, role.Title?.Title);

            newGroupTitle = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MinLength, RoleGroupTitle.MaxLength);
            role.Change(role.Title, RoleGroupTitle.Create(newGroupTitle), role.ActivityState);

            Assert.True(true);
            Assert.Equal(newGroupTitle, role.GroupTitle?.Title);
            Assert.Equal(title, role.Title?.Title);
        }
    }
}
