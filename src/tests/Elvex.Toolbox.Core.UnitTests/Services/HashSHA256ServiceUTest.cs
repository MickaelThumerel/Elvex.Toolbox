// Copyright (c) Elvexoft.
// The Elvexoft licenses this file to you under the MIT license.
// Produce by Elvexoft & community

namespace Elvex.Toolbox.Core.UnitTests.Services
{
    using Elvex.Toolbox.Services;

    using System.Security.Cryptography;

    public class HashSHA256ServiceUTest : HashBaseServiceUTest<HashSHA256Service>
    {
        public HashSHA256ServiceUTest() 
            : base(SHA256.Create)
        {
        }
    }
}
