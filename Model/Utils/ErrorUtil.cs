using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Model.DTO;
using Tools.ResourcesSupport;

namespace Model.Utils
{
    public static class ErrorUtil
    {
        private static readonly string ErrorSeparator = Environment.NewLine + "---" + Environment.NewLine;

        public static string GetTrace([NotNull] this List<ErrorModel> errors)
        {
            if(!errors.Any()) return String.Empty;

            var result = new StringBuilder();

            var fatals = new List<string>();
            var regular = new List<string>();

            foreach (ErrorModel error in errors)
            {
                if (error.Fatal)
                {
                    fatals.Add(error.Message);
                }
                else
                {
                    regular.Add(error.Message);
                }
            }

            if (fatals.Any())
            {
                CreateStackBlock(result, fatals, "CriticalErrorsHeadline");
            }

            if (regular.Any())
            {
                CreateStackBlock(result, regular, "RegularErrorsHeadline");
            }

            return result.ToString();
        }

        private static void CreateStackBlock([NotNull] StringBuilder result, [NotNull] List<string> errors, [NotNull] string headlineResource)
        {
            result.Append(AppResources.GetResource(headlineResource) + Environment.NewLine);
            result.Append(string.Join(ErrorSeparator, errors));
            result.Append(Environment.NewLine + Environment.NewLine);
        }
    }
}