/* Copyright (C) 2020 Tal Aloni <tal.aloni.il@gmail.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the GNU Lesser Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMBLibrary.SMB2;
using SMBLibrary.Utilities;

namespace SMBLibrary.Tests
{
    [TestClass]
    // https://docs.microsoft.com/en-us/archive/blogs/openspecification/encryption-in-smb-3-0-a-protocol-perspective
    public class SMB2EncryptionTests
    {
        [TestMethod]
        public void TestEncryptionKeyGeneration()
        {
            byte[] sessionKey = new byte[]{ 0xB4, 0x54, 0x67, 0x71, 0xB5, 0x15, 0xF7, 0x66, 0xA8, 0x67, 0x35, 0x53, 0x2D, 0xD6, 0xC4, 0xF0};

            byte[] expectedEncryptionKey = new byte[] { 0x26, 0x1B, 0x72, 0x35, 0x05, 0x58, 0xF2, 0xE9, 0xDC, 0xF6, 0x13, 0x07, 0x03, 0x83, 0xED, 0xBF };
            
            byte[] encryptionKey = SMB2Cryptography.GenerateClientEncryptionKey(sessionKey, SMB2Dialect.SMB300, null);

            Assert.IsTrue(ByteUtils.AreByteArraysEqual(expectedEncryptionKey, encryptionKey));
        }

        [TestMethod]
        public void TestDecryptionKeyGeneration()
        {
            byte[] sessionKey = new byte[] { 0xB4, 0x54, 0x67, 0x71, 0xB5, 0x15, 0xF7, 0x66, 0xA8, 0x67, 0x35, 0x53, 0x2D, 0xD6, 0xC4, 0xF0 };

            byte[] decryptionKey = SMB2Cryptography.GenerateClientDecryptionKey(sessionKey, SMB2Dialect.SMB300, null);

            byte[] expectedDecryptionKey = new byte[] { 0x8F, 0xE2, 0xB5, 0x7E, 0xC3, 0x4D, 0x2D, 0xB5, 0xB1, 0xA9, 0x72, 0x7F, 0x52, 0x6B, 0xBD, 0xB5 };

            Assert.IsTrue(ByteUtils.AreByteArraysEqual(expectedDecryptionKey, decryptionKey));
        }

        [TestMethod]
        public void TestEncryption()
        {
            byte[] encryptionKey = new byte[] { 0x26, 0x1B, 0x72, 0x35, 0x05, 0x58, 0xF2, 0xE9, 0xDC, 0xF6, 0x13, 0x07, 0x03, 0x83, 0xED, 0xBF };
            byte[] nonce = new byte[] { 0x66, 0xE6, 0x9A, 0x11, 0x18, 0x92, 0x58, 0x4F, 0xB5, 0xED, 0x52 };
            ulong sessionID = 0x8e40014000011;

            byte[] message = new byte[] { 0xFE, 0x53, 0x4D, 0x42, 0x40, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x40, 0x00,
                                          0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                          0xFF, 0xFE, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x14, 0x00, 0xE4, 0x08, 0x00,
                                          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                          0x31, 0x00, 0x70, 0x00, 0x17, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                          0x15, 0x01, 0x00, 0x00, 0x39, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x39, 0x02, 0x00, 0x00,
                                          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x70, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                          0x53, 0x6D, 0x62, 0x33, 0x20, 0x65, 0x6E, 0x63, 0x72, 0x79, 0x70, 0x74, 0x69, 0x6F, 0x6E, 0x20,
                                          0x74, 0x65, 0x73, 0x74, 0x69, 0x6E, 0x67};

            byte[] expectedEncrypted = new byte[] { 0x25, 0xC8, 0xFE, 0xE1, 0x66, 0x05, 0xA4, 0x37, 0x83, 0x2D, 0x1C, 0xD5, 0x2D, 0xA9, 0xF4, 0x64,
                                                    0x53, 0x33, 0x48, 0x2A, 0x17, 0x5F, 0xE5, 0x38, 0x45, 0x63, 0xF4, 0x5F, 0xCD, 0xAF, 0xAE, 0xF3,
                                                    0x8B, 0xC6, 0x2B, 0xA4, 0xD5, 0xC6, 0x28, 0x97, 0x99, 0x66, 0x25, 0xA4, 0x4C, 0x29, 0xBE, 0x56,
                                                    0x58, 0xDE, 0x2E, 0x61, 0x17, 0x58, 0x57, 0x79, 0xE7, 0xB5, 0x9F, 0xFD, 0x97, 0x12, 0x78, 0xD0,
                                                    0x85, 0x80, 0xD7, 0xFA, 0x89, 0x9E, 0x41, 0x0E, 0x91, 0x0E, 0xAB, 0xF5, 0xAA, 0x1D, 0xB4, 0x30,
                                                    0x50, 0xB3, 0x3B, 0x49, 0x18, 0x26, 0x37, 0x75, 0x9A, 0xC1, 0x5D, 0x84, 0xBF, 0xCD, 0xF5, 0xB6,
                                                    0xB2, 0x38, 0x99, 0x3C, 0x0F, 0x4C, 0xF4, 0xD6, 0x01, 0x20, 0x23, 0xF6, 0xC6, 0x27, 0x29, 0x70,
                                                    0x75, 0xD8, 0x4B, 0x78, 0x03, 0x91, 0x2D, 0x0A, 0x96, 0x39, 0x63, 0x44, 0x53, 0x59, 0x5E, 0xF3,
                                                    0xE3, 0x3F, 0xFE, 0x4E, 0x7A, 0xC2, 0xAB };

            byte[] expectedSignature = new byte[] { 0x81, 0xA2, 0x86, 0x53, 0x54, 0x15, 0x44, 0x5D, 0xAE, 0x39, 0x39, 0x21, 0xE4, 0x4F, 0xA4, 0x2E };

            byte[] signature;
            byte[] encryptedMessage = SMB2Cryptography.EncryptMessage(encryptionKey, nonce, message, sessionID, out signature);

            Assert.IsTrue(ByteUtils.AreByteArraysEqual(expectedEncrypted, encryptedMessage));
            // The associated data in this sample include non-zero nonce padding so we ignore signature validation
        }

        [TestMethod]
        public void TestDecryption()
        {
            byte[] decryptionKey = new byte[] { 0x8F, 0xE2, 0xB5, 0x7E, 0xC3, 0x4D, 0x2D, 0xB5, 0xB1, 0xA9, 0x72, 0x7F, 0x52, 0x6B, 0xBD, 0xB5 };

            byte[] transformedPacket = new byte[] { 0xFD, 0x53, 0x4D, 0x42, 0xA6, 0x01, 0x55, 0x30, 0xA1, 0x8F, 0x6D, 0x9A, 0xFF, 0xE2, 0x2A, 0xFA,
                                                    0xE8, 0xE6, 0x64, 0x84, 0x86, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x14,
                                                    0x00, 0xE4, 0x08, 0x00, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x11, 0x00, 0x00, 0x14,
                                                    0x00, 0xE4, 0x08, 0x00, 0xDB, 0xF4, 0x64, 0x35, 0xC5, 0xF1, 0x41, 0x69, 0x29, 0x3C, 0xE0, 0x79,
                                                    0xE3, 0x44, 0x47, 0x9B, 0xF6, 0x70, 0x22, 0x7E, 0x49, 0x87, 0x3F, 0x45, 0x86, 0x72, 0xC3, 0x09,
                                                    0x8D, 0xAC, 0x46, 0x7D, 0xD5, 0x80, 0x9F, 0x36, 0x9D, 0x67, 0x40, 0x91, 0x66, 0x51, 0x57, 0x87,
                                                    0x14, 0x83, 0xE0, 0x1F, 0x7B, 0xEC, 0xD0, 0x20, 0x64, 0xEA, 0xC3, 0xE2, 0x35, 0xF9, 0x13, 0x66,
                                                    0x8B, 0xBC, 0x2F, 0x09, 0x79, 0x80, 0xD4, 0xB3, 0x78, 0xF1, 0x99, 0x3E, 0xFF, 0x6E, 0x60, 0xD1,
                                                    0x77, 0x30, 0x9E, 0x5B };

            byte[] expectedDecryptedMessage = new byte[] { 0xFE, 0x53, 0x4D, 0x42, 0x40, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x21, 0x00,
                                                           0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                           0xFF, 0xFE, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x14, 0x00, 0xE4, 0x08, 0x00,
                                                           0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                           0x11, 0x00, 0x00, 0x00, 0x17, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            SMB2TransformHeader transformHeader = new SMB2TransformHeader(transformedPacket, 0);
            byte[] encryptedMessage = ByteReader.ReadBytes(transformedPacket, SMB2TransformHeader.Length, (int)transformHeader.OriginalMessageSize);
            byte[] decryptedMessage = SMB2Cryptography.DecryptMessage(decryptionKey, transformHeader, encryptedMessage);

            Assert.IsTrue(ByteUtils.AreByteArraysEqual(expectedDecryptedMessage, decryptedMessage));
        }

        public void TestAll()
        {
            TestEncryptionKeyGeneration();
            TestDecryptionKeyGeneration();
            TestEncryption();
            TestDecryption();
        }
    }
}
