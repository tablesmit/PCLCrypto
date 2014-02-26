﻿namespace PCLCrypto.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PCLTesting;

    [TestClass]
    public class CryptographicEngineTests
    {
        private readonly byte[] data = new byte[] { 0x3, 0x5, 0x8 };
        private readonly ICryptographicKey key =
            Crypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaSignPkcs1Sha1).CreateKeyPair(512);

        [TestMethod]
        public void Sign_NullInputs()
        {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Crypto.CryptographicEngine.Sign(null, this.data));
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Crypto.CryptographicEngine.Sign(this.key, null));
        }

        [TestMethod]
        public void VerifySignature_NullInputs()
        {
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Crypto.CryptographicEngine.VerifySignature(null, this.data, new byte[2]));
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Crypto.CryptographicEngine.VerifySignature(this.key, null, new byte[2]));
            ExceptionAssert.Throws<ArgumentNullException>(
                () => Crypto.CryptographicEngine.VerifySignature(this.key, this.data, null));
        }

        [TestMethod]
        public void SignAndVerifySignature()
        {
            byte[] signature = Crypto.CryptographicEngine.Sign(this.key, this.data);
            Assert.IsTrue(Crypto.CryptographicEngine.VerifySignature(this.key, this.data, signature));
        }

        [TestMethod]
        public void SignatureAndVerifyTamperedSignature()
        {
            byte[] signature = Crypto.CryptographicEngine.Sign(this.key, this.data);

            // Tamper with the signature.
            signature[signature.Length - 1] += 1;
            Assert.IsFalse(Crypto.CryptographicEngine.VerifySignature(this.key, this.data, signature));
        }

        [TestMethod]
        public void SignatureAndVerifyTamperedData()
        {
            byte[] signature = Crypto.CryptographicEngine.Sign(this.key, this.data);

            // Tamper with the data.
            byte[] tamperedData = new byte[this.data.Length];
            Array.Copy(this.data, tamperedData, this.data.Length);
            tamperedData[tamperedData.Length - 1] += 1;
            Assert.IsFalse(Crypto.CryptographicEngine.VerifySignature(this.key, tamperedData, signature));
        }
    }
}
