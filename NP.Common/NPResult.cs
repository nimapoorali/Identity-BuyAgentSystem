using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Common
{
    public class NPResult
    {
        [System.Text.Json.Serialization.JsonIgnore]
        private readonly List<string> _errors;

        [System.Text.Json.Serialization.JsonIgnore]
        private readonly List<string> _successes;

        public bool IsSuccess { get; set; }
        public IReadOnlyList<string> Successes { get { return _successes; } }
        public bool IsFailed { get; set; }
        public IReadOnlyList<string> Errors { get { return _errors; } }


        public NPResult()
        {
            _errors = new List<string>();
            _successes = new List<string>();
        }


        public void AddErrorMessage(string message)
        {
            message = message.Fix();

            if (message == null)
            {
                return;
            }

            if (_errors.Contains(message))
            {
                return;
            }

            _errors.Add(message);
        }

        public void AddSuccessMessage(string message)
        {
            message = message.Fix();

            if (message == null)
            {
                return;
            }

            if (_successes.Contains(message))
            {
                return;
            }

            _successes.Add(message);
        }
    }

    public class NPResult<T> : NPResult
    {
        public NPResult() : base()
        {
        }
        public T? Value { get; set; }
    }

    public static class ResultExtensions
    {
        public static NPResult ToNPResult(this Result result)
        {
            return result.SetMessages<Result, NPResult>();
        }

        public static NPResult<T> ToNPResult<T>(this Result<T> result)
        {
            var npResult = result.SetMessages<Result<T>, NPResult<T>>();

            if (result.IsSuccess)
            {
                npResult.Value = result.Value;
            }

            return npResult;
        }


        private static TDestination SetMessages<TSource, TDestination>(this TSource result)
            where TSource : IResultBase
            where TDestination : NPResult, new()
        {
            var npResult = new TDestination
            {
                IsFailed = result.IsFailed,
                IsSuccess = result.IsSuccess
            };

            if (result.Errors != null)
            {
                foreach (var item in result.Errors)
                {
                    npResult.AddErrorMessage(item.Message);
                }
            }

            if (result.Successes != null)
            {
                foreach (var item in result.Successes)
                {
                    npResult.AddSuccessMessage(item.Message);
                }
            }

            return npResult;
        }
    }
}
