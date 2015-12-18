﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.MarkdownLite
{
    using System.Collections.Immutable;

    public class MarkdownListBlockToken : IMarkdownToken
    {
        public MarkdownListBlockToken(IMarkdownRule rule, IMarkdownContext context, ImmutableArray<IMarkdownToken> tokens, bool ordered)
        {
            Rule = rule;
            Context = context;
            Tokens = tokens;
            Ordered = ordered;
        }

        public IMarkdownRule Rule { get; }

        public IMarkdownContext Context { get; }

        public ImmutableArray<IMarkdownToken> Tokens { get; }

        public bool Ordered { get; }

        public string RawMarkdown { get; set; }
    }
}