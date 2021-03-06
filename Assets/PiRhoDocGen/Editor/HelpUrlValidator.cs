﻿using PiRhoSoft.Utilities.Editor;
using PiRhoSoft.Utilities;
using System;
using UnityEngine;

namespace PiRhoSoft.DocGen.Editor
{
	[Serializable]
	public class HelpUrlValidator
	{
		private const string _missingHelpUrlWarning = "{0} does not have a HelpURL attribute";
		private const string _invalidHelpUrlWarning = "{0}'s HelpURL attribute is {1} and should be {2}";

		public string UrlRoot; // TODO: expose this as a regex or tag format or something
		[List] public DocumentationNamespaceList IncludedNamespaces = new DocumentationNamespaceList();
		[List] public DocumentationNamespaceList ExcludedNamespaces = new DocumentationNamespaceList();

		public void Validate()
		{
			var types = DocumentationGenerator.FindTypes(type => DocumentationGenerator.IsTypeIncluded(type, DocumentationTypeCategory.Asset | DocumentationTypeCategory.Behaviour, IncludedNamespaces, ExcludedNamespaces));

			foreach (var type in types)
			{
				var id = DocumentationGenerator.GetTypeId(type);
				var url = UrlRoot + id;
				var attribute = type.GetAttribute<HelpURLAttribute>();

				if (attribute == null)
					Debug.LogWarningFormat(_missingHelpUrlWarning, type.Name);
				else if (attribute.URL != url)
					Debug.LogWarningFormat(_invalidHelpUrlWarning, type.Name, attribute.URL, url);
			}
		}
	}
}
