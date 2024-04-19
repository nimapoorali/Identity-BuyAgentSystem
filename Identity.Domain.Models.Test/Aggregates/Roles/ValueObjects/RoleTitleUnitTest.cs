using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using Identity.Resources;
using NP.Common;
using NP.Resources;
using Xunit;

namespace Identity.Domain.Models.Test.Aggregates.Roles.ValueObjects
{
    public class RoleTitleUnitTest
    {
        //[Fact]
        //public void NullTitle()
        //{
        //    var result = RoleTitle.Create(null);

        //    Assert.True(result.IsFailed);
        //    Assert.Single(result.Errors);
        //    var message = string.Format(Validations.RequiredField, IdentityDataDictionary.RoleTitle);
        //    Assert.Equal(message, result.Errors[0].Message);
        //}

        //[Fact]
        //public void EmptyTitle()
        //{
        //    var result = RoleTitle.Create(string.Empty);

        //    Assert.True(result.IsFailed);
        //    Assert.Single(result.Errors);
        //    var message = string.Format(Validations.RequiredField, IdentityDataDictionary.RoleTitle);
        //    Assert.Equal(message, result.Errors[0].Message);
        //}

        //[Fact]
        //public void LongEmptyTitle()
        //{
        //    var result = RoleTitle.Create("  ");

        //    Assert.True(result.IsFailed);
        //    Assert.Single(result.Errors);
        //    var message = string.Format(Validations.RequiredField, IdentityDataDictionary.RoleTitle);
        //    Assert.Equal(message, result.Errors[0].Message);
        //}

        //[Fact]
        //public void FixTitle()
        //{
        //    var result = RoleTitle.Create(" Nima  Poorali ");

        //    Assert.True(result.IsSuccess);
        //    Assert.Equal("Nima Poorali", result.Value.Title);
        //}

        //[Fact]
        //public void ShortTitle()
        //{
        //    var title = StringUtil.RandomAlphaNumeric(RoleTitle.MinLength - 1);
        //    var result = RoleTitle.Create(title);

        //    Assert.True(result.IsFailed);
        //    Assert.Single(result.Errors);
        //    var message = string.Format(Validations.LengthNotInRangeField, IdentityDataDictionary.RoleTitle);
        //    Assert.Equal(message, result.Errors[0].Message);
        //}

        //[Fact]
        //public void LongTitle()
        //{
        //    var title = StringUtil.RandomAlphaNumeric(RoleTitle.MaxLength + 1);
        //    var result = RoleTitle.Create(title);

        //    Assert.True(result.IsFailed);
        //    Assert.Single(result.Errors);
        //    var message = string.Format(Validations.LengthNotInRangeField, IdentityDataDictionary.RoleTitle);
        //    Assert.Equal(message, result.Errors[0].Message);
        //}

        //[Fact]
        //public void ValidTitle()
        //{

        //    var title = StringUtil.RandomAlphaNumeric(RoleTitle.MinLength);
        //    var result = RoleTitle.Create(title);

        //    Assert.True(result.IsSuccess);

        //    title = StringUtil.RandomAlphaNumeric(RoleTitle.MaxLength);
        //    result = RoleTitle.Create(title);

        //    Assert.True(result.IsSuccess);

        //    title = StringUtil.RandomAlphaNumeric(RoleTitle.MinLength, RoleTitle.MaxLength);
        //    result = RoleTitle.Create(title);

        //    Assert.True(result.IsSuccess);
        //}


    }
}