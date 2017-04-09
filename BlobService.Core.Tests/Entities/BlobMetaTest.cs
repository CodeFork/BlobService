﻿using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Tests.Entities
{
    class BlobMetaTest : IBlobMeta
    {
        public string Id { get; set; }
        public string ContainerId { get; set; }
        public string OrigFileName { get; set; }
        public int SizeInBytes { get; set; }
        public string MimeType { get; set; }
        public string StorageSubject { get; set; }
        public IContainerMeta Container { get; set; }
    }
}
