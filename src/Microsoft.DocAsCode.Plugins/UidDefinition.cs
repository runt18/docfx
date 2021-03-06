﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.Plugins
{
    using Newtonsoft.Json;

    public class UidDefinition
    {
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("file")]
        public string File { get; }
        [JsonProperty("line")]
        public int? Line { get; }
        [JsonProperty("column")]
        public int? Column { get; }

        [JsonConstructor]
        public UidDefinition(string name, string file, int? line = null, int? column = null)
        {
            Name = name;
            File = file;
            Line = line;
            Column = column;
        }
    }
}
