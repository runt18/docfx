﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.Build.RestApi
{
    using System.Collections.Generic;
    using System.Composition;
    using System.Linq;

    using Microsoft.DocAsCode.Build.Common;
    using Microsoft.DocAsCode.DataContracts.Common;
    using Microsoft.DocAsCode.DataContracts.RestApi;
    using Microsoft.DocAsCode.Plugins;

    [Export(nameof(RestApiDocumentProcessor), typeof(IDocumentBuildStep))]
    public class ApplyOverwriteDocumentForRestApi : ApplyOverwriteDocument
    {
        public override string Name => nameof(ApplyOverwriteDocumentForRestApi);

        public override int BuildOrder => 0x10;

        public IEnumerable<RestApiRootItemViewModel> GetRootItemsFromOverwriteDocument(FileModel overwriteModel, string uid, IHostService host)
        {
            return OverwriteDocumentReader.Transform<RestApiRootItemViewModel>(
                overwriteModel,
                uid,
                s => (RestApiRootItemViewModel)BuildRestApiDocument.BuildItem(host, s, overwriteModel, content => content != null && content.Trim() == Constants.ContentPlaceholder));
        }

        public IEnumerable<RestApiRootItemViewModel> GetRootItemsToOverwrite(FileModel articleModel, string uid,
            IHostService host)
        {
            return new[] { (RestApiRootItemViewModel)articleModel.Content };
        }

        public IEnumerable<RestApiChildItemViewModel> GetChildItemsFromOverwriteDocument(FileModel overwriteModel, string uid, IHostService host)
        {
            return OverwriteDocumentReader.Transform<RestApiChildItemViewModel>(
                    overwriteModel,
                    uid,
                    s => (RestApiChildItemViewModel)BuildRestApiDocument.BuildItem(host, s, overwriteModel, content => content != null && content.Trim() == Constants.ContentPlaceholder));
        }

        public IEnumerable<RestApiChildItemViewModel> GetChildItemsToOverwrite(FileModel articleModel, string uid, IHostService host)
        {
            return ((RestApiRootItemViewModel)articleModel.Content).Children.Where(c => c.Uid == uid);
        }

        public IEnumerable<RestApiTagViewModel> GetTagsFromOverwriteDocument(FileModel overwriteModel, string uid, IHostService host)
        {
            return OverwriteDocumentReader.Transform<RestApiTagViewModel>(
                overwriteModel,
                uid,
                s => BuildRestApiDocument.BuildTag(host, s, overwriteModel, content => content != null && content.Trim() == Constants.ContentPlaceholder));
        }

        public IEnumerable<RestApiTagViewModel> GetTagItemsToOverwrite(FileModel articleModel, string uid, IHostService host)
        {
            return ((RestApiRootItemViewModel)articleModel.Content).Tags.Where(c => c.Uid == uid);
        }

        protected override void ApplyOverwrite(IHostService host, List<FileModel> od, string uid, List<FileModel> articles)
        {
            if (articles.Any(a => uid == ((RestApiRootItemViewModel)a.Content).Uid))
            {
                ApplyOverwrite(host, od, uid, articles, GetRootItemsFromOverwriteDocument, GetRootItemsToOverwrite);
            }
            else if (articles.Any(a => ((RestApiRootItemViewModel)a.Content).Children.Any(c => uid == c.Uid)))
            {
                ApplyOverwrite(host, od, uid, articles, GetChildItemsFromOverwriteDocument, GetChildItemsToOverwrite);
            }
            else if (articles.Any(a => ((RestApiRootItemViewModel)a.Content).Tags.Any(t => uid == t.Uid)))
            {
                ApplyOverwrite(host, od, uid, articles, GetTagsFromOverwriteDocument, GetTagItemsToOverwrite);
            }
        }
    }
}
