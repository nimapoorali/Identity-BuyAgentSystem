using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using Identity.Resources;
using NP.Common;
using NP.Resources;
using Xunit;

namespace Identity.Domain.Models.Test.Aggregates.Roles.ValueObjects
{
    public class RoleGroupTitleUnitTest
    {
        //[Fact]
        //public void NullTitle()
        //{
        //    var result = RoleGroupTitle.Create(null);

        //    Assert.True(result.IsFailed);
        //    Assert.Single(result.Errors);
        //    var message = string.Format(Validations.RequiredField, IdentityDataDictionary.RoleGroupTitle);
        //    Assert.Equal(message, result.Errors[0].Message);
        //}

        //[Fact]
        //public void EmptyTitle()
        //{
        //    RoleGroupTitle.Create(string.Empty);

        //    Assert.True(result.IsFailed);
        //    Assert.Single(result.Errors);
        //    var message = string.Format(Validations.RequiredField, IdentityDataDictionary.RoleGroupTitle);
        //    Assert.Equal(message, result.Errors[0].Message);
        //}

        //[Fact]
        //public void LongEmptyTitle()
        //{
        //    var result = RoleGroupTitle.Create("  ");

        //    Assert.True(result.IsFailed);
        //    Assert.Single(result.Errors);
        //    var message = string.Format(Validations.RequiredField, IdentityDataDictionary.RoleGroupTitle);
        //    Assert.Equal(message, result.Errors[0].Message);
        //}

        //[Fact]
        //public void FixTitle()
        //{
        //    var result = RoleGroupTitle.Create(" Group  Title ");

        //    Assert.True(result.IsSuccess);
        //    Assert.Equal("Group Title", result.Value.Title);
        //}

        //[Fact]
        //public void ShortTitle()
        //{
        //    var title = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MinLength - 1);
        //    var result = RoleGroupTitle.Create(title);

        //    Assert.True(result.IsFailed);
        //    Assert.Single(result.Errors);
        //    var message = string.Format(Validations.LengthNotInRangeField, IdentityDataDictionary.RoleGroupTitle);
        //    Assert.Equal(message, result.Errors[0].Message);
        //}

        //[Fact]
        //public void LongTitle()
        //{
        //    var title = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MaxLength + 1);
        //    var result = RoleGroupTitle.Create(title);

        //    Assert.True(result.IsFailed);
        //    Assert.Single(result.Errors);
        //    var message = string.Format(Validations.LengthNotInRangeField, IdentityDataDictionary.RoleGroupTitle);
        //    Assert.Equal(message, result.Errors[0].Message);
        //}

        //[Fact]
        //public void ValidTitle()
        //{

        //    var title = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MinLength);
        //    var result = RoleGroupTitle.Create(title);

        //    Assert.True(result.IsSuccess);

        //    title = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MaxLength);
        //    result = RoleGroupTitle.Create(title);

        //    Assert.True(result.IsSuccess);

        //    title = StringUtil.RandomAlphaNumeric(RoleGroupTitle.MinLength, RoleGroupTitle.MaxLength);
        //    result = RoleGroupTitle.Create(title);

        //    Assert.True(result.IsSuccess);
        //}


    }
}