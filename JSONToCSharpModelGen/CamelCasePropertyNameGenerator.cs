﻿using NJsonSchema;
using NJsonSchema.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JSONToCSharpModelGen
{
    public class CamelCasePropertyNameGenerator : IPropertyNameGenerator, ITypeNameGenerator
    {
        public string Generate(JsonSchemaProperty property)
        {
            string textToChange = property.Name;
            StringBuilder resultBuilder = new StringBuilder();

            foreach (char c in textToChange)
            {
                // Replace anything, but letters and digits, with space
                if (!char.IsLetterOrDigit(c))
                {
                    resultBuilder.Append(" ");
                }
                else
                {
                    if (char.IsUpper(c))
                    {
                        resultBuilder.Append(" " + c);

                    }
                    else
                    {
                        resultBuilder.Append(c);

                    }
                }
            }

            string result = resultBuilder.ToString();

            // Make result string all lowercase, because ToTitleCase does not change all uppercase correctly
            result = result.ToLower();

            // Creates a TextInfo based on the "en-US" culture.
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            result = myTI.ToTitleCase(result).Replace(" ", String.Empty);

            return result;
        }

        public string Generate(JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
        {
            //var property = schema.Properties[typeNameHint];
            //PluralizationService.Singularize(typeNameHint);
            if (typeNameHint.EndsWith("ie"))
            {
                typeNameHint = typeNameHint.Substring(0, typeNameHint.Length - 2) + "y";
            }
            return typeNameHint;
        }
    }
}
