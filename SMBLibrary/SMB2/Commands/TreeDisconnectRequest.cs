/* Copyright (C) 2017 Tal Aloni <tal.aloni.il@gmail.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the GNU Lesser Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 */
using System;
using System.Collections.Generic;
using SMBLibrary.Utilities;

namespace SMBLibrary.SMB2
{
    /// <summary>
    /// SMB2 TREE_DISCONNECT Request
    /// </summary>
    public class TreeDisconnectRequest : SMB2Command
    {
        public const int DeclaredSize = 4;

        private ushort StructureSize;
        public ushort Reserved;

        public TreeDisconnectRequest() : base(SMB2CommandName.TreeDisconnect)
        {
            StructureSize = DeclaredSize;
        }

        public TreeDisconnectRequest(byte[] buffer, int offset) : base(buffer, offset)
        {
            StructureSize = LittleEndianConverter.ToUInt16(buffer, offset + SMB2Header.Length + 0);
            Reserved = LittleEndianConverter.ToUInt16(buffer, offset + SMB2Header.Length + 2);
        }

        public override void WriteCommandBytes(byte[] buffer, int offset)
        {
            LittleEndianWriter.WriteUInt16(buffer, offset + 0, StructureSize);
            LittleEndianWriter.WriteUInt16(buffer, offset + 2, Reserved);
        }

        public override int CommandLength
        {
            get
            {
                return DeclaredSize;
            }
        }
    }
}
